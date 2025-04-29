using UnityEngine;
using UnityEngine.Events;

public class Interaccion : MonoBehaviour
{

    public UnityEvent _start;
    public UnityEvent _Interactuar;
    public UnityEvent _DejarDeInteractuar;

    


    private void Start()
    {
        _start.Invoke();
    }



    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            _Interactuar.Invoke();
        }

    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _DejarDeInteractuar.Invoke();
        }
    }



}
