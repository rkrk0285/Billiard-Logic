using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [Range(0, 1)] [SerializeField] private float InstructionEventProbability;
    private bool isAllyTurn;
    public bool _activeInstructionEvent;
    public bool isExtraTurn;
    
    private Queue<GameObject> allyTurnQueue;
    private Queue<GameObject> enemyTurnQueue;
    private GameObject currentTurnObject;
    private GameObject currentInstructionAlly;
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
        ResetInstructionObject();

        if (_activeInstructionEvent)
        {
            _activeInstructionEvent = false;
            currentTurnObject.GetComponent<BallStat>().NotFollowingInstruction();
        }

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
        if (isExtraTurn)
        {                        
            currentTurnObject.GetComponent<BallStat>().InstructionAlly = null;
            currentTurnObject.GetComponent<BallController>().ChangeState(E_BallState.Ready);            
        }
        else if (isAllyTurn)
        {
            if (allyTurnQueue.Count > 0)
            {
                currentTurnObject = allyTurnQueue.Dequeue();
                allyTurnQueue.Enqueue(currentTurnObject);

                if (currentTurnObject.GetComponent<BallStat>().GetSkipNextTurn())
                {
                    GoToNextTurn();
                    return;
                }

                currentTurnObject.GetComponent<BallController>().ChangeState(E_BallState.Ready);
                if (!currentTurnObject.name.Equals("Ghost"))
                {
                    float rand = Random.Range(0f, 1f);
                    if (rand <= InstructionEventProbability)
                        SetInstructionObject(currentTurnObject);
                }
            }
        }
        else
        {
            if (enemyTurnQueue.Count > 0)
            {
                currentTurnObject = enemyTurnQueue.Dequeue();
                enemyTurnQueue.Enqueue(currentTurnObject);                                
                currentTurnObject.GetComponent<EnemyBallAI>().AIShooting();                
            }
        }

        currentTurnObject.GetComponent<BallStat>().ResetStartCondition();
        readyButton.interactable = false;
        oneMoreButton.interactable = false;
    }
    public void SetInstructionObject(GameObject currObj)
    {        
        Queue<GameObject> newAllyQueue = new Queue<GameObject>(allyTurnQueue);

        List<GameObject> AllyList = new List<GameObject>();
        List<GameObject> EnemyList = enemyTurnQueue.ToList();

        while (newAllyQueue.Count > 0)
        {
            GameObject obj = newAllyQueue.Dequeue();
            if (obj != null && obj.name != "Ghost" && currObj != obj)
                AllyList.Add(obj);
        }

        if (AllyList.Count != 0)
        {            
            int rand = Random.Range(0, AllyList.Count);
            currentTurnObject.GetComponent<BallStat>().SetInstructionAlly(AllyList[rand]);
            AllyList[rand].GetComponent<BallStat>().HandsUp();
        }
        
        if (EnemyList.Count != 0)
        {
            int rand = Random.Range(0, EnemyList.Count);
            currentTurnObject.GetComponent<BallStat>().SetInstructionEnemy(EnemyList[rand]);
            EnemyList[rand].GetComponent<BallStat>().HandsUp();
        }

        _activeInstructionEvent = true;
    }
    public void ResetInstructionObject()
    {
        Queue<GameObject> newAllyQueue = new Queue<GameObject>(allyTurnQueue);
        Queue<GameObject> newEnemyQueue = new Queue<GameObject>(enemyTurnQueue);
        while (newAllyQueue.Count > 0)
        {
            GameObject obj = newAllyQueue.Dequeue();
            obj.GetComponent<BallStat>().HandsDown();
        }
        while (newEnemyQueue.Count > 0)
        {
            GameObject obj = newEnemyQueue.Dequeue();
            obj.GetComponent<BallStat>().HandsDown();
        }
    }    
    public void OnClickReloadButton()
    {
        augment.SetActive(true);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
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
