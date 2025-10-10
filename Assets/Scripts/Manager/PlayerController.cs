
using UnityEngine;

public class PlayerController : MonoBehaviour, IUpdatable
{
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
    public void RegisterDontDestroyOnLoad()
    {
        GameManager.Instance.RegisterPersistent(this);
    }
}
