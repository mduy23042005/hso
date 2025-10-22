using HSOEntities.Models;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void ReadDatabase()
    {
        string connection = @"metadata=res://*/HSOEntities.Models.HSOEntities.csdl|res://*/HSOEntities.Models.HSOEntities.ssdl|res://*/HSOEntities.Models.HSOEntities.msl;provider=System.Data.SqlClient;provider connection string='data source=LAPTOP-AC2MH2TQ,1433;initial catalog=HSO;user id=sa;password=mduy23042005;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework'";
        Debug.Log("Connection string: " + connection);

        using (var db = new HSOEntities.Models.HSOEntities(connection)) //HSOEntities là nơi chứa folder Models, folder Models chứa database tên HSOEntities.edmx
        {
            var accounts = db.Accounts.ToList();
            foreach (var acc in accounts)
            {
                Debug.Log($"ID: {acc.IDAccount}, Username: {acc.Username}, Password: {acc.Password}");
            }
        }
    }
}