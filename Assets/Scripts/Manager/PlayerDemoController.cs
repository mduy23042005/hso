using UnityEditor;
using UnityEngine;

public class PlayerDemoController : Controller
{
    private GameObject playerDemo;
    private GameObject chienBinhDemo;
    private GameObject satThuDemo;
    private GameObject phapSuDemo;
    //private GameObject xaThuDemo;

    private void Awake()
    {
        playerDemo = GameObject.Find("PlayerDemo").gameObject;
        chienBinhDemo = playerDemo.transform.Find("ChienBinh").gameObject;
        satThuDemo = playerDemo.transform.Find("SatThu").gameObject;
        phapSuDemo = playerDemo.transform.Find("PhapSu").gameObject;
        //xaThuDemo = playerDemo.transform.Find("XaThu").gameObject;
    }
    private void OnEnable()
    {
        GameManager.Instance.Register(this);
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Unregister(this);
        }
    }
    protected override void MoveKeyboard()
    {
        return;
    }
    protected override void MoveMouse()
    {
        return;
    }

    public override void OnUpdate()
    {
    }
    public override void OnLateUpdate()
    {
        return;
    }
    public override void OnFixedUpdate()
    {
        return;
    }

    private void Start()
    {
        if (GameObject.Find("Player"))
        {
            var idSchool = LogIn.GetIDSchool();
            switch (idSchool)
            {
                case 1:
                    chienBinhDemo.SetActive(true);
                    satThuDemo.SetActive(false);
                    phapSuDemo.SetActive(false);
                    //xaThuDemo.SetActive(false);
                    break;
                case 2:
                    chienBinhDemo.SetActive(false);
                    satThuDemo.SetActive(true);
                    phapSuDemo.SetActive(false);
                    //xaThuDemo.SetActive(false);
                    break;
                case 3:
                    chienBinhDemo.SetActive(false);
                    satThuDemo.SetActive(false);
                    phapSuDemo.SetActive(true);
                    //xaThuDemo.SetActive(false);
                    break;
            }
        }
    }
}
