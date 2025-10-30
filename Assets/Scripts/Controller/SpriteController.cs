using HSOEntities.Models;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteController : MonoBehaviour, IUpdatable
{
    private SpriteResolver[] resolvers;
    private Animator animator;
    private int lastFrame = -1;
    private string lastState = "";
    private Controller controller;

    [Header("Chỉ định sprite nào của player sẽ bị thay thế")]
    [SerializeField] private UnityEngine.U2D.Animation.SpriteLibrary[] spriteLibrary;

    [Header("Danh sách sprite sẽ thay thế")]
    [SerializeField] private LegArmorLibraries[] legArmorLibraries;
    [SerializeField] private ArmorLibraries[] armorLibraries;
    [SerializeField] private HeadLibraries[] headLibraries;
    [SerializeField] private HelmetLibraries[] helmetLibraries;
    [SerializeField] private HairLibraries[] hairLibraries;
    [SerializeField] private WeaponLibraries[] weaponLibraries;

    private int currentLegArmor = 0;
    private int currentArmor = 0;
    private int currentHead = 0;
    private int currentHelmet = 0;
    private int currentHair = 0;
    private int currentWeapon = 0;

    public static SpriteController instance;
    private HSOEntities.Models.HSOEntities db;

    void Awake()
    {
        // Lấy tất cả SpriteResolver trong object con
        resolvers = GetComponentsInChildren<SpriteResolver>();
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller>();
        db = SQLConnectionManager.GetData();
    }
    void Start()
    {
        ReadDatabase();
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

    public void CheckIncrease()
    {
        EquipLegArmor((currentLegArmor + 1) % legArmorLibraries.Length);
        EquipArmor((currentArmor + 1) % armorLibraries.Length);
        EquipHelmet((currentHelmet + 1) % helmetLibraries.Length);
        EquipWeapon((currentWeapon + 1) % weaponLibraries.Length);
        EquipHair((currentHair + 1) % hairLibraries.Length);

        UpdateDatabase();
    }
    public void CheckDecrease()
    {
        EquipLegArmor((currentLegArmor - 1 + legArmorLibraries.Length) % legArmorLibraries.Length);
        EquipArmor((currentArmor - 1 + armorLibraries.Length) % armorLibraries.Length);
        EquipHelmet((currentHelmet - 1 + helmetLibraries.Length) % helmetLibraries.Length);
        EquipWeapon((currentWeapon - 1 + weaponLibraries.Length) % weaponLibraries.Length);
        EquipHair((currentHair - 1 + hairLibraries.Length) % hairLibraries.Length);

        UpdateDatabase();
    }

    public void NextHead()
    {
        EquipHead(currentHead + 1);
    }
    public HairLibraries[] GetHair()
    {
        return hairLibraries;
    }

    private void EquipLegArmor(int legArmorIndex)
    {
        currentLegArmor = legArmorIndex;
        spriteLibrary[0].spriteLibraryAsset = legArmorLibraries[legArmorIndex].legArmorLibrariesAsset;
    }
    private void EquipArmor(int armorIndex)
    {
        currentArmor = armorIndex;
        spriteLibrary[1].spriteLibraryAsset = armorLibraries[armorIndex].armorLibrariesAsset;
    }
    private void EquipHead(int headIndex)
    {
        currentHead = headIndex;
        spriteLibrary[2].spriteLibraryAsset = headLibraries[headIndex].headLibrariesAsset;
    }
    private void EquipHelmet(int helmetIndex)
    {
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
    }
    private void EquipHair(int hairIndex)
    {
        currentHair = hairIndex;
        spriteLibrary[4].spriteLibraryAsset = hairLibraries[hairIndex].hairLibrariesAsset;
    }
    private void EquipWeapon(int weaponIndex)
    {
        currentWeapon = weaponIndex;
        spriteLibrary[5].spriteLibraryAsset = weaponLibraries[weaponIndex].weaponFrontLibraries;
        spriteLibrary[6].spriteLibraryAsset = weaponLibraries[weaponIndex].weaponBackLibraries;
    }

    private void UpdateDatabase()
    {
        // 1. Lấy item LegArmor theo IDItem0
        int idLegArmor = legArmorLibraries[currentLegArmor].idLegArmor;
        int idArmor = armorLibraries[currentArmor].idArmor;
        int idHelmet = helmetLibraries[currentHelmet].idHelmet;
        int idWeapon = weaponLibraries[currentWeapon].idWeapon;
        int idHair = hairLibraries[currentHair] != null ? currentHair : 0;

        var legArmorData = db.Item0.FirstOrDefault(item => item.IDItem0 == idLegArmor);
        var armorData = db.Item0.FirstOrDefault(item => item.IDItem0 == idArmor);
        var helmetData = db.Item0.FirstOrDefault(item => item.IDItem0 == idHelmet);
        var weaponData = db.Item0.FirstOrDefault(item => item.IDItem0 == idWeapon);

        legArmorLibraries[currentLegArmor].nameLegArmor = legArmorData != null ? legArmorData.NameItem0 : "Unknown LegArmor";
        armorLibraries[currentArmor].nameArmor = armorData != null ? armorData.NameItem0 : "Unknown Armor";
        helmetLibraries[currentHelmet].nameHelmet = helmetData != null ? helmetData.NameItem0 : "Unknown Helmet";
        weaponLibraries[currentWeapon].nameWeapon = weaponData != null ? weaponData.NameItem0 : "Unknown Weapon";

        Debug.Log($"Đang sử dụng LegArmor: {legArmorLibraries[currentLegArmor].nameLegArmor}");
        Debug.Log($"Đang sử dụng Armor: {armorLibraries[currentArmor].nameArmor}");
        Debug.Log($"Đang sử dụng Helmet: {helmetLibraries[currentHelmet].nameHelmet}");
        Debug.Log($"Đang sử dụng Weapon: {weaponLibraries[currentWeapon].nameWeapon}");
        Debug.Log($"Đang sử dụng Hair: {currentHair}");

        // 2. Lấy account đúng theo IDAccount
        int idAccount = LogInController.GetIDAccount();
        var account = db.Accounts.FirstOrDefault(acc => acc.IDAccount == idAccount);

        // 3. Cập nhật cột LegArmor trong bảng Account
        if (account != null)
        {
            account.LegArmor = legArmorData.IDItem0;
            account.Armor = armorData.IDItem0;
            account.Helmet = helmetData.IDItem0;
            account.Weapon = weaponData.IDItem0;
            account.Hair = idHair;
            db.SaveChanges();
            Debug.Log("Cập nhật trang bị nhân vật thành công trong bảng Account.");
        }
    }
    private void ReadDatabase()
    {
        int idAccount = LogInController.GetIDAccount();
        var account = db.Accounts.FirstOrDefault(acc => acc.IDAccount == idAccount);
        if (account != null)
        {
            var legArmorData = db.Item0.FirstOrDefault(item => item.IDItem0 == account.LegArmor);
            var armorData = db.Item0.FirstOrDefault(item => item.IDItem0 == account.Armor);
            var helmetData = db.Item0.FirstOrDefault(item => item.IDItem0 == account.Helmet);
            var weaponData = db.Item0.FirstOrDefault(item => item.IDItem0 == account.Weapon);

            currentLegArmor = legArmorLibraries.ToList().FindIndex(la => la.idLegArmor == legArmorData.IDItem0);
            currentArmor = armorLibraries.ToList().FindIndex(a => a.idArmor == armorData.IDItem0);
            currentHelmet = helmetLibraries.ToList().FindIndex(h => h.idHelmet == helmetData.IDItem0);
            currentWeapon = weaponLibraries.ToList().FindIndex(w => w.idWeapon == weaponData.IDItem0);
            if (account.Hair >= 0 && account.Hair < hairLibraries.Length) currentHair = account.Hair ?? 0;

            if (currentLegArmor < 0) currentLegArmor = 0;
            if (currentArmor < 0) currentArmor = 0;
            if (currentHelmet < 0) currentHelmet = 0;
            if (currentWeapon < 0) currentWeapon = 0;
            if (currentHair < 0) currentHair = 0;

            EquipLegArmor(currentLegArmor);
            EquipArmor(currentArmor);
            EquipHelmet(currentHelmet);
            EquipWeapon(currentWeapon);
            EquipHair(currentHair);
        }
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
        if (controller.GetIsMovingToTarget())
        {
            h = controller.GetMovement().x;
            v = controller.GetMovement().y;
        }
        else
        {
            h = controller.GetLastMovement().x;
            v = controller.GetLastMovement().y;
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