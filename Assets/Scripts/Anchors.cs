using UnityEngine;

public class Anchors : MonoBehaviour
{
    public Transform parent;
    public float xOffset, yOffset, zOffset;

    [Header("Advanced")]
    [SerializeField] private bool ApplyRotation;
    [SerializeField] private float RotationModifier;

    void Update()
    {
        transform.position = new Vector3(parent.position.x + xOffset, parent.position.y + yOffset, parent.position.z + zOffset); // Mantiene la posición

        if (ApplyRotation)
        {
            transform.rotation = parent.rotation * Quaternion.Euler(RotationModifier, RotationModifier, RotationModifier);
        }
    }
}
