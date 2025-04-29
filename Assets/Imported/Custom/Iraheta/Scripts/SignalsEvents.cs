using UnityEngine;
using UnityEngine.Events;

public class SignalsEvents : MonoBehaviour
{
    public UnityEvent[] _EventoSignal;
    
    public void eventoSignal(int index)
    {
        _EventoSignal[index].Invoke();
    }

}
