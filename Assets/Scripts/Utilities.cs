using System.Collections;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    [Header("Sun Rotation Settings")]
    [SerializeField] float rotationDegrees = 18f;     // Grados por rotación
    [SerializeField] float duration = 3f;             // Tiempo de transición
    [SerializeField] float targetFogVFinal = 0.25f;   // Valor final deseado del V del fog
    [SerializeField] int totalSteps = 10;             // Número total de iteraciones previstas
    [SerializeField] Gradient sunColorGradient;       // Gradient que representa el color de la luz solar a lo largo del día

    private bool isRotating = false;
    private int currentStep = 0;

    [Header("Audio Transition Settings")]
    //[SerializeField] int totalAudioSteps = 10;
    [SerializeField] float volumeTargetPercent = 0.02f; // 10%

    private bool isTransitioning = false;
    AmbienceSFX ambienceSFX;



    private void Awake()
    {
        ambienceSFX = GetComponent<AmbienceSFX>();
    }


    public void TransitionAmbience()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionAmbienceCoroutine(ambienceSFX));
        }
    }

    private IEnumerator TransitionAmbienceCoroutine(AmbienceSFX ambience)
    {
        isTransitioning = true;

        AudioSource source = ambience.GetAudioSource();
        float originalVolume = source.volume;

        // Calcula target y reducción por step solo una vez
        float fullOriginalVolume = 1f; // Suponemos que comienza desde 1.0
        float targetVolume = fullOriginalVolume * volumeTargetPercent;
        float stepAmount = (fullOriginalVolume - targetVolume) / totalSteps;

        float targetStepVolume = fullOriginalVolume - (currentStep * stepAmount);

        float time = 0f;
        float startVolume = source.volume;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            source.volume = Mathf.Lerp(startVolume, targetStepVolume, t);
            yield return null;
        }

        source.volume = targetStepVolume;

        // Si es el último paso, cambia el clip a noche
        if (currentStep >= totalSteps)
        {
            ambience.SetClipAndPlay(ambience.nightForestAmbience);

            // Restaurar volumen gradualmente (opcional)
            time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                source.volume = Mathf.Lerp(targetStepVolume, fullOriginalVolume, t);
                yield return null;
            }
            source.volume = fullOriginalVolume;
        }

        isTransitioning = false;
    }






    public void RotateSun(GameObject directionalLight)
    {
        if (directionalLight != null && !isRotating && currentStep < totalSteps)
        {
            StartCoroutine(RotateOverTime(directionalLight, rotationDegrees, duration, directionalLight.GetComponent<Light>()));
            TransitionAmbience(); // Iniciar cambio gradual de volumen
            currentStep++;
        }
    }

    private IEnumerator RotateOverTime(GameObject lightObj, float degrees, float duration, Light sunLight)
    {
        isRotating = true;

        // --- ROTACIÓN ---
        Quaternion startRot = lightObj.transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(degrees, 0f, 0f);

        // --- FOG HSV ---
        Color startFogColor = RenderSettings.fogColor;
        Color.RGBToHSV(startFogColor, out float h, out float s, out float v);
        float totalReduction = v - targetFogVFinal;
        float reductionPerStep = totalReduction / (totalSteps - currentStep + 1);
        float targetV = Mathf.Max(targetFogVFinal, v - reductionPerStep);

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Rotación suavizada
            lightObj.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            // Fog V suavizado
            float currentV = Mathf.Lerp(v, targetV, t);
            RenderSettings.fogColor = Color.HSVToRGB(h, s, currentV);

            // Luz solar con gradient
            float gradientT = (currentStep - 1 + t) / totalSteps;
            sunLight.color = sunColorGradient.Evaluate(gradientT);

            yield return null;
        }

        // Asignar valores finales exactos
        lightObj.transform.rotation = targetRot;
        RenderSettings.fogColor = Color.HSVToRGB(h, s, targetV);
        sunLight.color = sunColorGradient.Evaluate(currentStep / (float)totalSteps);

        isRotating = false;
    }
}
