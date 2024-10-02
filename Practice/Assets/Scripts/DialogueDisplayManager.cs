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
    private GameObject targetCharacter;
    private void Update()
    {
        remainingEnemies = GameManager.Instance.GetEnemyQueue().Count;
        remainingCharacters = GameManager.Instance.GetAllyQueue().Count;
        characters = GameManager.Instance.GetEnemyQueue();

        CheckDialogueDuringCombat();

        foreach (GameObject character in characters)
        {
            if(character != null)
            {
                BallStat ballStat = character.GetComponent<BallStat>();
                currentHP = ballStat.GetCurrentHP();
                maxHP = ballStat.GetMaxHP();
                CheckDialogueDuringCombat();
            }
        }
    }
    void Awake()
    {
        // Populate dialogue list with conditions
        dialogueList.Add(new DialogueEntry("We're ready to roll out!", () => remainingCharacters == 4, false));
        dialogueList.Add(new DialogueEntry("There's three left!", () => remainingCharacters == 3, false));
        dialogueList.Add(new DialogueEntry("There's two left!", () => remainingCharacters == 2, false));
        dialogueList.Add(new DialogueEntry("There's one left!", () => remainingCharacters == 1, false));
        dialogueList.Add(new DialogueEntry("Only one left! Let's finish this!", () => remainingEnemies == 1, false));

        dialogueList.Add(new DialogueEntry("I'm not sure I can hold on!", () => currentHP / maxHP < 0.25f, false));
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
        Debug.Log("Hello");
        int randomInt = Random.Range(0, remainingCharacters);
        targetCharacter = characters.ToArray()[randomInt];

        DialoguePanel[] allPanels = FindObjectsOfType<DialoguePanel>();
        BallController ballController = targetCharacter.GetComponent<BallController>();
 
        foreach (DialoguePanel panel in allPanels) 
        {
            if (panel.currentCharacter == ballController.currentCharacter)
            {
                panel.UpdateText(dialogue);
                panel.transform.GetChild(0).gameObject.SetActive(true);
            }
            
        }

        // Wait for the specified duration
        yield return new WaitForSeconds(dialogueDuration);

        foreach (DialoguePanel panel in allPanels)
        {
                panel.gameObject.SetActive(false);
        }
    }
}
