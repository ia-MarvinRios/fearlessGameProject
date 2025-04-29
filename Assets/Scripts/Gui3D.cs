using UnityEngine;

[RequireComponent(typeof(ScreenRaycast))]
public class Gui3D : MonoBehaviour
{
    TooltipsBehaviour tooltip = null;
    ScreenRaycast screenRaycast;

    bool isPointing = false;
    bool isOnRange = false;

    public bool IsPointing { get { return isPointing; } set { isPointing = value; } }
    public bool IsOnRange { get { return isOnRange; } set { isOnRange = value; } }

    private void Start()
    {
        screenRaycast = GetComponent<ScreenRaycast>();
    }

    private void Update()
    {
        if (isPointing && isOnRange)
        {
            ShowToolTip();
        }
        else
        {
            HideToolTips();
        }
    }

    public void ShowToolTip()
    {
        tooltip = screenRaycast.hit.collider != null ? screenRaycast.hit.transform.gameObject.GetComponentInChildren<TooltipsBehaviour>() : null;
        if (tooltip != null)
        {
            tooltip.Instantiate();
        }
    }

    public void HideToolTips()
    {
        if (tooltip != null)
        {
            tooltip.Destroy();
            tooltip = null;
        }
    }
}
