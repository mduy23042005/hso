using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour, IUpdatable
{
    [Header("Danh sách các ô hành trang")]
    [SerializeField] private List<Image> equipmentSlots;
    [SerializeField] private InventoryController inventoryController;

    private APIManager api;
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
        api = FindObjectOfType<APIManager>();
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
        _ = ReadDatabase();
    }

    // Đọc dữ liệu từ database và hiển thị vào Equipment Slots
    private async Task ReadDatabase()
    {
        int idAccount = LogInController.GetIDAccount();
        string urlItems = $"{api.GetApiUrl()}/api/account/{idAccount}/equipment?idAccount={idAccount}";

        try
        {
            HttpResponseMessage res = await api.GetHttpClient().GetAsync(urlItems);
            string json = await res.Content.ReadAsStringAsync();
            List<Account_Equipment> equipment = JsonConvert.DeserializeObject<List<Account_Equipment>>(json);

            idWeapon = equipment[0].IDItem0_1;
            idHelmet = equipment[1].IDItem0_1;
            idArmor = equipment[2].IDItem0_1;
            idLegArmor = equipment[3].IDItem0_1;
            idGloves = equipment[4].IDItem0_1;
            idShoes = equipment[5].IDItem0_1;
            idRing1 = equipment[6].IDItem0_1;
            idRing2 = equipment[7].IDItem0_1;
            idNecklace = equipment[8].IDItem0_1;
            idMedal = equipment[9].IDItem0_1;
            idSkin = equipment[10].IDItem0_1;
            idCloak = equipment[11].IDItem0_1;
            idWing = equipment[12].IDItem0_1;
            idSkinWing = equipment[13].IDItem0_1;
            idMounts = equipment[14].IDItem0_1;
            idPet = equipment[15].IDItem0_1;

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
        catch (System.Exception ex)
        {
            Debug.LogError("Lỗi khi lấy equipment: " + ex.Message);
        }
    }
    public void RefreshEquipmentUI()
    {
        _ = ReadDatabase(); // Gọi lại logic load item từ database
    }
    public int[] GetEquipmentSlotsArray()
    {
        return idEquipmentSlotsArray;
    }
}
