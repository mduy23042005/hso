﻿using UnityEditor;
using UnityEngine;

public class Demo : Controller
{
    private Animator animatorChild;
    private GameObject player;
    private Animator uiPickChienBinh;
    private Animator uiPickSatThu;
    private Animator uiPickPhapSu;
    private Animator uiPickXaThu;
    private Demo demo;
    private void Awake()
    {
        animatorChild = GetComponent<Animator>();
        if (GameObject.Find("CharaterSelectionUI"))
        {
            uiPickChienBinh = GameObject.Find("UIPickChienBinh").GetComponent<Animator>();
            uiPickSatThu = GameObject.Find("UIPickSatThu").GetComponent<Animator>();
            uiPickPhapSu = GameObject.Find("UIPickPhapSu").GetComponent<Animator>();
            uiPickXaThu = GameObject.Find("UIPickXaThu").GetComponent<Animator>();
        }
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
        LeftClick();
    }
    public override void OnLateUpdate()
    {
        return;
    }
    public override void OnFixedUpdate()
    {
        return;
    }

    protected override void LeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Lấy vị trí chuột trong thế giới
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    //Tiếp nhận gameobject player bị click
                    player = hit.collider.gameObject;
                    //Tạo biến demo hứng component demo từ player
                    demo = player.GetComponent<Demo>();
                    if (GameObject.Find("CharaterSelectionUI"))
                    {
                        switch (player.name)
                        {
                            case "ChienBinh":
                                uiPickChienBinh.SetBool("isPicked", true);
                                uiPickSatThu.SetBool("isPicked", false);
                                uiPickPhapSu.SetBool("isPicked", false);
                                uiPickXaThu.SetBool("isPicked", false);
                                break;
                            case "SatThu":
                                uiPickChienBinh.SetBool("isPicked", false);
                                uiPickSatThu.SetBool("isPicked", true);
                                uiPickPhapSu.SetBool("isPicked", false);
                                uiPickXaThu.SetBool("isPicked", false);
                                break;
                            case "PhapSu":
                                uiPickChienBinh.SetBool("isPicked", false);
                                uiPickSatThu.SetBool("isPicked", false);
                                uiPickPhapSu.SetBool("isPicked", true);
                                uiPickXaThu.SetBool("isPicked", false);
                                break;
                            case "XaThu":
                                uiPickChienBinh.SetBool("isPicked", false);
                                uiPickSatThu.SetBool("isPicked", false);
                                uiPickPhapSu.SetBool("isPicked", false);
                                uiPickXaThu.SetBool("isPicked", true);
                                break;
                            default:
                                uiPickChienBinh.SetBool("isPicked", false);
                                uiPickSatThu.SetBool("isPicked", false);
                                uiPickPhapSu.SetBool("isPicked", false);
                                uiPickXaThu.SetBool("isPicked", false);
                                break;
                        }
                    }
                    demo.UpdateAnimation();
                    return;
                }
            }
        }
    }
    protected override void UpdateAnimation()
    {
        animatorChild.SetTrigger("Atk");
    }
}
