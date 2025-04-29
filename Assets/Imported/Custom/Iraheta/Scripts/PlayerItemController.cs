using UnityEngine;


[RequireComponent (typeof(PlayerStats))]
public class PlayerItemController : MonoBehaviour
{
    PlayerStats _playerStats;
    NpcController _npcController;
    private GameManager gameManager;
    [HideInInspector]
    public Loot _loot;

    // Modified
    Item _item;
    Collider _other;
    Gui3D gui3D;

    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
        gameManager = GameManager.Instance;

        gui3D = FindObjectOfType<Gui3D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        _item = other.GetComponent<Item>();
        _other = other;

        gui3D.IsOnRange = true;

    }
    public void CollectItem()
    {
        if (gui3D.IsTooltipActive)
        {
            if (_item != null)
            {
                GameManager.Instance.RecolectarItem(_item.tag, _item._cantidad);
                gui3D.IsOnRange = false;
                Destroy(_other.gameObject);  // Destruir el ítem después de recogerlo
            }


            if (_other.CompareTag("NPC"))
            {
                _npcController = _other.GetComponent<NpcController>();

                _npcController.MostrarMision();
            }

            if (_other.CompareTag("EXP"))
            {
                if (_playerStats == null) return;
                ExpController _expController = _other.GetComponent<ExpController>();

                _playerStats.AgregarExperiencia(_expController._exp);
                Destroy(_other.gameObject);  // Destruir el ítem después de recogerlo
            }
        }
        else return;
    }





    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            _npcController = null;
            gui3D.IsOnRange = false;
           
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
