using UnityEditor;
using UnityEngine;

public class Demo : Controller
{
    private Animator animatorChild;
    private GameObject player;
    private Demo demo;
    private void Awake()
    {
        animatorChild = GetComponent<Animator>();
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
    protected override void MoveKeyboard()
    {
        return;
    }
    protected override void MoveMouse()
    {
        return;
    }

    public override void OnUpdate()
    {
        LeftClick();
    }
    public override void OnLateUpdate()
    {
        return;
    }
    public override void OnFixedUpdate()
    {
        return;
    }

    protected override void LeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Lấy vị trí chuột trong thế giới
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    //Tiếp nhận gameobject player bị click
                    player = hit.collider.gameObject;
                    //Tạo biến demo hứng component demo từ player
                    demo = player.GetComponent<Demo>();
                    demo.UpdateAnimation();
                    return;
                }
            }
        }
    }
    protected override void UpdateAnimation()
    {
        animatorChild.SetTrigger("Atk");
    }
}
