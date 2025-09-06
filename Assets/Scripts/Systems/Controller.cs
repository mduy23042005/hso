using UnityEngine;

public class Controller : MonoBehaviour, IUpdatable
{
    private float moveSpeed = 4f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastMove = new Vector2(0, -1);
    private Animator animator;

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
            GameManager.Instance.Unregister(this);
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

        if (movement.x != 0 && movement.y != 0)
        {
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                movement.y = 0;
            }
            else
            {
                movement.x = 0;
            }
        }
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
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }

        animator.SetFloat("LastHorizontal", lastMove.x);
        animator.SetFloat("LastVertical", lastMove.y);
    }
}
