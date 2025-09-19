using System.Collections.Generic;
using UnityEngine;

public interface IUpdatable
{
    void OnUpdate();
}
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    Debug.LogError("No GameManager found in scene!");
                }
            }
            return instance;
        }
    }

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
        if (obj != null && !updatables.Contains(obj))
            updatables.Add(obj);
    }

    public void Unregister(IUpdatable obj)
    {
        if (obj != null && updatables.Contains(obj))
            updatables.Remove(obj);
    }

    private void Update()
    {
        // copy ra list tạm để tránh lỗi khi remove trong foreach
        for (int i = updatables.Count - 1; i >= 0; i--)
        {
            var updatable = updatables[i];
            // check null hoặc bị destroy
            if (updatable == null || (updatable is Object unityObj && unityObj == null))
            {
                updatables.RemoveAt(i);
                continue;
            }
            updatable.OnUpdate();
        }
    }
}
