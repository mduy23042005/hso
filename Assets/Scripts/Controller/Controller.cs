using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour, IUpdatable
{
    private float moveSpeed = 6f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastMove = new Vector2(0, -1);
    private Vector2 targetPosition;
    private bool movingHorizontalFirst = false;
    private bool isMovingToTarget = false;
    private Animator animator;
    private MenuController menu;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        menu = FindAnyObjectByType<MenuController>(FindObjectsInactive.Include);
    }
    private void OnEnable()
    {
        GameManager.Instance.Register(this);
        animator.SetFloat("LastHorizontal", 0);
        animator.SetFloat("LastVertical", -1);
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Unregister(this);
        }
    }
    public virtual void OnUpdate()
    {
        MoveKeyboard();
        MoveMouse();
        UpdateAnimation();
    }
    public virtual void OnLateUpdate()
    {

    }    
    public virtual void OnFixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    public void RegisterDontDestroyOnLoad()
    {
        GameManager.Instance.RegisterPersistent(this);
    }

    protected virtual void LeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Lấy vị trí chuột trong thế giới
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);

            if (hit.collider != null)
            {
                // Nếu collider có tag "Mob" thì chỉ debug, không di chuyển
                if (hit.collider.CompareTag("Mob"))
                {
                    Debug.Log($"Clicked on Mob: {hit.collider.name}");
                    isMovingToTarget = false;
                    return;
                }
            }

            // Nếu không click vào mob thì tiến hành di chuyển đến vị trí click
            targetPosition = clickPos;

            // Quyết định hướng ưu tiên
            float deltaX = Mathf.Abs(targetPosition.x - rb.position.x);
            float deltaY = Mathf.Abs(targetPosition.y - rb.position.y);
            movingHorizontalFirst = deltaX > deltaY;

            isMovingToTarget = true;
        }
    }

    //Hàm di chuyển có thể override trong Demo.cs
    protected virtual void MoveKeyboard()
    {
        if (menu == null || !menu.getIsActive())
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            if (movement.x != 0)
            {
                movement.y = 0;
            }
            if (movement.y != 0)
            {
                movement.x = 0;
            }
            MoveStop();
        }
        else
        {
            return;
        }
    }
    protected virtual void MoveMouse()
    {
        if ((menu != null && menu.getIsActive()) || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        LeftClick();
        if (isMovingToTarget)
        {
            Vector2 currentPos = rb.position;
            float deltaX = targetPosition.x - currentPos.x;
            float deltaY = targetPosition.y - currentPos.y;

            // Nếu còn khoảng cách đáng kể thì tiếp tục di chuyển
            if (Mathf.Abs(deltaX) > 0.1f || Mathf.Abs(deltaY) > 0.1f)
            {
                if (movingHorizontalFirst)
                {
                    // Đi ngang trước
                    if (Mathf.Abs(deltaX) > 0.1f)
                    {
                        movement = new Vector2(Mathf.Sign(deltaX), 0);
                    }
                    else
                    {
                        movement = new Vector2(0, Mathf.Sign(deltaY));
                    }
                }
                else
                {
                    // Đi dọc trước
                    if (Mathf.Abs(deltaY) > 0.1f)
                    {
                        movement = new Vector2(0, Mathf.Sign(deltaY));
                    }
                    else
                    {
                        movement = new Vector2(Mathf.Sign(deltaX), 0);
                    }
                }
                MoveStop();
            }
            else
            {
                isMovingToTarget = false;
                return;
            }
        }
    }

    private void MoveStop()
    {
        if (movement != Vector2.zero)
        {
            lastMove = movement;
        }
    }

    //Getter - Setter cho các biến chuyển động
    public Vector2 GetMovement()
    {
        return movement;
    }
    public Vector2 GetLastMovement()
    {
        return lastMove;
    }
    public void SetMovement(Vector2 value)
    {
        movement = value;
    }
    public void SetLastMovement(Vector2 value)
    {
        lastMove = value;
    }
    public bool GetIsMovingToTarget()
    {
        return isMovingToTarget;
    }

    //Cập nhật các thông số chuyển động cho animator
    private void UpdateMoveToAnimator()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
    }
    private void UpdateLastMoveToAnimator()
    {
        animator.SetFloat("LastHorizontal", lastMove.x);
        animator.SetFloat("LastVertical", lastMove.y);
    }
    //Cập nhật animation tạm thời trước khi có hệ thống animation tương tác hoàn chỉnh
    protected virtual void UpdateAnimation()
    {
        // Stand
        if (movement.x == 0 && movement.y == 0)
        {
            animator.SetBool("isMove", false);
            UpdateLastMoveToAnimator();
        }
        // Move
        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetBool("isMove", true);
            UpdateMoveToAnimator();
        }
        // Atk
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("Atk");
            animator.SetBool("isMove", false);
            UpdateLastMoveToAnimator();
        }
        // Injured
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Injured");
            animator.SetBool("isMove", false);
            UpdateLastMoveToAnimator();
        }
    }
}
