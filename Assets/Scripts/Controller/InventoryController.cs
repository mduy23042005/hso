using HSOEntities.Models;
using System.Collections.Generic;
using System.Linq;
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

    private Dictionary<int, Sprite> itemDefaultMap;
    private Dictionary<int, Sprite> item0Map;
    private Dictionary<int, Sprite> item1Map;
    private Dictionary<int, Sprite> item2Map;
    private Dictionary<int, Sprite> item3Map;
    private Dictionary<int, Sprite> item4Map;

    HSOEntities.Models.HSOEntities db;
    List<Account_Item0> inventoryItem0;

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

        int[] equippedItems = equipmentController.GetIDEquipmentSlots();

        int idItem = equippedItems[idSlot];
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

        foreach (var r in result)
        {
            Debug.Log(r);
        }
    }
    // Đọc attribute của item trong inventory
    public void ReadAttributeInInventory(int idSlot)
    {
        int idAccount = LogInController.GetIDAccount();
        var account = db.Accounts.FirstOrDefault(acc => acc.IDAccount == idAccount);

        int idItem = 0; // id được khởi tạo = 0
        int categoryItem = 0;

        if (idSlot >= 0 && idSlot < inventoryItem0.Count) // nếu id hợp lệ thì lấy idItem từ inventory
        {
            idItem = inventoryItem0[idSlot].IDItem0;
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
        foreach (var r in result)
        {
            Debug.Log(r);
        }
    }
}
