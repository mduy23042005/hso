using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Controller : MonoBehaviour, IUpdatable
{
    private float moveSpeed = 4f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastMove = new Vector2(0, -1);
    private Animator animator;

    [SerializeField] private SpriteRenderer hairRenderer;
    [SerializeField] private List<Hair> hairList;

    private int currentHair = 0;
    public void Next()
    {
        currentHair++;
        if (currentHair >= hairList.Count) currentHair = 0;
        Debug.Log("HairRenderer: " + hairRenderer);
        UpdateHairSprite();
    }

    public void Prev()
    {
        currentHair--;
        if (currentHair < 0) currentHair = hairList.Count - 1;
        Debug.Log("HairRenderer: " + hairRenderer);

        UpdateHairSprite();
    }

    private void UpdateHairSprite()
    {
        Hair set = hairList[currentHair];

        if (lastMove.y > 0)
            hairRenderer.sprite = set.back;
        else if (lastMove.y < 0)
            hairRenderer.sprite = set.front;
        else if (lastMove.x != 0)
            hairRenderer.sprite = set.side;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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
        UpdateAnimation();

    }
    private void LateUpdate()
    {
        UpdateHairSprite();
    }
    private void MoveKeyboard()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        MoveStop();
    }
    private void MoveStop()
    {
        if (movement != Vector2.zero)
        {
            lastMove = movement;
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    private void UpdateAnimation()
    {
        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetBool("isMove", true);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        else
        {
            animator.SetBool("isMove", false);
            animator.SetFloat("LastHorizontal", lastMove.x);
            animator.SetFloat("LastVertical", lastMove.y);
        }
    }
}
