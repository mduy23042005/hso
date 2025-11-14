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
    [SerializeField] private List<SpriteController> spriteController;
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
        ReadDatabase();
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
    private void ReadDatabase()
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
            categoryItem = inventoryItem0[idSlot].Category;
        }

        if (idItem == 0 || idSlot >= inventoryItem0.Count) // Slot trống hoặc ngoài phạm vi
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

    // Kiểm tra Item trước khi Equip
    private bool CheckBeforeEquipItem(int type, int cate)
    {
        bool exists = db.Item0.Any(x => x.IDItem0 == type) && cate >= 1 && cate <= 5;
        if (!exists)
        {
            var itemToEquip = inventoryItem0.FirstOrDefault(i => i.IDItem0 == idItem);

            var deleteItemToEquip = inventoryItem0.Where(x => x.IDItem0 == itemToEquip.IDItem0).FirstOrDefault();
            if (deleteItemToEquip != null)
            {
                db.Account_Item0.Remove(deleteItemToEquip);
            }
            return exists; // false
        }
        return exists; // true
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
                var typeSupportW = account.Weapon.GetValueOrDefault();
                var cateSupportW = account.CateWeapon.GetValueOrDefault();
                account.Weapon = itemToEquip.IDItem0;
                account.CateWeapon = itemToEquip.Category;
                if (CheckBeforeEquipItem(typeSupportW, cateSupportW))
                {
                    itemToEquip.IDItem0 = typeSupportW;
                    itemToEquip.Category = cateSupportW;
                    break;
                }
                break;
            case "Helmet":
                var typeSupportH = account.Helmet.GetValueOrDefault();
                var cateSupportH = account.CateHelmet.GetValueOrDefault();
                account.Helmet = itemToEquip.IDItem0;
                account.CateHelmet = itemToEquip.Category;
                if (CheckBeforeEquipItem(typeSupportH, cateSupportH))
                {
                    itemToEquip.IDItem0 = typeSupportH;
                    itemToEquip.Category = cateSupportH;
                    break;
                }
                break;
            case "Armor":
                var typeSupportA = account.Armor.GetValueOrDefault();
                var cateSupportA = account.CateArmor.GetValueOrDefault();
                account.Armor = itemToEquip.IDItem0;
                account.CateArmor = itemToEquip.Category;
                if (CheckBeforeEquipItem(typeSupportA, cateSupportA))
                {
                    itemToEquip.IDItem0 = typeSupportA;
                    itemToEquip.Category = cateSupportA;
                    break;
                }
                break;
            case "LegArmor":
                var typeSupportL = account.LegArmor.GetValueOrDefault();
                var cateSupportL = account.CateLegArmor.GetValueOrDefault();
                account.LegArmor = itemToEquip.IDItem0;
                account.CateLegArmor = itemToEquip.Category;
                if (CheckBeforeEquipItem(typeSupportL, cateSupportL))
                {
                    itemToEquip.IDItem0 = typeSupportL;
                    itemToEquip.Category = cateSupportL;
                    break;
                }
                break;
            case "Gloves":
                var typeSupportG = account.Gloves.GetValueOrDefault();
                var cateSupportG = account.CateGloves.GetValueOrDefault();
                account.Gloves = itemToEquip.IDItem0;
                account.CateGloves = itemToEquip.Category;
                if (CheckBeforeEquipItem(typeSupportG, cateSupportG))
                {
                    itemToEquip.IDItem0 = typeSupportG;
                    itemToEquip.Category = cateSupportG;
                    break;
                }
                break;
            case "Shoes":
                var typeSupportS = account.Shoes.GetValueOrDefault();
                var cateSupportS = account.CateShoes.GetValueOrDefault();
                account.Shoes = itemToEquip.IDItem0;
                account.CateShoes = itemToEquip.Category;
                if (CheckBeforeEquipItem(typeSupportS, cateSupportS))
                {
                    itemToEquip.IDItem0 = typeSupportS;
                    itemToEquip.Category = cateSupportS;
                    break;
                }
                break;
            case "Ring": // riêng ring từ từ tính =)))
                // Xác định xem là nhẫn 1 hay nhẫn 2
                if (account.Ring1 == itemToEquip.IDItem0 || account.CateRing1 == itemToEquip.Category)
                {
                    var typeSupportR1 = account.Ring1.GetValueOrDefault();
                    var cateSupportR1 = account.CateRing1.GetValueOrDefault();
                    account.Ring1 = itemToEquip.IDItem0;
                    account.CateRing1 = itemToEquip.Category;
                    if (CheckBeforeEquipItem(typeSupportR1, cateSupportR1))
                    {
                        itemToEquip.IDItem0 = typeSupportR1;
                        itemToEquip.Category = cateSupportR1;
                        break;
                    }
                }
                else
                {
                    var typeSupportR2 = account.Ring2.GetValueOrDefault();
                    var cateSupportR2 = account.CateRing2.GetValueOrDefault();
                    account.Ring2 = itemToEquip.IDItem0;
                    account.CateRing2 = itemToEquip.Category;
                    if (CheckBeforeEquipItem(typeSupportR2, cateSupportR2))
                    {
                        itemToEquip.IDItem0 = typeSupportR2;
                        itemToEquip.Category = cateSupportR2;
                        break;
                    }
                }
                break;
            case "Necklace":
                var typeSupportN = account.Necklace.GetValueOrDefault();
                var cateSupportN = account.CateNecklace.GetValueOrDefault();
                account.Necklace = itemToEquip.IDItem0;
                account.CateNecklace = itemToEquip.Category;
                if (CheckBeforeEquipItem(typeSupportN, cateSupportN))
                {
                    itemToEquip.IDItem0 = typeSupportN;
                    itemToEquip.Category = cateSupportN;
                    break;
                }
                break;
            case "Medal":
                var typeSupportM = account.Medal.GetValueOrDefault();
                var cateSupportM = account.CateMedal.GetValueOrDefault();
                account.Medal = itemToEquip.IDItem0;
                account.CateMedal = itemToEquip.Category;
                if (CheckBeforeEquipItem(typeSupportM, cateSupportM))
                {
                    itemToEquip.IDItem0 = typeSupportM;
                    itemToEquip.Category = cateSupportM;
                    break;
                }
                break;
            default:
                Debug.LogWarning("Item không có loại hợp lệ để trang bị.");
                break;
        }
        db.SaveChanges();
        equipmentController.RefreshEquipmentUI();
        switch (account.IDSchool)
        {
            case 1: // Chiến binh
                spriteController[0].RefreshCharacterSprite();
                spriteController[1].RefreshCharacterSprite();
                break;
            case 2: // Sát thủ
                spriteController[2].RefreshCharacterSprite();
                spriteController[3].RefreshCharacterSprite();
                break;
            case 3: // Pháp sư
                spriteController[4].RefreshCharacterSprite();
                spriteController[5].RefreshCharacterSprite();
                break;
                /*
            case 4: // Xạ thủ
                spriteController[6].RefreshCharacterSprite();
                spriteController[7].RefreshCharacterSprite();
                break;
                */
        }
        ReadDatabase();
    }
}
