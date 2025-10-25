using UnityEngine;

public class PlayerController : MonoBehaviour, IUpdatable
{
    private GameObject player;
    private GameObject chienBinh;
    private GameObject satThu;
    private GameObject phapSu;
    //private GameObject XaThu;

    private void Awake()
    {
        if (GameObject.Find("Player"))
        {
            player = GameObject.Find("Player").gameObject;
            chienBinh = player.transform.Find("ChienBinh").gameObject;
            satThu = player.transform.Find("SatThu").gameObject;
            phapSu = player.transform.Find("PhapSu").gameObject;
            //xaThu = player.transform.Find("XaThu").gameObject;
        }
    }
    private void OnEnable()
    {
        GameManager.Instance.Register(this);
        RegisterDontDestroyOnLoad();
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Unregister(this);
        }
    }
    public void OnUpdate()
    {
    }
    public void OnLateUpdate()
    {
    }
    public void OnFixedUpdate()
    {
    }
    public void RegisterDontDestroyOnLoad()
    {
        GameManager.Instance.RegisterPersistent(this);
    }

    private void Start()
    {
        if (GameObject.Find("Player"))
        {
            var school = LogIn.GetSchool();
            switch (school)
            {
                case "ChienBinh":
                    chienBinh.SetActive(true);
                    satThu.SetActive(false);
                    phapSu.SetActive(false);
                    //xaThu.SetActive(false);
                    break;
                case "SatThu":
                    chienBinh.SetActive(false);
                    satThu.SetActive(true);
                    phapSu.SetActive(false);
                    //xaThu.SetActive(false);
                    break;
                case "PhapSu":
                    chienBinh.SetActive(false);
                    satThu.SetActive(false);
                    phapSu.SetActive(true);
                    //xaThu.SetActive(false);
                    break;
            }
        }
    }
}
