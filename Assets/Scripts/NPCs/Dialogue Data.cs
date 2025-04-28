using UnityEngine;

[CreateAssetMenu]
public class DialogueData : ScriptableObject
{
    [SerializeField] private Dialogue[] dialogues;

    public Dialogue[] Dialogues { get{ return dialogues; } }
}
