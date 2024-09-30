using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDisplayManager : MonoBehaviour
{
    public class DialogueEntry
    {
        public string dialogue;
        public System.Func<bool> condition; // Condition function to trigger dialogue
        public bool displayed;

        public DialogueEntry(string dialogue, System.Func<bool> condition, bool displayed)
        {
            this.dialogue = dialogue;
            this.condition = condition;
            this.displayed = displayed;
        }
    }

    // Dialogue list for the character
    private List<DialogueEntry> dialogueList = new List<DialogueEntry>();

    // Character stats (you would probably get these from another script in a real game)
    private float maxHP;
    private float currentHP;
    private int remainingEnemies;
    private int remainingCharacters;
    private int Position;

    // Cooldown for how often dialogue can be triggered (optional, to avoid spamming)
    public float dialogueCooldown = 5.0f;
    private float lastDialogueTime = -Mathf.Infinity;
    private float dialogueDuration = 2f;
    public GameObject dialoguePanelPrefab;
    private Queue<GameObject> characters;
    public Canvas canvas;
    private GameObject targetCharacter;
    private void Update()
    {
        remainingEnemies = GameManager.Instance.enemyQueue.Count;
        remainingCharacters = GameManager.Instance.alleyQueue.Count;
        characters = GameManager.Instance.alleyQueue;

        CheckDialogueDuringCombat();

        /*foreach (GameObject character in characters)
        {
            if(character != null)
            {
                BallStat ballStat = character.GetComponent<BallStat>();

                currentHP = ballStat.GetCurrentHP();
                maxHP = ballStat.GetMaxHP();
                Vector3 Position = character.transform.position;
                CheckDialogueDuringCombat();
            }
        }*/
    }
    void Awake()
    {
        // Populate dialogue list with conditions
        dialogueList.Add(new DialogueEntry("We're ready to roll out!", () => remainingCharacters == 4, false));
        dialogueList.Add(new DialogueEntry("I'm not sure I can hold on!", () => currentHP / maxHP < 0.25f, false));
        dialogueList.Add(new DialogueEntry("Only one left! Let's finish this!", () => remainingEnemies == 1, false));
        dialogueList.Add(new DialogueEntry("Stay focused, we can still win!", () => currentHP / maxHP < 0.5f, false));
        dialogueList.Add(new DialogueEntry("Is that all they've got?", () => remainingEnemies < 3, false));
    }

    // This method can be called at any time during combat
    public void CheckDialogueDuringCombat()
    {
        if (Time.time - lastDialogueTime < dialogueCooldown)
            return;

        
        // Check each dialogue entry to see if conditions are met
        foreach (DialogueEntry entry in dialogueList)
        {
            if (entry.dialogue != null && entry.condition() && !entry.displayed)
            {
                
                entry.displayed = true;
                Debug.Log(entry.dialogue);
                StartCoroutine(TriggerDialogue(entry.dialogue));
                lastDialogueTime = Time.time; // Reset cooldown timer
                break; // Trigger only one dialogue at a time
            }
        }
        
    }
    private IEnumerator TriggerDialogue(string dialogue)
    {
        int randomInt = Random.Range(0, remainingCharacters);
        targetCharacter = characters.ToArray()[randomInt];
        
        // Instantiate the panel as a child of the target character
        GameObject instantiatedPanel = Instantiate(dialoguePanelPrefab, targetCharacter.transform);

        DialoguePanel dialoguePanelScript = instantiatedPanel.GetComponent<DialoguePanel>();
        dialoguePanelScript.UpdateText(dialogue);
        
        // Wait for the specified duration
        yield return new WaitForSeconds(dialogueDuration);


        // Destroy the dialogue panel after the duration
     
        Destroy(instantiatedPanel);
    }
}
