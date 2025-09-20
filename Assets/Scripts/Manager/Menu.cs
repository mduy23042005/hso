using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Menu : MonoBehaviour, IUpdatable
{
    public void Start()
    {
        this.gameObject.SetActive(false);
    }
    public void ToggleMenu()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void CloseMenu()
    {
        this.gameObject.SetActive(false);
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
