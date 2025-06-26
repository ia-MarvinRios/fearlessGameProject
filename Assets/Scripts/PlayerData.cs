using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum RenderPreset { Low, Medium, High }

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Graphic Settings")]
    [SerializeField] private RenderPreset renderPreset = RenderPreset.High;

    [Header("Gameplay Settings")]
    public bool doSave = false;
    public Vector3 respawnPos = Vector3.zero;
    [Tooltip("The current active mission.")]
    [SerializeField, Range(1, 6)] int currentMission = 1;
    [Tooltip("Is torch available for the player to use?")]
    [SerializeField] bool torchUnlocked = false;

    public RenderPreset RenderPreset { get => renderPreset; set { renderPreset = value; } }
    public int CurrentMission { get => currentMission; set { currentMission = value; } }
    public bool TorchUnlocked { get => torchUnlocked; set { torchUnlocked = value; } }
}
