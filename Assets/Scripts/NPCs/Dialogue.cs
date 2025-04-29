using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [Serializable]
    public class  Option
    {
        public string text;
        public int nextDialogueId;
    }

    public int id;
    public int phase;
    public int nextDialogueId;
    public int nextPhaseId;
    [TextArea(2, 5)] public string text;
    public Option[] options;
}
