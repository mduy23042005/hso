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
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        Debug.Log(movement);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        UpdateAnimation();
    }
    private void UpdateAnimation()
    {
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);

        bool isMove = (movement.x != 0 || movement.y != 0);
        animator.SetBool("isMove", isMove);

        if (isMove)
        {
            // Ưu tiên theo hướng
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                if (movement.x > 0)
                    animator.SetInteger("direction", 3); // Right
                else
                    animator.SetInteger("direction", 2); // Left
            }
            else
            {
                if (movement.y > 0)
                    animator.SetInteger("direction", 1); // Back
                else
                    animator.SetInteger("direction", 0); // Front
            }
        }
    }
}
