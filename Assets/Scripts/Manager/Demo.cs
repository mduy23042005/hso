using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour, IUpdatable
{
    private Vector2 movement;
    private Vector2 lastMove = new Vector2(0, -1);
    private bool isMoving = false;
    private Animator animator;

    private void Awake()
    {
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
    private void MoveKeyboard()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        isMoving = movement != Vector2.zero;
        if (isMoving) lastMove = movement;
        MoveStop();
    }
    private void MoveStop()
    {
        if (movement != Vector2.zero)
        {
            lastMove = movement;
        }
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
