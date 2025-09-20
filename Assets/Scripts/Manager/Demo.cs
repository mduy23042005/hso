using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour, IUpdatable
{
    private Vector2 movement;
    private Vector2 lastMove = new Vector2(0, -1);
    private bool isMoving = false;
    private Animator animator;

    [SerializeField] private SpriteRenderer hairRenderer;
    [SerializeField] private List<Hair> hairList;
    private int currentHair = 0;

    [SerializeField] private List<SpriteRenderer> armorRenderer;
    [SerializeField] private List<Armor> armorList;
    private int currentArmor = 0;
    [SerializeField] private float fps = 10f;

    public void NextArmor()
    {
        if (armorList.Count == 0) return; // tránh lỗi nếu list rỗng
        currentArmor = (currentArmor + 1) % armorList.Count;
        UpdateSprite();
    }

    public void PrevArmor()
    {
        if (armorList.Count == 0) return;
        currentArmor = (currentArmor - 1 + armorList.Count) % armorList.Count;
        UpdateSprite();
    }

    public void NextHair()
    {
        currentHair++;
        if (currentHair >= hairList.Count) currentHair = 0;
        UpdateSprite();
    }
    public void PrevHair()
    {
        currentHair--;
        if (currentHair < 0) currentHair = hairList.Count - 1;
        UpdateSprite();
    }
    private void UpdateSprite()
    {
        Hair setHair = hairList[currentHair];
        switch (true)
        {
            case bool _ when lastMove.y > 0:
                hairRenderer.sprite = setHair.back;
                break;

            case bool _ when lastMove.y < 0:
                hairRenderer.sprite = setHair.front;
                break;

            case bool _ when lastMove.x != 0:
                hairRenderer.sprite = setHair.side;
                break;
        }

        Armor setArmor = armorList[currentArmor];
        foreach (var renderer in armorRenderer)
        {
            //Stand
            if (!isMoving)
            {
                switch (true)
                {
                    case bool _ when lastMove.y > 0:
                        renderer.sprite = setArmor.standBack;
                        break;

                    case bool _ when lastMove.y < 0:
                        renderer.sprite = setArmor.standFront;
                        break;

                    case bool _ when lastMove.x != 0:
                        renderer.sprite = setArmor.standSide;
                        break;
                }
            }
            //Move
            else
            {
                switch (true)
                {
                    case bool _ when movement.y > 0:
                        renderer.sprite = setArmor.moveBack[0];
                        break;

                    case bool _ when movement.y < 0:
                        renderer.sprite = setArmor.moveFront[0];
                        break;

                    case bool _ when movement.x != 0:
                        renderer.sprite = setArmor.moveSide[0];
                        break;
                }
            }
        }
    }
    public int getCurrentHair()
    {
        return currentHair;
    }
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
    private void LateUpdate()
    {
        UpdateSprite();
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
