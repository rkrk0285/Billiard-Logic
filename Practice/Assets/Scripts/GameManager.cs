using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Button oneMoreButton;    

    [Header("Parameters")]    
    private bool isAllyTurn;
    public bool isExtraTurn;
    
    private Queue<GameObject> allyTurnQueue;
    private Queue<GameObject> enemyTurnQueue;
    private GameObject currentTurnObject;    

    //ADDED
    public GameObject augment;
    public PhysicsMaterial2D pm;

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

            if (obj.activeSelf)            
                newAllyQueue.Enqueue(obj);                
        }
        while (enemyTurnQueue.Count > 0)
        {
            GameObject obj = enemyTurnQueue.Dequeue();

            if (obj.activeSelf)            
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
        oneMoreButton.interactable = true;
    }
    public void GoToExtraTurn(GameObject obj)
    {
        UpdateQueue();        
        isExtraTurn = true;
        readyButton.interactable = true;
        oneMoreButton.interactable = true;

        currentTurnObject = obj;
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
                //CheckHandsUpAlly(currentTurnObject);
                
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
        oneMoreButton.interactable = false;
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
        augment.SetActive(true);
    }
    public void OnClickResetButton()
    {
        pm.bounciness = 0.9f;
        PlayerPrefs.DeleteAll();
    }
    
    // For Debug
    public void OnClickOneMoreButton()
    {
        isAllyTurn = !isAllyTurn;
        GoToExtraTurn(currentTurnObject);
    }    
}
