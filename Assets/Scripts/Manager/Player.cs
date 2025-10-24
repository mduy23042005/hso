using HSOEntities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    private HSOEntities.Models.HSOEntities db;
    /*
     khi muốn update database trong thread phụ thì phải tạo connection mới và update trong vùng async để nó depose connection sau khi update xong
     */
    async void ReadDatabase()
    {
        List<Account> accounts = null;
        db = SQLConnectionManager.GetData();
        await Task.Run(() =>
        {
            accounts = db.Accounts.AsNoTracking().ToList();
        });

        foreach (var acc in accounts)
        {
            Debug.Log($"ID: {acc.IDAccount}, Username: {acc.Username}, Password: {acc.Password}");
        }
    }
}