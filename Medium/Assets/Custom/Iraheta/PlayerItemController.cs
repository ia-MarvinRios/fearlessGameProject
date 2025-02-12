using UnityEngine;

[RequireComponent (typeof(PlayerStats))]
public class PlayerItemController : MonoBehaviour
{
    PlayerStats _playerStats; 
    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Item _item = other.GetComponent<Item>();
            if(_item != null)
        {
            GameManager.Instance.RecolectarItem(_item.tag, _item._cantidad);
            Destroy(other.gameObject);  // Destruir el ítem después de recogerlo
        }


        if (other.CompareTag("NPC"))
        {
            NpcController _npcController = other.GetComponent<NpcController>();

            _npcController.MostrarMision();
        }

        if (other.CompareTag("EXP"))
        {
            if(_playerStats == null) return;
            ExpController _expController = other.GetComponent<ExpController>();

            _playerStats.AgregarExperiencia(_expController._exp);
            Destroy(other.gameObject);  // Destruir el ítem después de recogerlo
        }

    }
}
