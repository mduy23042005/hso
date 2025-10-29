using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    [Header("Quản lý Input")]
    [SerializeField] private TMP_InputField inputNameChar;
    [SerializeField] private TMP_InputField inputUsername;
    [SerializeField] private TMP_InputField inputPassword;
    [Header("Xuất thông báo lỗi")]
    [SerializeField] private TMP_Text textMessageNameChar;
    [SerializeField] private TMP_Text textMessageUsername;
    [SerializeField] private TMP_Text textMessagePassword;
    [Header("Thông báo Error School")]
    [SerializeField] private Animator uiPickChienBinh;
    [SerializeField] private Animator uiPickSatThu;
    [SerializeField] private Animator uiPickPhapSu;
    [SerializeField] private Animator uiPickXaThu;

    private int idSchool;
    HSOEntities.Models.HSOEntities db;

    private void Awake()
    {
        if (GameObject.Find("CharaterSelectionUI"))
        {
            uiPickChienBinh = GameObject.Find("UIPickChienBinh").GetComponent<Animator>();
            uiPickSatThu = GameObject.Find("UIPickSatThu").GetComponent<Animator>();
            uiPickPhapSu = GameObject.Find("UIPickPhapSu").GetComponent<Animator>();
            uiPickXaThu = GameObject.Find("UIPickXaThu").GetComponent<Animator>();
        }
    }

    public void ClickRegister()
    {
        idSchool = Demo.GetIDSchool();
        string nameChar = inputNameChar.text;
        string username = inputUsername.text;
        string password = inputPassword.text;
        int weapon = 0;
        int helmet = 0;
        int armor = 0;
        int legArmor = 0;

        switch (idSchool)
        {
            case 1:
                weapon = 1;
                helmet = 9;
                armor = 17;
                legArmor = 25;
                break;
            case 2:
                weapon = 2;
                helmet = 10;
                armor = 18;
                legArmor = 26;
                break;
            case 3:
                weapon = 3;
                helmet = 11;
                armor = 19;
                legArmor = 27;
                break;
            case 4:
                weapon = 4;
                helmet = 12;
                armor = 20;
                legArmor = 28;
                break;
        }

        if (CheckInfo(idSchool, inputNameChar, inputUsername, inputPassword))
        {
            db = SQLConnectionManager.GetData();
            db.Accounts.Add(new HSOEntities.Models.Account
            {
                NameChar = nameChar,
                Username = username,
                Password = password,
                IDSchool = idSchool,
                Weapon = weapon,
                Helmet = helmet,
                Armor = armor,
                LegArmor = legArmor,
                #region DefaultValueForColumn
                Level = 1,
                SkillPoints = 0,
                StatPoints = 0,
                Exp = 0,
                Gloves = 0,
                Shoes = 0,
                Ring1 = 0,
                Ring2 = 0,
                Necklace = 0,
                Medal = 0,
                Cloak = 0,
                Wing = 0,
                SkinWing = 0,
                Mounts = 0,
                Pet = 0,
                Skin = 0,
                Gold = 20000,
                Gem = 2000,
                Point0 = 0,
                Point1 = 0,
                Point2 = 0,
                Point3 = 0,
                PointArena = 0,
                PointActive = 100000,
                Skill0 = 1,
                Skill1 = 0,
                Skill2 = 0,
                Skill3 = 0,
                Skill4 = 0,
                Skill5 = 0,
                Skill6 = 0,
                Skill7 = 0,
                Skill8 = 0,
                Skill9 = 0,
                Skill10 = 0,
                Skill11 = 0,
                Skill12 = 0,
                Skill13 = 0,
                Skill14 = 0,
                Skill15 = 0,
                Skill16 = 0,
                Skill17 = 0,
                Skill18 = 0,
                Skill19 = 0,
                Skill20 = 0,
                BlessingPoints = 0
                #endregion
            });
            db.SaveChanges();
            Debug.Log("Đăng ký thành công!");
            SceneManager.LoadScene("LogIn");
        }
    }
    private bool CheckInfo(int idSchool, TMP_InputField nameChar, TMP_InputField username, TMP_InputField password)
    {
        bool isValid = true;

        //Kiểm tra trường phái
        if (idSchool == null || idSchool == 0)
        {
            SendErrorSchool();
            isValid = false;
        }

        //Kiểm tra tên nhân vật
        if (string.IsNullOrEmpty(nameChar.text))
        {
            textMessageNameChar.color = Color.red;
            textMessageNameChar.text = "!";
            isValid = false;
        }
        else
        {
            textMessageNameChar.text = "";
        }
        //Kiểm tra tên đăng nhập
        if (string.IsNullOrEmpty(username.text))
        {
            textMessageUsername.color = Color.red;
            textMessageUsername.text = "!";
            isValid = false;
        }
        else
        {
            textMessageUsername.text = "";
        }
        //Kiểm tra mật khẩu
        if (string.IsNullOrEmpty(password.text))
        {
            textMessagePassword.color = Color.red;
            textMessagePassword.text = "!";
            isValid = false;
        }
        else
        {
            textMessagePassword.text = "";
        }
        return isValid;
    }
    private void SendErrorSchool()
    {
        uiPickChienBinh.SetTrigger("Error");
        uiPickSatThu.SetTrigger("Error");
        uiPickPhapSu.SetTrigger("Error");
        uiPickXaThu.SetTrigger("Error");
    }
}
