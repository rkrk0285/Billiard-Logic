using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Transform target; // The target object (e.g., the character) to follow
    private Camera mainCamera; // Reference to the main camera

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        mainCamera = Camera.main; // Cache the main camera
    }

    public void UpdateText(string dialogueScript)
    {
        text.text = dialogueScript;
    }

}