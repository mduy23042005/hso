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
    private Demo demo;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        menu = FindAnyObjectByType<Menu>();
        demo = FindAnyObjectByType<Demo>();

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
    private void MoveKeyboard()
    {
        if (!menu.getIsActive())
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
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    private void LateUpdate()
    {
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
