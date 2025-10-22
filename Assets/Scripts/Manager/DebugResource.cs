using System.Reflection;
using UnityEngine;

public class DebugResource : MonoBehaviour
{
    void Start()
    {
        var asm = Assembly.GetExecutingAssembly();
        foreach (var res in asm.GetManifestResourceNames())
        {
            Debug.Log("Resource found: " + res);
        }
    }
}
