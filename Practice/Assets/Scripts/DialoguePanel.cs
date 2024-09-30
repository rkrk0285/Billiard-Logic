using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    private Transform target; // The target object (e.g., the character) to follow
    private Camera mainCamera; // Reference to the main camera


    void Awake()
    {
        mainCamera = Camera.main; // Cache the main camera
    }

    public void UpdateText(string dialogueScript)
    {
        dialogueText.text = dialogueScript;
    }

}