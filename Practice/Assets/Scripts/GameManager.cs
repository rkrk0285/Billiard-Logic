using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("GameObject")]
    [SerializeField] private Transform AllyTransform;
    [SerializeField] private Transform EnemyTransform;
    [SerializeField] private GameObject SkeletonObj;
    [SerializeField] private List<DamageZone> damageZones; // 모든 데미지 구역 참조

    [Header("Parameters")]
    private int remainTurnCount;
    private int currentAllyTurn;
    private int currentEnemyTurn;
    private GameObject currentObject;
    private const int TURN_COUNT = 8;

    public Action TurnEndEvent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        Initialize();
    }    
    private void Initialize()
    {
        remainTurnCount = 0;
        currentAllyTurn = 0;
        currentEnemyTurn = 0;
    }
    IEnumerator TurnEndAction()
    {
        while (remainTurnCount < TURN_COUNT)
        {
            if (remainTurnCount % 2 == 0)
            {
                while (currentAllyTurn < AllyTransform.childCount && !AllyTransform.GetChild(currentAllyTurn).gameObject.activeSelf)
                {
                    currentAllyTurn++;
                }

                if (currentAllyTurn < AllyTransform.childCount)
                {
                    AllyTransform.GetChild(currentAllyTurn).GetComponent<MonsterStat>().OnNotifyTurnEnd();
                    currentAllyTurn++;
                }
            }
            else
            {
                while (currentEnemyTurn < EnemyTransform.childCount && !EnemyTransform.GetChild(currentEnemyTurn).gameObject.activeSelf)
                {
                    currentEnemyTurn++;
                }

                if (currentEnemyTurn < EnemyTransform.childCount)
                {
                    EnemyTransform.GetChild(currentEnemyTurn).GetComponent<MonsterStat>().OnNotifyTurnEnd();
                    currentEnemyTurn++;
                }
            }
            remainTurnCount++;
            ApplyDamageZones();

            yield return new WaitForSeconds(1f);
        }

        remainTurnCount = 0;
        currentAllyTurn = 0;
        currentEnemyTurn = 0;        
    }
    private void ApplyDamageZones()
    {
        foreach (var zone in damageZones)
        {
            zone.ApplyDamage();
        }
    }
    // For Debug.
    public void OnClickReadyButton(GameObject clickedObj)
    {
        currentObject = clickedObj;
        clickedObj.GetComponent<MonsterController>().ChangeState(E_MonsterState.Ready);
    }
    public void OnClickTurnEndButton()
    {
        //StartCoroutine(TurnEndAction());
        TurnEndEvent?.Invoke();
        currentObject.GetComponent<MonsterStat>().OnNotifyTurnEnd();
        ApplyDamageZones();

    }
    public void OnClickClearLineRenderer()
    {
        for (int i = 0; i < AllyTransform.childCount; i++)
        {
            AllyTransform.GetChild(i).GetComponent<LineRenderer>().enabled = false;
        }
        for (int i = 0; i < EnemyTransform.childCount; i++)
        {
            EnemyTransform.GetChild(i).GetComponent<LineRenderer>().enabled = false;
        }
    }
    public void OnClickRandomEnemy()
    {
        List<GameObject> aliveEnemy = new List<GameObject>();
        for (int i = 0; i < EnemyTransform.childCount; i++)
        {
            if (EnemyTransform.GetChild(i).gameObject.activeSelf)
                aliveEnemy.Add(EnemyTransform.GetChild(i).gameObject);
        }

        int rand = UnityEngine.Random.Range(0, aliveEnemy.Count);
        aliveEnemy[rand].GetComponent<EnemyBallAI>().AIStraightShooting();
    }
    public void OnClickReload()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    public void OnClickBreak()
    {
        currentObject.GetComponent<MonsterController>().BreakMonster();
    }
    private void Update()
    {        
        if (Input.GetMouseButtonDown(1))
            OnClickBreak();
    }
    public Transform GetAllyTransform()
    {
        return AllyTransform;
    }
    public Transform GetEnemyTransform()
    {
        return EnemyTransform;
    }
}
