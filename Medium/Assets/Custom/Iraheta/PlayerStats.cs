using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Variables del sistema de nivel y experiencia
    public int nivel = 1;
    public int experiencia = 0;
    public int experienciaNecesaria = 100;
    public int puntosDisponibles = 0; // Puntos para mejorar atributos

    // Atributos del jugador
    public int velocidad = 5;
    public int fuerza = 5;
    public int magia = 5;

    // Multiplicador de experiencia necesaria para el próximo nivel
    public float multiplicadorExp = 1.5f;

    // Agregar experiencia al jugador
    public void AgregarExperiencia(int cantidad)
    {
        experiencia += cantidad;
        Debug.Log($"Ganaste {cantidad} de experiencia. Total: {experiencia}/{experienciaNecesaria}");

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
        puntosDisponibles += 3; // 3 puntos por nivel
        experienciaNecesaria = Mathf.RoundToInt(experienciaNecesaria * multiplicadorExp);

        Debug.Log($"¡Subiste a nivel {nivel}! Puntos disponibles: {puntosDisponibles}");
    }

    // Mejorar atributos usando puntos
    public void MejorarAtributo(string atributo)
    {
        if (puntosDisponibles > 0)
        {
            switch (atributo.ToLower())
            {
                case "velocidad":
                    velocidad++;
                    break;
                case "fuerza":
                    fuerza++;
                    break;
                case "magia":
                    magia++;
                    break;
                default:
                    Debug.LogWarning("Atributo no válido.");
                    return;
            }

            puntosDisponibles--;
            Debug.Log($"Atributo {atributo} mejorado. Puntos restantes: {puntosDisponibles}");
        }
        else
        {
            Debug.LogWarning("No tienes puntos suficientes.");
        }
    }
}
