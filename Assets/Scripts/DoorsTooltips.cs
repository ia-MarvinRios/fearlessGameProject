using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorsTooltips : MonoBehaviour
{
    [Header("Tooltip Settings")]
    [Space(10)]
    [Tooltip("Text to display when door is locked.")]
    [SerializeField] string lockedText = "Door is locked";
    [SerializeField] Sprite lockedIcon = null;
    [Space(5)]
    [Tooltip("Text to display when door is unlocked.")]
    [SerializeField] string unlockedText = "Door is unlocked";
    [SerializeField] Sprite unlockedIcon = null;

    TMP_Text text;
    Image icon;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        icon = GetComponentInChildren<Image>();
    }

    private void OnDisable()
    {
        text.text = unlockedText;
        icon.sprite = unlockedIcon;
    }

    public void ShowLockedTooltip()
    {
        StartCoroutine(SLTt());
    }

    private IEnumerator SLTt()
    {
        text.text = lockedText;
        icon.sprite = lockedIcon;
        yield return new WaitForSeconds(1f);
        text.text = unlockedText;
        icon.sprite = unlockedIcon;
    }
}