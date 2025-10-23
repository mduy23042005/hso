﻿using UnityEngine;
using HSOEntities.Models;

public class SQLConnectionManager : MonoBehaviour, IUpdatable
{
    private static HSOEntities.Models.HSOEntities data;

    void Awake()
    {
        // Đảm bảo chỉ có một instance duy nhất tồn tại
        if (data == null)
        {
            string connection = @"metadata=res://*/HSOEntities.Models.HSOEntities.csdl|
                                  res://*/HSOEntities.Models.HSOEntities.ssdl|
                                  res://*/HSOEntities.Models.HSOEntities.msl;
                                  provider=System.Data.SqlClient;
                                  provider connection string='data source=LAPTOP-AC2MH2TQ,1433;
                                  initial catalog=HSO;
                                  user id=sa;
                                  password=mduy23042005;
                                  trustservercertificate=True;
                                  MultipleActiveResultSets=True;
                                  App=EntityFramework'";

            data = new HSOEntities.Models.HSOEntities(connection);
            Debug.Log("SQL Server connected successfully!");
        }
        else
        {
            Debug.Log("SQLConnectionManager already initialized.");
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
    public void OnFixedUpdate()
    {
    }
    public void OnLateUpdate()
    {
    }
    public void RegisterDontDestroyOnLoad()
    {
        GameManager.Instance.RegisterPersistent(this);
    }

    public static HSOEntities.Models.HSOEntities GetData()
    {
        if (data == null)
        {
            Debug.LogError("Database context not initialized!");
        }
        return data;
    }
}
