using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteController : MonoBehaviour, IUpdatable
{
    private SpriteResolver[] resolvers;
    private Animator animator;
    private int lastFrame = -1;
    private string lastState = "";

    [Header("Chỉ định sprite nào của player sẽ bị thay thế")]
    [SerializeField] private SpriteLibrary[] spriteLibrary;

    [Header("Danh sách sprite sẽ thay thế")]
    [SerializeField] private SpriteLibraryAsset[] legArmorLibraries;
    [SerializeField] private SpriteLibraryAsset[] armorLibraries;

    private int currentArmor = 0;
    private int currentLegArmor = 0;

    void Awake()
    {
        // Lấy tất cả SpriteResolver trong object con
        resolvers = GetComponentsInChildren<SpriteResolver>();
        animator = GetComponent<Animator>();
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
    public void RegisterDontDestroyOnLoad()
    {
        GameManager.Instance.RegisterPersistent(this);
    }
    public void OnUpdate()
    {
        UpdateSprite();
    }

    public void NextLegArmor()
    {
        EquipLegArmor(currentLegArmor + 1);
    }
    public void PrevLegArmor()
    {
        if (currentLegArmor - 1 < 0)
        {
            return;
        }
        EquipLegArmor(currentLegArmor - 1);
    }
    public void NextArmor()
    {
        EquipArmor(currentArmor + 1);
    }
    public void PrevArmor()
    {
        if (currentArmor - 1 < 0)
        {
            return;
        }
        EquipArmor(currentArmor - 1);
    }

    private void EquipLegArmor(int legArmorIndex)
    {
        if (legArmorIndex < 0 || legArmorIndex >= legArmorLibraries.Length)
        {
            Debug.LogWarning("LegArmor index không hợp lệ!");
            return;
        }

        currentLegArmor = legArmorIndex;
        spriteLibrary[0].spriteLibraryAsset = legArmorLibraries[legArmorIndex];

        Debug.Log($"Đã equip LegArmor {legArmorIndex}");
    }
    private void EquipArmor(int armorIndex)
    {
        if (armorIndex < 0 || armorIndex >= armorLibraries.Length)
        {
            Debug.LogWarning("Armor index không hợp lệ!");
            return;
        }

        currentArmor = armorIndex;
        spriteLibrary[1].spriteLibraryAsset = armorLibraries[armorIndex];

        Debug.Log($"Đã equip Armor {armorIndex}");
    }

    private void UpdateSprite()
    {
        if (animator == null) return;

        for (int i = 0; i < resolvers.Length; i++)
        {
            if (resolvers[i] == null)
                continue;
        }

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        // Lấy hướng từ Animator (LastHorizontal, LastVertical)
        float h = animator.GetFloat("Horizontal");
        float v = animator.GetFloat("Vertical");

        string direction = "Front";
        if (Mathf.Abs(h) > Mathf.Abs(v))
        {
            direction = h > 0 ? "Right" : "Left";
        }
        else
        {
            direction = v > 0 ? "Back" : "Front";
        }

        // Stand
        if (state.IsName("Stand"))
        {
            if (lastState != "Stand" + direction)
            {
                lastFrame = -1;
                lastState = "Stand" + direction;
                SetAllResolvers("Stand", $"Stand{direction}");
            }
        }
        // Move
        if (state.IsName("Move"))
        {
            float t = state.normalizedTime % 1f;
            int frame = (t < 0.5f) ? 0 : 1;

            if (frame != lastFrame || lastState != "Move" + direction)
            {
                lastFrame = frame;
                lastState = "Move" + direction;
                SetAllResolvers("Move", $"Move{direction}Frame{frame}");
            }
        }
        // Attack
        if (state.IsName("Atk"))
        {
            float t = state.normalizedTime % 1f;
            int frame = (t < 0.5f) ? 0 : 1;

            if (frame != lastFrame || lastState != "Atk" + direction)
            {
                lastFrame = frame;
                lastState = "Atk" + direction;
                SetAllResolvers("Atk", $"Atk{direction}Frame{frame}");
            }
        }
        // Die
        if (state.IsName("Die"))
        {
            if (lastState != "Die")
            {
                lastFrame = -1;
                lastState = "Die";
                SetAllResolvers("Die", $"Die{direction}Frame0");
            }
        }
    }
    private void SetAllResolvers(string category, string label)
    {
        foreach (var r in resolvers)
        {
            if (r != null && r.spriteLibrary != null)  // ✅ Check tránh null
            {
                r.SetCategoryAndLabel(category, label);
            }
        }
    }
}
