using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Menu : MonoBehaviour, IUpdatable
{
    private GameObject playerDemo;
    private GameObject chienBinhDemo;
    private GameObject satThuDemo;
    private GameObject phapSuDemo;
    //private GameObject xaThuDemo;

    private bool isActive = false;
    private void Awake()
    {
        playerDemo = GameObject.Find("PlayerDemo").gameObject;
        chienBinhDemo = playerDemo.transform.Find("ChienBinh").gameObject;
        satThuDemo = playerDemo.transform.Find("SatThu").gameObject;
        phapSuDemo = playerDemo.transform.Find("PhapSu").gameObject;
        //xaThuDemo = playerDemo.transform.Find("XaThu").gameObject;
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
                    Destroy(satThuDemo);
                    Destroy(phapSuDemo);
                    //Destroy(xaThuDemo);
                    break;
                case 2:
                    Destroy(chienBinhDemo);
                    satThuDemo.SetActive(true);
                    Destroy(phapSuDemo);
                    //Destroy(xaThuDemo);
                    break;
                case 3:
                    Destroy(chienBinhDemo);
                    Destroy(satThuDemo);
                    phapSuDemo.SetActive(true);
                    //Destroy(xaThuDemo);
                    break;
            }
        }
        gameObject.SetActive(isActive);
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
    public void ToggleMenu()
    {
        isActive = !gameObject.activeSelf;
        gameObject.SetActive(isActive);
    }
    public bool getIsActive()
    {
        return isActive;
    }
    private void FixedUpdate()
    {

    }
}
