using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph;
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

    private Dictionary<int, Sprite> itemDefaultMap;
    private Dictionary<int, Sprite> item0Map;
    private Dictionary<int, Sprite> item1Map;
    private Dictionary<int, Sprite> item2Map;
    private Dictionary<int, Sprite> item3Map;
    private Dictionary<int, Sprite> item4Map;
    private Dictionary<int, Image> inventorySlotsMap;

    HSOEntities.Models.HSOEntities db;

    private void Awake()
    {
        itemDefaultMap = ConvertListToMap(itemDefault);
        item0Map = ConvertListToMap(item0);
        item1Map = ConvertListToMap(item1);
        item2Map = ConvertListToMap(item2);
        item3Map = ConvertListToMap(item3);
        item4Map = ConvertListToMap(item4);

        inventorySlotsMap = ConvertListToMap(inventorySlots);

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
    private Dictionary<int, Image> ConvertListToMap(List<Image> list)
    {
        var map = new Dictionary<int, Image>();
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

    private void ReadDataBase()
    {
        int idAccount = LogInController.GetIDAccount();
        var account = db.Accounts.FirstOrDefault(acc => acc.IDAccount == idAccount);

        if (account != null)
        {
            var inventoryItem0 = db.Account_Item0.Where(item => item.IDAccount == idAccount).ToList();
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
}
