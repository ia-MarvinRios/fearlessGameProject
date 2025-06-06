using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Credits : MonoBehaviour
{
    public float scrollSpeed = 5f;
    public float startPosY = -1976f;
    public float endPosY = 1080f;
    public AudioClip clip01;
    public AudioClip clip02;
    AudioSource audioSource;
    RectTransform rectTransform;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rectTransform = GetComponent<RectTransform>();
        if (audioSource != null)
        {
            audioSource.clip = clip01; // Asigna el primer clip de audio
            audioSource.Play(); // Reproduce el clip de audio al iniciar
        }
    }

    private void Start()
    {
        // Establece la posición inicial del texto
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, startPosY, rectTransform.localPosition.z);
    }

    void Update()
    {
        // Mueve el texto hacia arriba a una velocidad constante
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            if (audioSource.clip == clip01)
                audioSource.clip = clip02;
            else
                audioSource.clip = clip01;

            audioSource.Play();
        }

        // Si el texto sale de la pantalla, reinicia su posición
        if (rectTransform.localPosition.y > endPosY)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
