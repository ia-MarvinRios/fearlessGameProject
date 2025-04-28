using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("GUI Manager Reference")]
    [SerializeField] private GuiManager guiMan;
    [SerializeField] TMP_Text textArea;

    [Header("Dialogues")]
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private int currentPhase;  
    private int currentId = 0;
    public bool didDialogueStart = false;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private float typingSpeed = 0.05f;

    public void StartDialogue()
    {
        if (!didDialogueStart)
        {
            DestroyAllChildren(guiMan.buttonsLayout);
            didDialogueStart = true;
            guiMan.lowerPanel.SetActive(true);
            textArea.gameObject.SetActive(true);
            guiMan.buttonsLayout.gameObject.SetActive(false);

            List<Dialogue> currentPhaseDialogues = new List<Dialogue>();

            foreach (Dialogue dialogue in dialogueData.Dialogues)
            {
                if (dialogue.phase == currentPhase)
                {
                    currentPhaseDialogues.Add(dialogue);
                }
            }

            StartCoroutine(DialogueCoroutine(currentPhaseDialogues));
        }
        
    }


    IEnumerator DialogueCoroutine(List<Dialogue> dialogues)
    {
        for (int i = 0; i < dialogues.Count; i++)
        {
            if (currentId != dialogues[i].id)
            {
                currentId = dialogues[i].id;
                typingCoroutine = StartCoroutine(TypeText(dialogues[i].text));

                yield return new WaitUntil(() => !isTyping);

                yield return new WaitUntil(() => guiMan.interInput);
                yield return null;

                if (dialogues[i].options.Length > 0)
                {
                    yield return new WaitUntil(() => ShowDialogueOptions(dialogues[i].options));
                    yield return null;
                    Cursor.lockState = CursorLockMode.None;
                    guiMan.inputScript.enabled = false;
                }
            }
        }

        EndDialogue();
    }

    private bool ShowDialogueOptions(Dialogue.Option[] options)
    {
        textArea.gameObject.SetActive(false);
        textArea.text = "";
        guiMan.buttonsLayout.gameObject.SetActive(true);

        foreach (Dialogue.Option option in options)
        {
            GameObject newOption = Instantiate(guiMan.optionButton, guiMan.buttonsLayout);
            Button btn = newOption.GetComponent<Button>();
            TMP_Text btnText = newOption.GetComponentInChildren<TMP_Text>();
            btnText.text = option.text;

            btn.onClick.AddListener(() =>
            {
                currentId = option.nextDialogueId;
                textArea.gameObject.SetActive(true);
                guiMan.buttonsLayout.gameObject.SetActive(false);
            });
        }

        return true;
    }

    private void EndDialogue()
    {
        didDialogueStart = false;
        guiMan.lowerPanel.SetActive(false);
        textArea.gameObject.SetActive(false);
        guiMan.buttonsLayout.gameObject.SetActive(false);
        guiMan.inputScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        StopAllCoroutines();
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        textArea.text = "";

        foreach (char letter in text)
        {
            textArea.text += letter;

            // Aquí hacemos una pausa normal... PERO
            float timer = 0;
            while (timer < typingSpeed)
            {
                /*
                if (guiMan.interInput)
                {
                    // Si el usuario presiona E, terminar tipeo instantáneo
                    textArea.text = text;
                    isTyping = false;
                    yield break;
                }
                */

                timer += Time.deltaTime;
                yield return null;
            }
        }

        // Terminó de escribir naturalmente
        isTyping = false;
    }

    public void DestroyAllChildren(Transform parent)
    {
        // Verifica si el GameObject tiene hijos
        if (parent.childCount > 0)
        {
            // Itera sobre cada hijo
            foreach (Transform child in parent)
            {
                // Destruye el hijo
                Destroy(child.gameObject);
                Debug.Log("Destruyendo hijo: " + child.name);
            }
        }
    }
}
