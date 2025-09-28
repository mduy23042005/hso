using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Controller : MonoBehaviour, IUpdatable
{
    private float moveSpeed = 6f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastMove = new Vector2(0, -1);
    private Vector2 targetPosition;
    private bool movingHorizontalFirst = false;
    private bool isMovingToTarget = false;
    private Animator animator;
    private Menu menu;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        menu = FindAnyObjectByType<Menu>(FindObjectsInactive.Include);

        animator.SetFloat("LastHorizontal", 0);
        animator.SetFloat("LastVertical", -1);
    }
    private void OnEnable()
    {
        GameManager.Instance.Register(this);
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Unregister(this);
        }
    }
    public void OnUpdate()
    {
        MoveKeyboard();
        MoveMouse();
        UpdateAnimation();
    }
    public void RegisterDontDestroyOnLoad()
    {
        GameManager.Instance.RegisterPersistent(this);
    }

    //Hàm di chuyển có thể override trong Demo.cs
    protected virtual void MoveKeyboard()
    {
        if (menu == null || !menu.getIsActive())
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            MoveStop();
        }
        else
        {
            return;
        }
    }
    protected virtual void MoveMouse()
    {
        if ((menu == null || !menu.getIsActive()) && Input.GetMouseButtonDown(0))
        {
            // Lấy tọa độ click
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            // Quyết định hướng ưu tiên
            float deltaX = Mathf.Abs(targetPosition.x - rb.position.x);
            float deltaY = Mathf.Abs(targetPosition.y - rb.position.y);
            movingHorizontalFirst = deltaX > deltaY;

            isMovingToTarget = true;
        }

        if (isMovingToTarget)
        {
            Vector2 currentPos = rb.position;
            float deltaX = targetPosition.x - currentPos.x;
            float deltaY = targetPosition.y - currentPos.y;

            // Nếu còn khoảng cách đáng kể thì tiếp tục di chuyển
            if (Mathf.Abs(deltaX) > 0.1f || Mathf.Abs(deltaY) > 0.1f)
            {
                if (movingHorizontalFirst)
                {
                    // Đi ngang trước
                    if (Mathf.Abs(deltaX) > 0.1f)
                    {
                        movement = new Vector2(Mathf.Sign(deltaX), 0);
                    }
                    else
                    {
                        movement = new Vector2(0, Mathf.Sign(deltaY));
                    }
                }
                else
                {
                    // Đi dọc trước
                    if (Mathf.Abs(deltaY) > 0.1f)
                    {
                        movement = new Vector2(0, Mathf.Sign(deltaY));
                    }
                    else
                    {
                        movement = new Vector2(Mathf.Sign(deltaX), 0);
                    }
                }
            }
            else
            {
                // Đến nơi → dừng lại
                MoveStop();
                movement = Vector2.zero;
                isMovingToTarget = false;
            }
        }
    }

    private void MoveStop()
    {
        if (movement != Vector2.zero)
        {
            lastMove = movement;
        }
    }
    protected virtual void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    //Getter - Setter cho các biến chuyển động
    public Vector2 getMovement()
    {
        return movement;
    }
    public Vector2 getLastMove()
    {
        return lastMove;
    }
    public void setMovement(Vector2 value)
    {
        movement = value;
    }
    public void setLastMove(Vector2 value)
    {
        lastMove = value;
    }

    //Cập nhật các thông số chuyển động cho animator
    private void UpdateMoveToAnimator()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
    }
    private void UpdateLastMoveToAnimator()
    {
        animator.SetFloat("LastHorizontal", lastMove.x);
        animator.SetFloat("LastVertical", lastMove.y);
    }
    //Cập nhật animation tạm thời trước khi có hệ thống animation tương tác hoàn chỉnh
    private void UpdateAnimation()
    {
        // Stand
        if (movement.x == 0 && movement.y == 0)
        {
            animator.SetBool("isMove", false);
            UpdateLastMoveToAnimator();
        }
        // Move
        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetBool("isMove", true);
            UpdateMoveToAnimator();
        }
        // Atk
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("Atk");
            UpdateLastMoveToAnimator();
        }
        // Injured
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Injured");
            UpdateLastMoveToAnimator();
        }
    }
}
