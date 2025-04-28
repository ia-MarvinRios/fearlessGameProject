using UnityEngine;
using StarterAssets;
using UnityEngine.Events;
public class PlayerStats : MonoBehaviour
{

    //StarterAssetsInputs starterAssetsInputs;


    // Variables del sistema de nivel y experiencia
    public int nivel = 1;
    public int experiencia = 0;
    public int experienciaNecesaria = 1;
    public int _puntosDisponibles = 0; // Puntos para mejorar atributos

    // Atributos del jugador
    public int _velocidad = 1;
    public int _fuerza = 5;
    public int _magia = 5;
    public float _vida = 30;
    public float _vidaActual; 

    // Multiplicador de experiencia necesaria para el próximo nivel
    public float multiplicadorExp = 1.5f;
    [Space]
    public string _msgPuntos; 
    public string _msgVida;
    public string _msgFuerza;
    public string _msgMagia;
    public string _msgVelocidad;



    //bool _inventarioAbierto = false;

    //ThirdPersonController thirdPersonController;

    public UnityEvent _obtenerExperiencia;
    public UnityEvent _subirNivel;
    public UnityEvent _mejorarAtributo;
    public UnityEvent _abrirInventario;
    public UnityEvent _cerrarInventario;

    private GameManager gameManager;
    public Transform _spawn;

    private void Start()
    {
        //thirdPersonController = GetComponent<ThirdPersonController>();
        gameManager = GameManager.Instance;

        //starterAssetsInputs = GetComponent<StarterAssetsInputs>();

        ActualizarUI();
        setMaxHealth(_vida);
        setMagic(_magia);

        //  _vidaActual = _vida;

        if (_spawn == null) _spawn = transform;

       
    }



    public void Spawn()
    {
        Debug.Log("Moviendo");
        transform.position = _spawn.position;
    }

    string Reemplazar(string _parametro, string _nuevaPalabra, string _cadena)
    {
        return _cadena.Replace(_parametro, _nuevaPalabra);
    }

    void ActualizarUI()
    {
        if (gameManager.uIController._txtVida) gameManager.uIController._txtVida.text = Reemplazar("{vida}", _vida.ToString(), _msgVida);
        if (gameManager.uIController._txtFuerza) gameManager.uIController._txtFuerza.text = Reemplazar("{fuerza}", _fuerza.ToString(), _msgFuerza);
        if (gameManager.uIController._txtMagia) gameManager.uIController._txtMagia.text = Reemplazar("{magia}", _magia.ToString(), _msgMagia);
        if (gameManager.uIController._txtVelocidad) gameManager.uIController._txtVelocidad.text = Reemplazar("{velocidad}", _velocidad.ToString(), _msgVelocidad);
        if (gameManager.uIController._txtPuntos) gameManager.uIController._txtPuntos.text = Reemplazar("{puntos}", _puntosDisponibles.ToString(), _msgPuntos);
    }

    // Agregar experiencia al jugador
    public void AgregarExperiencia(int cantidad)
    {
        experiencia += cantidad;
        Debug.Log($"Ganaste {cantidad} de experiencia. Total: {experiencia}/{experienciaNecesaria}");

        if(experiencia >= experienciaNecesaria)
        {
            _subirNivel.Invoke();
        }
        else
        {
            _obtenerExperiencia.Invoke();
        }

        // Verificar si sube de nivel
        while (experiencia >= experienciaNecesaria)
        {
            SubirNivel();
        } 
    }

    // Método para subir de nivel
    private void SubirNivel()
    {
        experiencia -= experienciaNecesaria;
        nivel++;
        _puntosDisponibles += 3; // 3 puntos por nivel
        experienciaNecesaria = Mathf.RoundToInt(experienciaNecesaria * multiplicadorExp);
        
        Debug.Log($"¡Subiste a nivel {nivel}! Puntos disponibles: {_puntosDisponibles}");
    }

    // Mejorar atributos usando puntos
    public void MejorarAtributo(string atributo)
    {
        if(_mejorarAtributo == null)
        {
            _mejorarAtributo.Invoke();
        }

        if (_puntosDisponibles > 0)
        {
            switch (atributo.ToLower())
            {
                case "velocidad":
                    _velocidad++;
                    //thirdPersonController.MoveSpeed = _velocidad;
                    break;
                case "fuerza":
                    _fuerza++;
                    break;
                case "magia":
                    _magia++;
                    break;
                case "vida":
                    _vida++;
                    break;
                default:
                    Debug.LogWarning("Atributo no válido.");
                    return;
            }

            _puntosDisponibles--;
            Debug.Log($"Atributo {atributo} mejorado. Puntos restantes: {_puntosDisponibles}");
            ActualizarUI();
        }
        else
        {
            Debug.LogWarning("No tienes puntos suficientes.");
        }
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            MejorarAtributo("velocidad");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            take_dmg(5);
        }


    }


    public void take_dmg(float _dmg)
    {

        _vidaActual -= _dmg;
        _vidaActual = Mathf.Clamp(_vidaActual, 0, _vida); // Asegura que no baje de 0

        Debug.LogWarning("Recibiendo daño: " + _dmg + " Vida: " + _vida);

        

        if (_vidaActual <= 0)
        {
            Spawn();
            _vidaActual = _vida;
            ;
            
            
        }


        setHealth(_vidaActual);

    }

    /*
    public void OnAbrirInventario()
    {

        if (!_inventarioAbierto)
        {
            _inventarioAbierto = true;
            starterAssetsInputs.cursorLocked = false;
            SetCursorState(starterAssetsInputs.cursorLocked);
            if (_abrirInventario!=null) _abrirInventario.Invoke();
        }
        else
        {
            _inventarioAbierto = false;
            starterAssetsInputs.cursorLocked = true;
            SetCursorState(starterAssetsInputs.cursorLocked);
            if (_cerrarInventario!=null) _cerrarInventario.Invoke();
        }

    }
    */

    public void setMaxHealth(float _health)
    {

        if (gameManager.uIController._HealthBar != null) gameManager.uIController._HealthBar.maxValue = _health;
        setHealth(_health);


    }

    public void setHealth(float _health)
    {
        if (gameManager.uIController._HealthBar != null) gameManager.uIController._HealthBar.value = _health;
    }



    public void setMaxMagic(float _magic)
    {

        if (gameManager.uIController._MagicBar != null) gameManager.uIController._MagicBar.maxValue = _magic;
        setMagic(_magic);


    }

    public void setMagic(float _magic)
    {
        if (gameManager.uIController._MagicBar != null) gameManager.uIController._MagicBar.value = _magic;
    }




    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SPAWN")
        {
            _spawn = other.transform;
        }
    }




}
