using UnityEngine;


[RequireComponent (typeof(PlayerStats))]
public class PlayerItemController : MonoBehaviour
{
    PlayerStats _playerStats;
    NpcController _npcController;
    private GameManager gameManager;
    [HideInInspector]
    public Loot _loot;


    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
        gameManager = GameManager.Instance;



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
            _npcController = other.GetComponent<NpcController>();

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





    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            _npcController = null;

           
        }
    }

    public void OnAceptarMision()
    {
        _npcController?.AceptarMision();
    }


    public void Lootear()
    {
        if(_loot != null)
        {
            _loot.give_loot();
        }
    }

}
