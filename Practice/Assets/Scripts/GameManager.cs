using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("GameObject")]
    [SerializeField] private Transform AllyTrans;
    [SerializeField] private Transform EnemyTrans;

    [Header("Components")]
    [SerializeField] private Button readyButton;

    [Header("Parameters")]    
    private bool isAllyTurn;
    public bool isExtraTurn;
    
    private Queue<GameObject> allyTurnQueue;
    private Queue<GameObject> enemyTurnQueue;

    private GameObject currentTurnObject;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        InitializeList();        
        isAllyTurn = true;
        isExtraTurn = false;
    }
    private void InitializeList()
    {
        allyTurnQueue = new Queue<GameObject>();
        enemyTurnQueue = new Queue<GameObject>();

        for(int i = 0; i < AllyTrans.childCount; i++)
        {
            allyTurnQueue.Enqueue(AllyTrans.GetChild(i).gameObject);
        }
        for (int i = 0; i < EnemyTrans.childCount; i++)
        {
            enemyTurnQueue.Enqueue(EnemyTrans.GetChild(i).gameObject);
        }
    }
    private void UpdateQueue()
    {        
        Queue<GameObject> newAllyQueue = new Queue<GameObject>();
        Queue<GameObject> newEnemyQueue = new Queue<GameObject>();
        while(allyTurnQueue.Count > 0)
        {
            GameObject obj = allyTurnQueue.Dequeue();

            if (obj != null)
                newAllyQueue.Enqueue(obj);
        }
        while (enemyTurnQueue.Count > 0)
        {
            GameObject obj = enemyTurnQueue.Dequeue();

            if (obj != null)
                newEnemyQueue.Enqueue(obj);
        }
        allyTurnQueue = newAllyQueue;
        enemyTurnQueue = newEnemyQueue;        
    }
    public void TurnEnd()
    {        
        if (currentTurnObject.GetComponent<BallStat>().Interact)        
            currentTurnObject.GetComponent<BallStat>().ActiveInteractiveSkill();        
        else
            GoToNextTurn();
    }
    public void GoToNextTurn()
    {        
        currentTurnObject.GetComponent<BallStat>().ResetEndParameter();
        currentTurnObject.GetComponent<BallController>().ResetPhysicsParameter();

        UpdateQueue();
        isAllyTurn = !isAllyTurn;
        isExtraTurn = false;
        readyButton.interactable = true;
    }
    public void GoToExtraTurn()
    {
        isExtraTurn = true;
        readyButton.interactable = true;
    }
    public void GoToExtraTurn(GameObject obj)
    {
        currentTurnObject = obj;
        isExtraTurn = true;
        readyButton.interactable = true;
    }
    public void OnClickReadyButton()
    {
        ResetHandsUpAlly();        
        if (isExtraTurn)
        {            
            currentTurnObject.GetComponent<BallStat>().ResetStartCondition();
            currentTurnObject.GetComponent<BallStat>().InteractiveAllyName = null;
            currentTurnObject.GetComponent<BallController>().ChangeState(E_BallState.Ready);            
        }
        else if (isAllyTurn)
        {
            if (allyTurnQueue.Count > 0)
            {
                currentTurnObject = allyTurnQueue.Dequeue();
                allyTurnQueue.Enqueue(currentTurnObject);
                CheckHandsUpAlly(currentTurnObject);
                
                currentTurnObject.GetComponent<BallStat>().ResetStartCondition();
                currentTurnObject.GetComponent<BallController>().ChangeState(E_BallState.Ready);
            }
        }
        else
        {
            if (enemyTurnQueue.Count > 0)
            {
                currentTurnObject = enemyTurnQueue.Dequeue();
                enemyTurnQueue.Enqueue(currentTurnObject);
                
                currentTurnObject.GetComponent<BallStat>().ResetStartCondition();
                currentTurnObject.GetComponent<EnemyBallAI>().AIShooting();                
            }
        }
        
        readyButton.interactable = false;
    }
    public void CheckHandsUpAlly(GameObject currObj)
    {        
        Queue<GameObject> newAllyQueue = new Queue<GameObject>(allyTurnQueue);
        List<GameObject> possibleAlly = new List<GameObject>();

        while (newAllyQueue.Count > 0)
        {
            GameObject obj = newAllyQueue.Dequeue();
            if (obj != null && currObj != obj)
                possibleAlly.Add(obj);
        }

        if (possibleAlly.Count != 0)
        {            
            int rand = Random.Range(0, possibleAlly.Count);
            currentTurnObject.GetComponent<BallStat>().SetInteractiveAllyName(possibleAlly[rand].name);
            possibleAlly[rand].GetComponent<BallStat>().HandsUp();
        }
    }

    public bool isPossibleToUseInteractive(string currName, string targetName)
    {
        switch (currName)
        {
            case "Skeleton":
                break;

            case "Goblin":
                if (targetName.Equals("Golem"))
                    return true;
                break;

            case "Golem":                
                if (targetName.Equals("Skeleton"))
                    return true;
                break;

            case "Ghost":
                if (targetName.Equals("Skeleton"))
                    return true;
                break;
        }
        return false;
    }
    public void ResetHandsUpAlly()
    {
        Queue<GameObject> newAllyQueue = new Queue<GameObject>(allyTurnQueue);
        while (newAllyQueue.Count > 0)
        {
            GameObject obj = newAllyQueue.Dequeue();
            obj.GetComponent<BallStat>().HandsDown();
        }
    }    
    public void OnClickReloadButton()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    // For Debug
    public void OnClickOneMoreButton()
    {
        GoToExtraTurn(currentTurnObject);
    }
}
