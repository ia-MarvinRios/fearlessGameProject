using UnityEngine;
using System.Diagnostics;

public class DestroyDebugger : MonoBehaviour
{
    private void OnDestroy()
    {
        StackTrace stackTrace = new StackTrace();
        UnityEngine.Debug.Log($"[DestroyDebugger] {gameObject.name} fue destruido.\nStackTrace:\n{stackTrace}");
    }
}