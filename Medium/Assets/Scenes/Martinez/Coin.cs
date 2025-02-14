using UnityEngine;



public class Coin : MonoBehaviour
{
    public static int coinCount = 0;


    private void Start()
    {
        Coin.coinCount = Coin.coinCount + 1;
        Debug.Log("Empieza la recoleccion" + Coin.coinCount + "monedas");
    }
    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("player") == true) //condicional moneda solo recogida por el jugador
        {

        }

        Coin.coinCount--; //resta moneda recogida
        Debug.Log("recogida de moneda" + coinCount + "monedas");

        if (coinCount == 0)
        {
            Debug.Log("la recoleccion termino");
        }
    


        Destroy(gameObject); //la moneda desaparece
    }
}
