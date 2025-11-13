using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

public class EquipmentController : MonoBehaviour, IUpdatable
{
    [Header("Danh sách các ô hành trang")]
    [SerializeField] private List<Image> equipmentSlots;
    [SerializeField] private InventoryController inventoryController;

    private HSOEntities.Models.HSOEntities db;
    private int idWeapon;
    private int idArmor;
    private int idLegArmor;
    private int idHelmet;
    private int idGloves;
    private int idShoes;
    private int idRing1;
    private int idRing2;
    private int idNecklace;
    private int idMedal;
    private int idSkin;
    private int idPet;
    private int idCloak;
    private int idMounts;
    private int idWing;
    private int idSkinWing;

    private int[] idEquipmentSlotsArray = new int[16];

    private void Awake()
    {
        db = SQLConnectionManager.GetData();
    }
    
    public void OnUpdate() { }
    public void OnLateUpdate() { }
    public void OnFixedUpdate() { }
    public void RegisterDontDestroyOnLoad()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        ReadDatabase();
    }

    // Đọc dữ liệu từ database và hiển thị vào Equipment Slots
    private void ReadDatabase()
    {
        int idAccount = LogInController.GetIDAccount();
        var account = db.Accounts.FirstOrDefault(acc => acc.IDAccount == idAccount);

        if (account == null) return;

        idWeapon = account.Weapon ?? 0;
        idHelmet = account.Helmet ?? 0;
        idArmor = account.Armor ?? 0;
        idLegArmor = account.LegArmor ?? 0;
        idGloves = account.Gloves ?? 0;
        idShoes = account.Shoes ?? 0;
        idRing1 = account.Ring1 ?? 0;
        idRing2 = account.Ring2 ?? 0;
        idNecklace = account.Necklace ?? 0;
        idMedal = account.Medal ?? 0;
        idSkin = account.Skin ?? 0;
        idCloak = account.Cloak ?? 0;
        idWing = account.Wing ?? 0;
        idSkinWing = account.SkinWing ?? 0;
        idMounts = account.Mounts ?? 0;
        idPet = account.Pet ?? 0;

        // Danh sách ID tương ứng index
        idEquipmentSlotsArray = new int[]
        {
            idWeapon,   //0
            idHelmet,   //1
            idArmor,    //2
            idLegArmor, //3
            idGloves,   //4
            idShoes,    //5
            idRing1,    //6
            idRing2,    //7
            idNecklace, //8
            idMedal,    //9
            idSkin,     //10
            idCloak,    //11
            idWing,     //12
            idSkinWing, //13
            idMounts,   //14
            idPet       //15
        };

        for (int i = 0; i < equipmentSlots.Count && i < idEquipmentSlotsArray.Length; i++)
        {
            if (idEquipmentSlotsArray[i] == 0)
            {
                equipmentSlots[i].sprite = inventoryController.GetItemDefault(i + 1);
            }
            else
            {
                equipmentSlots[i].sprite = inventoryController.GetItem0(idEquipmentSlotsArray[i]);
            }
        }
    }
    public void RefreshEquipmentUI()
    {
        ReadDatabase(); // Gọi lại logic load item từ database
    }
    public int[] GetEquipmentSlotsArray()
    {
        return idEquipmentSlotsArray;
    }
}
