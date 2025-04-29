using UnityEngine;
using UnityEngine.Events;

public class Loot : MonoBehaviour
{
    public UnityEvent OnFinished;

    PlayerItemController playerItemController; 



   public void give_loot()
    {
        OnFinished?.Invoke();
        Destroy(gameObject.GetComponent<Loot>());
        playerItemController._loot = null;
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerItemController = other.GetComponent<PlayerItemController>();
            playerItemController._loot = GetComponent<Loot>();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            playerItemController._loot = null;

        }
    }


}
