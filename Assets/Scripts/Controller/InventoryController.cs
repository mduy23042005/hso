using HSOEntities.Models;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    #region Sprite list variable
    [Header("Item Default")]
    [SerializeField] private List<Sprite> itemDefault;
    [Header("Item0")]
    [SerializeField] private List<Sprite> item0;
    [Header("Item1")]
    [SerializeField] private List<Sprite> item1;
    [Header("Item2")]
    [SerializeField] private List<Sprite> item2;
    [Header("Item3")]
    [SerializeField] private List<Sprite> item3;
    [Header("Item4")]
    [SerializeField] private List<Sprite> item4;
    #endregion

    [Header("Các ô hành trang")]
    [SerializeField] private List<Image> inventorySlots;
    [SerializeField] private EquipmentController equipmentController;
    [SerializeField] private TMP_Text itemInfo;

    private Dictionary<int, Sprite> itemDefaultMap;
    private Dictionary<int, Sprite> item0Map;
    private Dictionary<int, Sprite> item1Map;
    private Dictionary<int, Sprite> item2Map;
    private Dictionary<int, Sprite> item3Map;
    private Dictionary<int, Sprite> item4Map;

    HSOEntities.Models.HSOEntities db;
    List<Account_Item0> inventoryItem0;
    private int idItem = 0;
    private int idSlot = 0;
    int[] equippedItems;

    private void Awake()
    {
        itemDefaultMap = ConvertListToMap(itemDefault);
        item0Map = ConvertListToMap(item0);
        item1Map = ConvertListToMap(item1);
        item2Map = ConvertListToMap(item2);
        item3Map = ConvertListToMap(item3);
        item4Map = ConvertListToMap(item4);

        db = SQLConnectionManager.GetData();
    }
    private void Start()
    {
        ReadDataBase();
    }
    private Dictionary<int, Sprite> ConvertListToMap(List<Sprite> list)
    {
        var map = new Dictionary<int, Sprite>();
        for (int i = 0; i < list.Count; i++)
        {
            map[i + 1] = list[i];
        }
        return map;
    }

    public Sprite GetItemDefault(int id)
    {
        return itemDefaultMap.TryGetValue(id, out var sprite) ? sprite : null;
    }
    public Sprite GetItem0(int id)
    {
        if (id < 0) return null;
        return item0Map.TryGetValue(id, out var sprite) ? sprite : null;
    }
    public Sprite GetItem1(int id)
    {
        if (id < 0) return null;
        return item1Map.TryGetValue(id, out var sprite) ? sprite : null;
    }
    public Sprite GetItem2(int id)
    {
        if (id < 0) return null;
        return item2Map.TryGetValue(id, out var sprite) ? sprite : null;
    }
    public Sprite GetItem3(int id)
    {
        if (id < 0) return null;
        return item3Map.TryGetValue(id, out var sprite) ? sprite : null;
    }
    public Sprite GetItem4(int id)
    {
        if (id < 0) return null;
        return item4Map.TryGetValue(id, out var sprite) ? sprite : null;
    }

    // Đọc dữ liệu từ database và hiển thị vào Inventory Slots
    private void ReadDataBase()
    {
        int idAccount = LogInController.GetIDAccount();
        var account = db.Accounts.FirstOrDefault(acc => acc.IDAccount == idAccount);

        if (account != null)
        {
            inventoryItem0 = db.Account_Item0.Where(item => item.IDAccount == idAccount).ToList();
            //Duyệt qua tất cả các item0 mà player sở hữu, player có 4 item0 thì hiển thị 4 item0 đó
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (i < inventoryItem0.Count && inventoryItem0[i].IDItem0 != 0)
                {
                    inventorySlots[i].sprite = GetItem0(inventoryItem0[i].IDItem0);
                }
                else
                {
                    inventorySlots[i].sprite = null;
                    inventorySlots[i].color = new Color(0, 0, 0, 0);
                }
            }
        }
    }
    // Đọc attribute của item trong equipment
    public void ReadAttributeInEquipment(int idSlot)
    {
        int idAccount = LogInController.GetIDAccount();
        var account = db.Accounts.FirstOrDefault(acc => acc.IDAccount == idAccount);

        equippedItems = equipmentController.GetEquipmentSlotsArray();

        idItem = equippedItems[idSlot];
        if (idItem == 0)
        {
            Debug.Log("Slot này chưa có item");
            return;
        }
        int requiredCategory = idSlot switch
        {
            0 => account.CateWeapon ?? 1,
            1 => account.CateHelmet ?? 1,
            2 => account.CateArmor ?? 1,
            3 => account.CateLegArmor ?? 1,
            4 => account.CateGloves ?? 1,
            5 => account.CateShoes ?? 1,
            6 => account.CateRing1 ?? 1,
            7 => account.CateRing2 ?? 1,
            8 => account.CateNecklace ?? 1,
            9 => account.CateMedal ?? 1,
            _ => 1
        };
        // Tiếp tục phần LINQ join để đọc attribute
        var result = (from itemAttr in db.Item0_Attribute
                      join attr in db.Attributes on itemAttr.IDAttribute equals attr.IDAttribute
                      join item0 in db.Item0 on itemAttr.IDItem0 equals item0.IDItem0
                      where itemAttr.IDItem0 == idItem && itemAttr.Category == requiredCategory
                      select new
                      {
                          ItemName = item0.NameItem0,
                          AttributeName = attr.NameAttribute,
                          Value = itemAttr.Value,
                          Category = itemAttr.Category
                      }).ToList();
        itemInfo.text = "";
        foreach (var r in result)
        {
            itemInfo.text += $"{r.Value} {r.AttributeName}\n";
        }
    }
    // Đọc attribute của item trong inventory
    public void ReadAttributeInInventory(int idSlot)
    {
        int idAccount = LogInController.GetIDAccount();
        var account = db.Accounts.FirstOrDefault(acc => acc.IDAccount == idAccount);

        int categoryItem = 0;

        if (idSlot >= 0 && idSlot < inventoryItem0.Count) // nếu id hợp lệ thì lấy idItem từ inventory
        {
            idItem = inventoryItem0[idSlot].IDItem0;
            this.idSlot = idSlot;
            categoryItem = inventoryItem0[idSlot].Category;
        }

        if (idItem == 0) // Slot trống hoặc ngoài phạm vi
        {
            Debug.Log("Slot này chưa có item");
            return;
        }

        var result = (from itemAttr in db.Item0_Attribute
                      join attr in db.Attributes on itemAttr.IDAttribute equals attr.IDAttribute
                      join item0 in db.Item0 on itemAttr.IDItem0 equals item0.IDItem0
                      where itemAttr.IDItem0 == idItem && itemAttr.Category == categoryItem
                      select new
                      {
                          ItemName = item0.NameItem0,
                          AttributeName = attr.NameAttribute,
                          Value = itemAttr.Value,
                          Category = itemAttr.Category
                      }).ToList();
        itemInfo.text = "";
        foreach (var r in result)
        {
            itemInfo.text += $"{r.Value} {r.AttributeName}\n";
        }
    }
    public void ClickEquipItem()
    {
        int idAccount = LogInController.GetIDAccount();
        var account = db.Accounts.FirstOrDefault(acc => acc.IDAccount == idAccount);

        if (idItem == 0)
        {
            Debug.Log("Chọn Item đi");
            return;
        }

        var itemToEquip = inventoryItem0.FirstOrDefault(i => i.IDItem0 == idItem);

        var inFoItemToEquip = db.Item0.Where(x => x.IDItem0 == itemToEquip.IDItem0).FirstOrDefault();

        // Kiểm tra IDSchool
        if (inFoItemToEquip.IDSchool != 0 && inFoItemToEquip.IDSchool != account.IDSchool)
        {
            Debug.LogWarning($"Không thể trang bị item ID {idItem}: Trường phái không phù hợp (Account School: {account.IDSchool}, Item School: {inFoItemToEquip.IDSchool})");
            itemInfo.text = "Không thể trang bị - Trường phái không phù hợp!";
            return;
        }
        
        switch (inFoItemToEquip.TypeItem0)
        {
            case "Weapon":
                var typeSupportW = account.Weapon;
                var cateSupportW = account.CateWeapon;
                account.Weapon = itemToEquip.IDItem0;
                account.CateWeapon = itemToEquip.Category;
                itemToEquip.IDItem0 = typeSupportW ?? 1;
                itemToEquip.Category = cateSupportW ?? 1;
                break;

            case "Helmet":
                var typeSupportH = account.Helmet;
                var cateSupportH = account.CateHelmet;
                account.Helmet = itemToEquip.IDItem0;
                account.CateHelmet = itemToEquip.Category;
                itemToEquip.IDItem0 = typeSupportH ?? 1;
                itemToEquip.Category = cateSupportH ?? 1;
                break;
            case "Armor":
                var typeSupportA = account.Armor;
                var cateSupportA = account.CateArmor;
                account.Armor = itemToEquip.IDItem0;
                account.CateArmor = itemToEquip.Category;
                itemToEquip.IDItem0 = typeSupportA ?? 1;
                itemToEquip.Category = cateSupportA ?? 1;
                break;
            case "LegArmor":
                var typeSupportL = account.LegArmor;
                var cateSupportL = account.CateLegArmor;
                account.LegArmor = itemToEquip.IDItem0;
                account.CateLegArmor = itemToEquip.Category;
                itemToEquip.IDItem0 = typeSupportL ?? 1;
                itemToEquip.Category = cateSupportL ?? 1;
                break;
            case "Gloves":
                var typeSupportG = account.Gloves;
                var cateSupportG = account.CateGloves;
                account.Gloves = itemToEquip.IDItem0;
                account.CateGloves = itemToEquip.Category;
                itemToEquip.IDItem0 = typeSupportG ?? 1;
                itemToEquip.Category = cateSupportG ?? 1;
                break;
            case "Shoes":
                var typeSupportS = account.Shoes;
                var cateSupportS = account.CateShoes;
                account.Shoes = itemToEquip.IDItem0;
                account.CateShoes = itemToEquip.Category;
                itemToEquip.IDItem0 = typeSupportS ?? 1;
                itemToEquip.Category = cateSupportS ?? 1;
                break;
            case "Ring": // riêng ring từ từ tính =)))
                // Xác định xem là nhẫn 1 hay nhẫn 2
                if (account.Ring1 == itemToEquip.IDItem0 || account.CateRing1 == itemToEquip.Category)
                {
                    var typeSupportR1 = account.Ring1;
                    var cateSupportR1 = account.CateRing1;
                    account.Ring1 = itemToEquip.IDItem0;
                    account.CateRing1 = itemToEquip.Category;
                    itemToEquip.IDItem0 = typeSupportR1 ?? 1;
                    itemToEquip.Category = cateSupportR1 ?? 1;
                }
                else
                {
                    var typeSupportR2 = account.Ring2;
                    var cateSupportR2 = account.CateRing2;
                    account.Ring2 = itemToEquip.IDItem0;
                    account.CateRing2 = itemToEquip.Category;
                    itemToEquip.IDItem0 = typeSupportR2 ?? 1;
                    itemToEquip.Category = cateSupportR2 ?? 1;
                }
                break;
            case "Necklace":
                var typeSupportN = account.Necklace;
                var cateSupportN = account.CateNecklace;
                account.Necklace = itemToEquip.IDItem0;
                account.CateNecklace = itemToEquip.Category;
                itemToEquip.IDItem0 = typeSupportN ?? 1;
                itemToEquip.Category = cateSupportN ?? 1;
                break;
            case "Medal":
                var typeSupportM = account.Medal;
                var cateSupportM = account.CateMedal;
                account.Medal = itemToEquip.IDItem0;
                account.CateMedal = itemToEquip.Category;
                itemToEquip.IDItem0 = typeSupportM ?? 1;
                itemToEquip.Category = cateSupportM ?? 1;
                break;
            case "":
                Debug.LogWarning("Item không có loại hợp lệ để trang bị.");
                break;
        }
        db.SaveChanges();
        equipmentController.RefreshEquipmentUI();
        ReadDataBase();
    }
}
