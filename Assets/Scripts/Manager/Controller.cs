using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Controller : MonoBehaviour, IUpdatable
{
    private float moveSpeed = 4f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastMove = new Vector2(0, -1);
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
        RegisterDontDestroyOnLoad();
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
        UpdateAnimation();
    }
    public void RegisterDontDestroyOnLoad()
    {
        GameManager.Instance.RegisterPersistent(this);
    }

    //Hàm di chuyển bằng bàn phím, có thể override trong Demo.cs
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
    //Hàm được gọi bằng event trong animation
    private void AttackAnimationEnd()
    {
        animator.SetBool("isAtk", false);
        UpdateLastMoveToAnimator();
    }
    private void InjuredAnimationEnd()
    {
        animator.SetBool("isInjured", false);
        UpdateLastMoveToAnimator();
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
            animator.SetBool("isMove", false);
            animator.SetBool("isAtk", true);
            UpdateLastMoveToAnimator();
        }
        // Injured
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetBool("isInjured", true);
            UpdateLastMoveToAnimator();
        }
    }
}
