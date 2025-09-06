using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private List<IUpdatable> updatables = new List<IUpdatable>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Register(IUpdatable obj)
    {
        if (!updatables.Contains(obj))
            updatables.Add(obj);
    }

    public void Unregister(IUpdatable obj)
    {
        if (updatables.Contains(obj))
            updatables.Remove(obj);
    }

    private void Update()
    {
        foreach (var obj in updatables)
        {
            obj.OnUpdate();
        }
    }
}
