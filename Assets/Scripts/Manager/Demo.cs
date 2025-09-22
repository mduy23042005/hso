using UnityEngine;

public class Demo : Controller
{
    protected override void MoveKeyboard()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        setMovement(movement);

        if (getMovement() != Vector2.zero)
        {
            setLastMove(movement);
        }

        UpdateAnimationOnly();
    }

    private void UpdateAnimationOnly()
    {
        Animator animator = GetComponent<Animator>();

        // Stand
        if (getMovement() == Vector2.zero)
        {
            animator.SetBool("isMove", false);
            animator.SetFloat("LastHorizontal", getLastMove().x);
            animator.SetFloat("LastVertical", getLastMove().y);
        }
        // Move
        else
        {
            animator.SetBool("isMove", true);
            animator.SetFloat("Horizontal", getMovement().x);
            animator.SetFloat("Vertical", getMovement().y);
        }
    }
    protected override void FixedUpdate()
    {
        return;
    }
}
