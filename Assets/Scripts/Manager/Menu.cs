using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Menu : MonoBehaviour, IUpdatable
{
    private bool isActive = false;

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
