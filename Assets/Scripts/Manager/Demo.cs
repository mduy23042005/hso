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
    }
    protected override void FixedUpdate()
    {
        return;
    }
}
