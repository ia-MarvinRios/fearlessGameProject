using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootFollower : MonoBehaviour
{
    [SerializeField] Transform root;
    [SerializeField] bool applyRotation;

    private void Update()
    {
        if (root != null)
        {
            transform.position = root.position;
            
            if (applyRotation)
                transform.rotation = root.rotation;
        }
    }
}
