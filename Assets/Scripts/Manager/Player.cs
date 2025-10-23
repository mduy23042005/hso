using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void ReadDatabase()
    {
        var db = SQLConnectionManager.GetData();
        var accounts = db.Accounts.Where(acc => acc.IDAccount == 1).ToList();
        foreach (var acc in accounts)
        {
            Debug.Log($"ID: {acc.IDAccount}, Username: {acc.Username}, Password: {acc.Password}");
        }
    }
}