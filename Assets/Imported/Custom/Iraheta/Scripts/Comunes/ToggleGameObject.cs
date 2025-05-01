using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    
    public bool _showHideCursor = false; 
    public void ActivarObjeto()
    {
        if (_showHideCursor) SetCursorState(gameObject.activeSelf);
        
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
    

    // Modified
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}