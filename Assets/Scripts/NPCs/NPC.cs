using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("GUI Manager Reference")]
    [SerializeField] private GuiManager guiMan;

    [Header("Dialogues")]
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private int currentPhase = 1;
    [SerializeField] private int dialogueCooldown = 2;
    private int currentId = 1;
    public bool availableForDialogue = false;
    bool waitingForOption = false;
    bool doneWithDialogues = true;

    private bool isTyping = false;
    private float typingSpeed = 0.05f;

    // Getters and Setters
    public int CurrentPhase { get { return currentPhase; } set { currentPhase = value; } }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is within the detection range
            Debug.Log("Player entered the trigger zone!");

            availableForDialogue = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is out of the detection range
            Debug.Log("Player left the trigger zone!");
            availableForDialogue = false;
            CancelDialogue();
        }
    }



    public IEnumerator StartDialogue()
    {
        if (availableForDialogue && !isTyping && !waitingForOption)
        {
            availableForDialogue = false;

            // Set UI
            guiMan.lowerPanel.SetActive(true);
            guiMan.textArea.text = "";

            StartCoroutine(ShowDialogue());
            yield return new WaitUntil(() => doneWithDialogues);
            StartCoroutine(WaitDialogueCooldown());
        }
        else yield break;
    }

    IEnumerator ShowDialogue()
    {
        doneWithDialogues = false;
        foreach (Dialogue dialogue in dialogueData.Dialogues)
        {
            if (dialogue.id == currentId && dialogue.phase == currentPhase)
            {
                Debug.Log("Showing dialogue: " + dialogue.text);

                // Set UI
                guiMan.TextArea.gameObject.SetActive(true);
                guiMan.buttonsLayout.gameObject.SetActive(false);

                // Show text
                yield return StartCoroutine(TypeText(dialogue.text, guiMan.TextArea));

                // Wait for user input to continue
                yield return new WaitForSeconds(1f);

                // Show options if any
                if (dialogue.options.Length > 0)
                {
                    // Unlock cursor and disable input
                    Cursor.lockState = CursorLockMode.None;
                    guiMan.inputScript.enabled = false;

                    yield return StartCoroutine(ShowOptions(dialogue.options, guiMan.optionButton));

                    // Lock cursor and enable input
                    Cursor.lockState = CursorLockMode.Locked;
                    guiMan.inputScript.enabled = true;
                }
                
                if (dialogue.nextDialogueId != 0)
                {
                    currentId = dialogue.nextDialogueId;
                }
                if (dialogue.nextPhaseId != 0)
                {
                    currentPhase = dialogue.nextPhaseId;
                }

                CheckCurrentPhaseForMissions();
            }
        }
        doneWithDialogues = true;
    }

    IEnumerator ShowOptions(Dialogue.Option[] options, GameObject btnPrefab)
    {
        waitingForOption = true;

        // Set UI
        DestroyAllChildren(guiMan.buttonsLayout);
        guiMan.buttonsLayout.gameObject.SetActive(true);
        guiMan.TextArea.gameObject.SetActive(false);

        // Create buttons
        foreach (Dialogue.Option option in options)
        {
            GameObject btn = Instantiate(btnPrefab, guiMan.buttonsLayout);
            btn.GetComponentInChildren<TMP_Text>().text = option.text;
            btn.GetComponent<Button>().onClick.AddListener(() => OnOptionSelected(option));
        }

        yield return new WaitUntil(() => !waitingForOption);
    }
    private void OnOptionSelected(Dialogue.Option option)
    {
        currentId = option.nextDialogueId;
        waitingForOption = false;
        DestroyAllChildren(guiMan.buttonsLayout);
        //StartCoroutine(ShowDialogue());
    }

    IEnumerator TypeText(string text, TMP_Text textArea)
    {
        isTyping = true;
        textArea.text = "";

        foreach (char letter in text)
        {
            textArea.text += letter;

            float timer = 0;
            while (timer < typingSpeed)
            {
                /*
                if (guiMan.interInput)
                {
                    guiMan.interInput = false;
                    textArea.text = text;
                    isTyping = false;
                    yield break;
                }
                */

                timer += Time.deltaTime;
                yield return null;
            }
        }
        isTyping = false;
    }

    public void DestroyAllChildren(Transform parent)
    {
        if (parent.childCount > 0)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
                Debug.Log("Destruyendo hijo: " + child.name);
            }
        }
    }

    IEnumerator WaitDialogueCooldown()
    {
        guiMan.lowerPanel.SetActive(false);
        yield return new WaitForSeconds(dialogueCooldown);
        availableForDialogue = true;

        StopAllCoroutines();
    }

    private void CancelDialogue()
    {
        StopAllCoroutines();
        guiMan.textArea.text = "";
        guiMan.lowerPanel.SetActive(false);
        DestroyAllChildren(guiMan.buttonsLayout);

        // Reset variables
        availableForDialogue = true;
        doneWithDialogues = true;
        waitingForOption = false;
        isTyping = false;
    }

    private void CheckCurrentPhaseForMissions()
    {
        NpcController irahetaNPC = gameObject.GetComponent<NpcController>();

        if (currentPhase == 2)
        {
            irahetaNPC.AceptarMision();
        }
    }
}
