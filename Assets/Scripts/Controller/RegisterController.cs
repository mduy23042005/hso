using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;

public class RegisterController : MonoBehaviour
{
    [Header("Quản lý Input")]
    [SerializeField] private TMP_InputField inputNameChar;
    [SerializeField] private TMP_InputField inputUsername;
    [SerializeField] private TMP_InputField inputPassword;
    [SerializeField] private TMP_Text inputNameHair;
    [SerializeField] private TMP_Text inputNameBlessing;

    [Header("Quản lý danh sách kiểu tóc")]
    [SerializeField] private List<SpriteLibrary> spriteLibrary;
    private List<HairLibraries> hairLibraries;

    [Header("Xuất thông báo lỗi")]
    [SerializeField] private TMP_Text textMessageNameChar;
    [SerializeField] private TMP_Text textMessageUsername;
    [SerializeField] private TMP_Text textMessagePassword;

    [Header("Thông báo Error School")]
    [SerializeField] private Animator uiPickChienBinh;
    [SerializeField] private Animator uiPickSatThu;
    [SerializeField] private Animator uiPickPhapSu;
    [SerializeField] private Animator uiPickXaThu;

    private int idSchool = 0;
    private string nameSchool;
    private int[] idHair = new int[4];
    private int idBlessing = 0;
    private string[] nameBlessing;
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
        nameBlessing = new string[] { "Ánh sáng", "Bóng tối" };
    }
    #region ChangeHair
    private bool CheckOptionSchool()
    {
        if (idSchool == 0) return false;
        switch (idSchool)
        {
            case 1: nameSchool = "ChienBinh"; break;
            case 2: nameSchool = "SatThu"; break;
            case 3: nameSchool = "PhapSu"; break;
            case 4: nameSchool = "XaThu"; break;
            default: return false;
        }

        hairLibraries = GameObject.Find(nameSchool).GetComponent<SpriteController>().GetHair();
        EquipHair(idHair[idSchool - 1]);
        UpdateNameHair();
        return true;
    }
    public void OnSelectSchool()
    {
        idSchool = DemoController.GetIDSchool();
        if (CheckOptionSchool())
        {
            EquipHair(idHair[idSchool - 1]);
            UpdateNameHair();
        }
    }
    public void NextHair()
    {
        if (idSchool == 0)
        {   
            SendErrorSchool();
            return; 
        }
        idHair[idSchool - 1] = (idHair[idSchool - 1] + 1) % hairLibraries.Count;
        EquipHair(idHair[idSchool - 1]);
        UpdateNameHair();
    }
    public void PrevHair()
    {
        if (idSchool == 0)
        {
            SendErrorSchool();
            return;
        }
        idHair[idSchool - 1] = (idHair[idSchool - 1] - 1 + hairLibraries.Count) % hairLibraries.Count;
        EquipHair(idHair[idSchool - 1]);
        UpdateNameHair();
    }

    private void EquipHair(int hairIndex)
    {
        spriteLibrary[idSchool - 1].spriteLibraryAsset = hairLibraries[hairIndex].hairLibrariesAsset;
    }
    private void UpdateNameHair()
    {
        inputNameHair.text = $"Kiểu: {idHair[idSchool - 1]}";
    }
    #endregion ChangeHair
    #region ChangeBlessing
    public void NextBlessing()
    {
        idBlessing = (idBlessing + 1) % nameBlessing.Length;
        inputNameBlessing.text = $"{nameBlessing[idBlessing]}";
    }
    public void PrevBlessing()
    {
        idBlessing = (idBlessing - 1 + nameBlessing.Length) % nameBlessing.Length;
        inputNameBlessing.text = $"{nameBlessing[idBlessing]}";
    }
    #endregion ChangeBlessing

    public void ClickRegister()
    {
        idSchool = DemoController.GetIDSchool();
        if (idSchool == 0)
        {
            SendErrorSchool();
            return;
        }
        string nameChar = inputNameChar.text.Trim();
        string username = inputUsername.text.Trim();
        string password = inputPassword.text.Trim();
        int weapon = 0;
        int helmet = 0;
        int armor = 0;
        int legArmor = 0;
        int hair = idHair[idSchool - 1];
        int blessingPoints = idBlessing;
        db = SQLConnectionManager.GetData();

        bool usernameExists = db.Accounts.Any(a => a.Username == username);
        if (usernameExists)
        {
            textMessageUsername.color = Color.white;
            textMessageUsername.text = "!";
            return;
        }

        bool nameCharExists = db.Accounts.Any(a => a.NameChar == nameChar);
        if (nameCharExists)
        {
            textMessageNameChar.color = Color.white;
            textMessageNameChar.text = "!";
            return;
        }

        switch (idSchool)
        {
            case 1:
                weapon = 1; helmet = 9; armor = 17; legArmor = 25;
                break;
            case 2:
                weapon = 2; helmet = 10; armor = 18; legArmor = 26;
                break;
            case 3:
                weapon = 3; helmet = 11; armor = 19; legArmor = 27;
                break;
            case 4:
                weapon = 4; helmet = 12; armor = 20; legArmor = 28;
                break;
        }

        if (CheckAllInfo(idSchool, inputNameChar, inputUsername, inputPassword))
        {
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
                Hair = hair,
                BlessingPoints = blessingPoints,
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
                #endregion
            });
            db.SaveChanges();
            Debug.Log("Đăng ký thành công!");
            SceneManager.LoadScene("Main");
        }
    }
    private bool CheckAllInfo(int idSchool, TMP_InputField nameChar, TMP_InputField username, TMP_InputField password)
    {
        bool isValid = true;

        //Kiểm tra trường phái
        if (idSchool == 0)
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
            textMessageNameChar.text = "!";
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
            textMessageNameChar.text = "!";
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