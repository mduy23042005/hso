using UnityEngine;

public class Controller : MonoBehaviour
{
    private float moveSpeed = 4f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
        Debug.Log(movement);
    }

    private void FixedUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.x != 0 && movement.y != 0)
        {
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
                movement.y = 0;
            else
                movement.x = 0;
        }
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
    }
}
