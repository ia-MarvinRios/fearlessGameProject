using UnityEngine;

public class TooltipsBehaviour : MonoBehaviour
{
    [SerializeField] GameObject tooltipPrefab;
    GameObject instance = null;

    private void Update()
    {
        if (instance != null)
        {
            Quaternion rot = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

            instance.transform.position = transform.position;
            instance.transform.rotation = rot;
        }
    }

    public GameObject Instantiate()
    {
        if (instance == null)
        {
            instance = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);
            return instance;
        }
        return null;
    }
    public void Destroy()
    {
        if (instance != null)
        {
            Destroy(instance);
            instance = null;
        }
    }
}
