using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    float initVolume = 1f;
    float minVolume = -40f;

    float fogDensity = 0.011f;
    float maxFogDensity = 0.04f;
    float farClipPlane = 150f;
    float minClipPlane = 30f;

    [SerializeField] PlayerData playerData;

    [Header("Volume")]
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Slider volumeSlider;
    [SerializeField] TMP_Text volumeSliderPercent;

    [Header("Render Distance")]
    [SerializeField] CinemachineVirtualCamera _Camera;
    [SerializeField] Slider fogDensitySlider;
    [SerializeField] TMP_Text fogDensitySliderPercent;

    [Header("FOV")]
    [SerializeField] Slider fovSlider;
    [SerializeField] TMP_Text fovSliderPercent;

    [Header("Presets Settings")]
    [SerializeField] TMP_Dropdown presetsDropdown;
    [SerializeField] Light dirLight;
    [SerializeField] RenderPipelineAsset[] renderPresets;

    private void Awake()
    {
        fogDensity = RenderSettings.fogDensity;
        farClipPlane = _Camera.m_Lens.FarClipPlane;
    }

    private void Start()
    {
        SetUpSettings();
    }

    private void SetUpSettings()
    {
        //Set up presets dropdown
        string preset = playerData.RenderPreset.ToString();
        int index = presetsDropdown.options.FindIndex(option => option.text == preset);
        presetsDropdown.value = index;
        presetsDropdown.RefreshShownValue();

        // Set up volume slider
        volumeSlider.value = 1f;
        _audioMixer.GetFloat("MasterVolume", out initVolume);
        volumeSliderPercent.text = $"{Mathf.Round(volumeSlider.value * 100)}%";
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);


        // Set up fog density slider
        fogDensitySlider.value = 1f;
        fogDensitySliderPercent.text = $"{fogDensitySlider.value * 100}%";
        fogDensitySlider.onValueChanged.AddListener(OnFogDensityChanged);

        // Set up FOV slider
        fovSlider.value = 0f;
        fovSliderPercent.text = $"{Mathf.Round(fovSlider.value * 100)}%";
        fovSlider.onValueChanged.AddListener(OnFOVChanged);
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
    
    private void OnFOVChanged(float value)
    {
        _Camera.m_Lens.FieldOfView = Mathf.Lerp(40f, 100f, value);
        fovSliderPercent.text = $"{Mathf.Round(fovSlider.value * 100)}%";
    }

    public void PresetsControl()
    {
        switch (presetsDropdown.value)
        {
            case 0:
                print("High Preset");
                playerData.RenderPreset = RenderPreset.High;
                dirLight.shadows = LightShadows.Soft;
                GraphicsSettings.renderPipelineAsset = renderPresets[0];
                QualitySettings.renderPipeline = renderPresets[0];
                fogDensitySlider.value = 1f;
                OnFogDensityChanged(1f);
                break;
            case 1:
                print("Medium Preset");
                playerData.RenderPreset = RenderPreset.Medium;
                dirLight.shadows = LightShadows.Hard;
                GraphicsSettings.renderPipelineAsset = renderPresets[1];
                QualitySettings.renderPipeline = renderPresets[1];
                fogDensitySlider.value = 0.5f;
                OnFogDensityChanged(0.5f);
                break;
            case 2:
                print("Low Preset");
                playerData.RenderPreset = RenderPreset.Low;
                dirLight.shadows = LightShadows.None;
                GraphicsSettings.renderPipelineAsset = renderPresets[2];
                QualitySettings.renderPipeline = renderPresets[2];
                fogDensitySlider.value = 0.1f;
                OnFogDensityChanged(0.1f);
                break;
            default:
                break;
        }
    }
}
