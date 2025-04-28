using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [Serializable]
    public class  Option
    {
        public int id;
        public string text;
        public int nextDialogueId;
    }

    public int id;
    public int phase;
    [TextArea(2, 5)] public string text;
    public Option[] options;
}
