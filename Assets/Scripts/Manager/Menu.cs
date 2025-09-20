using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Menu : MonoBehaviour, IUpdatable
{
    private bool isActive = false;
    public void Start()
    {
        this.gameObject.SetActive(false);
    }
    public void ToggleMenu()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
        if (this.gameObject.activeSelf == true)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
    }
    public bool getIsActive()
    {
        return isActive;
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
    private void FixedUpdate()
    {

    }
}
