using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogInController : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputUsername;
    [SerializeField] private TMP_InputField inputPassword;
    [SerializeField] private TMP_Text textMessage;
    private HSOEntities.Models.HSOEntities db;
    private static int idSchool;
    private static int idAccount;

    public void ClickLogIn()
    {
        string username = inputUsername.text;
        string password = inputPassword.text;

        db = SQLConnectionManager.GetData();
        var account = db.Accounts.ToList();
        if (account.Any(acc => acc.Username == username && acc.Password == password))
        {
            var loggedInAccount = account.FirstOrDefault(acc => acc.Username == username && acc.Password == password);
            textMessage.color = Color.green;
            textMessage.text = $"Đăng nhập {loggedInAccount.NameChar} thành công!";
            idSchool = loggedInAccount.IDSchool ?? 0;
            idAccount = loggedInAccount.IDAccount;
            SceneManager.LoadScene("Map1");
        }
        else
        {
            textMessage.color = Color.red;
            textMessage.text = "Username hoặc Password không đúng.";
        }
    }
    public void ClickRegister()
    {
        SceneManager.LoadScene("SelectCharacterScene");
    }
    public static int GetIDSchool()
    {
        return idSchool;
    }
    public static int GetIDAccount()
    {
        return idAccount;
    }
}
