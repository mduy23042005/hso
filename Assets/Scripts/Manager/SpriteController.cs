using UnityEngine;
using UnityEngine.U2D.Animation;

[System.Serializable]
public class WeaponLibraries
{
    public SpriteLibraryAsset weaponBackLibraries;
    public SpriteLibraryAsset weaponFrontLibraries;
}
[System.Serializable]
public class HelmetLibraries
{
    public SpriteLibraryAsset helmetLibrariesAsset;
    public bool isHiddenHair = false;
}

public class SpriteController : MonoBehaviour, IUpdatable
{
    private SpriteResolver[] resolvers;
    private Animator animator;
    private int lastFrame = -1;
    private string lastState = "";
    private Controller controller;

    [Header("Chỉ định sprite nào của player sẽ bị thay thế")]
    [SerializeField] private SpriteLibrary[] spriteLibrary;

    [Header("Danh sách sprite sẽ thay thế")]
    [SerializeField] private SpriteLibraryAsset[] legArmorLibraries;
    [SerializeField] private SpriteLibraryAsset[] armorLibraries;
    [SerializeField] private SpriteLibraryAsset[] headLibraries;
    [SerializeField] private HelmetLibraries[] helmetLibraries;
    [SerializeField] private SpriteLibraryAsset[] hairLibraries;
    [SerializeField] private WeaponLibraries[] weaponLibraries;

    private int currentArmor = 0;
    private int currentLegArmor = 0;
    private int currentHead = 0;
    private int currentHelmet = 0;
    private int currentHair = 0;
    private int currentWeapon = 0;

    void Awake()
    {
        // Lấy tất cả SpriteResolver trong object con
        resolvers = GetComponentsInChildren<SpriteResolver>();
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller>();
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

    }
    public void OnLateUpdate()
    {
        UpdateSprite();
    }
    public void OnFixedUpdate()
    {
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
    public void NextHead()
    {
        EquipHead(currentHead + 1);
    }
    public void PrevHead()
    {
        if (currentHead - 1 < 0)
        {
            return;
        }
        EquipHead(currentHead - 1);
    }
    public void NextHelmet()
    {
        EquipHelmet(currentHelmet + 1);
    }
    public void PrevHelmet()
    {
        if (currentHelmet - 1 < 0)
        {
            return;
        }
        EquipHelmet(currentHelmet - 1);
    }
    public void NextHair()
    {
        EquipHair(currentHair + 1);
    }
    public void PrevHair()
    {
        if (currentHair - 1 < 0)
        {
            return;
        }
        EquipHair(currentHair - 1);
    }
    public void NextWeapon()
    {
        EquipWeapon(currentWeapon + 1);
    }
    public void PrevWeapon()
    {
        if (currentWeapon - 1 < 0)
        {
            return;
        }
        EquipWeapon(currentWeapon - 1);
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
    private void EquipHead(int headIndex)
    {
        if (headIndex < 0 || headIndex >= headLibraries.Length)
        {
            Debug.LogWarning("Head index không hợp lệ!");
            return;
        }

        currentHead = headIndex;
        spriteLibrary[2].spriteLibraryAsset = headLibraries[headIndex];

        Debug.Log($"Đã equip Head {headIndex}");
    }
    private void EquipHelmet(int helmetIndex)
    {
        if (helmetIndex < 0 || helmetIndex >= helmetLibraries.Length)
        {
            Debug.LogWarning("Helmet index không hợp lệ!");
            Debug.Log(helmetLibraries.Length);
            return;
        }

        currentHelmet = helmetIndex;
        spriteLibrary[3].spriteLibraryAsset = helmetLibraries[helmetIndex].helmetLibrariesAsset;

        if (helmetLibraries[helmetIndex].isHiddenHair)
        {
            spriteLibrary[4].gameObject.SetActive(false);
        }
        else
        {
            spriteLibrary[4].gameObject.SetActive(true);
        }

        Debug.Log($"Đã equip Helmet {helmetIndex}");
    }
    private void EquipHair(int hairIndex)
    {
        if (hairIndex < 0 || hairIndex >= hairLibraries.Length)
        {
            Debug.LogWarning("Hair index không hợp lệ!");
            return;
        }

        currentHair = hairIndex;
        spriteLibrary[4].spriteLibraryAsset = hairLibraries[hairIndex];
        Debug.Log($"Đã equip Hair {hairIndex}");
    }
    private void EquipWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weaponLibraries.Length)
        {
            Debug.LogWarning("Weapon index không hợp lệ!");
            return;
        }

        currentWeapon = weaponIndex;
        spriteLibrary[5].spriteLibraryAsset = weaponLibraries[weaponIndex].weaponFrontLibraries;
        spriteLibrary[6].spriteLibraryAsset = weaponLibraries[weaponIndex].weaponBackLibraries;

        Debug.Log($"Đã equip Weapon {weaponIndex}");
    }

    private string GetDirection(float h, float v)
    {
        if (Mathf.Abs(h) == 0 && Mathf.Abs(v) == 0)
            return "Front";

        if (Mathf.Abs(v) > 0.01f)
            return v > 0 ? "Back" : "Front";

        return h > 0 ? "Right" : "Left";
    }
    private int GetFrameByTime(float t, float[] changeTimes)
    {
        t %= 1f;

        for (int i = 0; i < changeTimes.Length; i++)
        {
            if (t < changeTimes[i])
                return Mathf.Max(0, i - 1);
        }

        return changeTimes.Length - 1;
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

        float h;
        float v;
        if (controller.getIsMovingToTarget())
        {
            h = controller.getMovement().x;
            v = controller.getMovement().y;
        }
        else
        {
            h = controller.getLastMovement().x;
            v = controller.getLastMovement().y;
        }

        string direction = GetDirection(h, v);

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

            float[] moveChangeTimes = { 0.0f, 0.5f }; // Clip dài 0:40 giây, đổi frame ở 0 / 0.4, 0.2 / 0.4

            int frame = GetFrameByTime(t, moveChangeTimes);

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

            float[] moveChangeTimes = { 0.0f, 0.6667f }; // Clip dài 0:15 giây, đổi frame ở 0/0.5, 0.1/0.15

            int frame = GetFrameByTime(t, moveChangeTimes);

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
                SetAllResolvers("Die", $"DieFrame0");
            }
        }
    }

    private void SetAllResolvers(string category, string label)
    {
        foreach (var r in resolvers)
        {
            if (r != null && r.spriteLibrary != null)
            {
                r.SetCategoryAndLabel(category, label);
                r.ResolveSpriteToSpriteRenderer();
            }
        }
    }
}