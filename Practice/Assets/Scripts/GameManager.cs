using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("GameObject")]
    [SerializeField] private Transform AlleyTrans;
    [SerializeField] private Transform EnemyTrans;

    [Header("Components")]
    [SerializeField] private Button readyButton;

    [Header("Parameters")]
    private int alleyTurnIndex;
    private int enemyTurnIndex;
    private bool isAlleyTurn;
    public Queue<GameObject> alleyQueue;
    public Queue<GameObject> enemyQueue;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        InitializeQueue();
        alleyTurnIndex = 0;
        enemyTurnIndex = -1;
        isAlleyTurn = true;
    }

    private void InitializeQueue()
    {
        alleyQueue = new Queue<GameObject>();
        enemyQueue = new Queue<GameObject>();

        for(int i = 0; i < AlleyTrans.childCount; i++)
        {
            alleyQueue.Enqueue(AlleyTrans.GetChild(i).gameObject);
        }
        for (int i = 0; i < EnemyTrans.childCount; i++)
        {
            enemyQueue.Enqueue(EnemyTrans.GetChild(i).gameObject);
        }
    }
    private void UpdateQueue()
    {
        Queue<GameObject> newAlleyQueue = new Queue<GameObject>();
        Queue<GameObject> newEnemyQueue = new Queue<GameObject>();
        while(alleyQueue.Count > 0)
        {
            GameObject obj = alleyQueue.Dequeue();

            if (obj != null)
                newAlleyQueue.Enqueue(obj);
        }
        while (enemyQueue.Count > 0)
        {
            GameObject obj = enemyQueue.Dequeue();

            if (obj != null)
                newEnemyQueue.Enqueue(obj);
        }
        alleyQueue = newAlleyQueue;
        enemyQueue = newEnemyQueue;        
    }
    public void GoNextTurn()
    {
        UpdateQueue();
        isAlleyTurn = !isAlleyTurn;        
        readyButton.interactable = true;
    }

    public void OnClickReadyButton()
    {
        GameObject obj;
        if (isAlleyTurn)
        {
            if (alleyQueue.Count > 0)
            {
                obj = alleyQueue.Dequeue();
                obj.GetComponent<BallController>().ChangeState(E_BallState.Ready);
                alleyQueue.Enqueue(obj);
            }
        }
        else
        {
            if (enemyQueue.Count > 0)
            {
                obj = enemyQueue.Dequeue();
                obj.GetComponent<EnemyBallAI>().AIShooting();
                enemyQueue.Enqueue(obj);
            }
        }
        
        readyButton.interactable = false;
    }
    public void OnClickReloadButton()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
