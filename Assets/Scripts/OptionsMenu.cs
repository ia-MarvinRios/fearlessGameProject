using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    float initVolume = 1f;
    float minVolume = -40f;

    float fogDensity = 0.011f;
    float maxFogDensity = 0.04f;
    float farClipPlane = 150f;
    float minClipPlane = 30f;

    [Header("Volume")]
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Slider volumeSlider;
    [SerializeField] TMP_Text volumeSliderPercent;

    [Header("Render Distance")]
    [SerializeField] CinemachineVirtualCamera _Camera;
    [SerializeField] Slider fogDensitySlider;
    [SerializeField] TMP_Text fogDensitySliderPercent;

    private void Awake()
    {
        fogDensity = RenderSettings.fogDensity;
        farClipPlane = _Camera.m_Lens.FarClipPlane;
    }

    private void Start()
    {
        SetUpSettings();
    }

    private void OnFogDensityChanged(float value)
    {
        RenderSettings.fogDensity = Mathf.Lerp(maxFogDensity, fogDensity, value);
        _Camera.m_Lens.FarClipPlane = Mathf.Lerp(minClipPlane, farClipPlane, value);
        fogDensitySliderPercent.text = $"{Mathf.Round(fogDensitySlider.value * 100)}%";
    }

    private void OnVolumeChanged(float value)
    {
        _audioMixer.SetFloat("MasterVolume", Mathf.Lerp(minVolume, initVolume, value));
        volumeSliderPercent.text = $"{Mathf.Round(volumeSlider.value * 100)}%";
    }

    private void SetUpSettings()
    {
        // Set up volume slider
        volumeSlider.value = 1f;
        _audioMixer.GetFloat("MasterVolume", out initVolume);
        volumeSliderPercent.text = $"{Mathf.Round(volumeSlider.value * 100)}%";
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);


        // Set up fog density slider
        fogDensitySlider.value = 1f;
        fogDensitySliderPercent.text = $"{fogDensitySlider.value * 100}%";
        fogDensitySlider.onValueChanged.AddListener(OnFogDensityChanged);
    }
}
