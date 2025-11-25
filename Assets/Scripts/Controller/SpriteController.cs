using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteController : MonoBehaviour, IUpdatable
{
    private List<SpriteResolver> resolvers;
    private Animator animator;
    private int lastFrame = -1;
    private string lastState = "";
    private Controller controller;

    [Header("Chỉ định sprite nào của player sẽ bị thay thế")]
    [SerializeField] private List<SpriteLibrary> spriteLibrary;

    [Header("Danh sách sprite sẽ thay thế")]
    [SerializeField] private List<LegArmorLibraries> legArmorLibraries;
    [SerializeField] private List<ArmorLibraries> armorLibraries;
    [SerializeField] private List<HeadLibraries> headLibraries;
    [SerializeField] private List<HelmetLibraries> helmetLibraries;
    [SerializeField] private List<HairLibraries> hairLibraries;
    [SerializeField] private List<WeaponLibraries> weaponLibraries;

    private int currentLegArmor = 0;
    private int currentArmor = 0;
    private int currentHead = 0;
    private int currentHelmet = 0;
    private int currentHair = 0;
    private int currentWeapon = 0;

    private APIManager api;

    void Awake()
    {
        // Lấy tất cả SpriteResolver trong object con
        resolvers = GetComponentsInChildren<SpriteResolver>().ToList();
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller>();
        api = Object.FindFirstObjectByType<APIManager>();
    }
    void Start()
    {
        _ = ReadDatabase();
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
    public void OnUpdate() { }
    public void OnLateUpdate()
    {
        UpdateSprite();
    }
    public void OnFixedUpdate() { }

    public void NextHead()
    {
        EquipHead(currentHead + 1);
    }
    public List<HairLibraries> GetHair()
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

    private async Task ReadDatabase()
    {
        int idAccount = LogInController.GetIDAccount();

        try
        {
            string urlItems = $"{api.GetApiUrl()}/api/account/{idAccount}/equipment?idAccount={idAccount}";
            HttpResponseMessage res = await api.GetHttpClient().GetAsync(urlItems);
            string json = await res.Content.ReadAsStringAsync();
            List<Account_Equipment> equipment = JsonConvert.DeserializeObject<List<Account_Equipment>>(json);

            var weaponData = equipment[0].IDItem0_1;
            var helmetData = equipment[1].IDItem0_1;
            var armorData = equipment[2].IDItem0_1;
            var legArmorData = equipment[3].IDItem0_1;

            currentWeapon = weaponLibraries.FindIndex(w => w.idWeapon == weaponData);
            currentHelmet = helmetLibraries.FindIndex(h => h.idHelmet == helmetData);
            currentArmor = armorLibraries.FindIndex(a => a.idArmor == armorData);
            currentLegArmor = legArmorLibraries.FindIndex(la => la.idLegArmor == legArmorData);

            EquipLegArmor(currentLegArmor);
            EquipArmor(currentArmor);
            EquipHelmet(currentHelmet);
            EquipWeapon(currentWeapon);
            EquipHair(currentHair);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"lỗi đọc database cho sprite: {ex.Message}");
            return;
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

        for (int i = 0; i < resolvers.Count; i++)
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
            float t = state.normalizedTime % 1f;

            float[] moveChangeTimes = { 0.0f, 0.5f }; // Clip dài 0:40 giây, đổi frame ở 0 / 0.4, 0.2 / 0.4

            int frame = GetFrameByTime(t, moveChangeTimes);

            if (lastState != "Stand" + direction)
            {
                lastFrame = -1;
                lastState = "Stand" + direction;
                SetAllResolvers("Stand", $"Stand{direction}");
            }
                foreach (var r in resolvers)
                {
                    if (r != null && r.spriteLibrary != null && r.gameObject.name == "4_0_0")
                    {
                        r.SetCategoryAndLabel("Stand", $"Stand{direction}Frame{frame}");
                        r.ResolveSpriteToSpriteRenderer();
                    }
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

            float[] moveChangeTimes = { 0.0f, 0.6667f }; // Clip dài 0:15 giây, đổi frame ở 0 / 0.15, 0.1 / 0.15

            int frame = GetFrameByTime(t, moveChangeTimes);

            if (frame != lastFrame || lastState != "Atk" + direction)
            {
                lastFrame = frame;
                lastState = "Atk" + direction;
                SetAllResolvers("Atk", $"Atk{direction}Frame{frame}");
            }
        }
        //Injured
        if (state.IsName("Injured"))
        {
            float t = state.normalizedTime % 1f;

            float[] moveChangeTimes = { 0.0f, 0.5f }; // Clip dài 0:20 giây, đổi frame ở 0 / 0.2, 0.1 / 0.2

            int frame = GetFrameByTime(t, moveChangeTimes);

            if (frame != lastFrame || lastState != "Injured" + direction)
            {
                lastFrame = frame;
                lastState = "Injured" + direction;
                foreach (var r in resolvers)
                {
                    if (r != null && r.spriteLibrary != null && r.gameObject.name == "4_0_0")
                    {
                        r.SetCategoryAndLabel("Injured", $"Injured{direction}Frame{frame}");
                        r.ResolveSpriteToSpriteRenderer();
                    }
                }
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
    public void RefreshCharacterSprite()
    {
        _ = ReadDatabase(); // Gọi lại logic load item từ database
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