using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BallStat : MonoBehaviour
{
    [Header("Objects")]
    public GameObject holePrefab;
    public GameObject holePrefabParents;

    [Header("Parameters")]
    [SerializeField] protected float MaxHP;
    [SerializeField] protected float ATK;
    [SerializeField] protected float DEF;
    [SerializeField] protected float Heal;

    public float currentHP;
    public float currentATK;
    public float currentDEF;
    public float currentHeal;
    public int currentBarrier;    
    
    protected int wallBounce;
    protected int ballBounce;

    public bool Interact;
    public GameObject InstructionAlly;
    public GameObject InstructionEnemy;

    [SerializeField] private float _decreasePower = 0.25f;
    [SerializeField] private float _increasePower = 4f;
    [SerializeField] protected float _InstructionDamageMultiplier = 1.5f;
    private bool skipNextTurn;

    [Header("Components")]
    [SerializeField] private InfoUI infoUI;
    
    protected Dictionary<string, Action> InteractiveSkill = new Dictionary<string, Action>();

    private void Start()
    {
        currentHP = MaxHP;
        currentBarrier = 0;
        ResetEndParameter();        
    }
    public virtual void ResetEndParameter()
    {
        currentATK = ATK;
        currentDEF = DEF;
        currentHeal = Heal;
        wallBounce = 0;
        ballBounce = 0;

        Interact = false;
        InstructionAlly = null;
        InstructionEnemy = null;
        skipNextTurn = false;
        ShowInfo();
    }
    public void ActiveInteractiveSkill()
    {        
        if (InteractiveSkill.ContainsKey(InstructionAlly.name))
            InteractiveSkill[InstructionAlly.name]?.Invoke();        
    }
    public void HandsUp()
    {
        transform.Find("HandsUp").gameObject.SetActive(true);
    }
    public void HandsDown()
    {
        transform.Find("HandsUp").gameObject.SetActive(false);
    }
    public void TakeDamage(float damage)
    {        
        if (currentBarrier > 0)        
            currentBarrier--;        
        else
        {
            currentHP -= damage;            
            if (currentHP <= 0)            
                Dead();
        }
        ShowInfo();
    }
    public void TakeHeal(float heal)
    {
        currentHP += heal;
        if (currentHP >= MaxHP)
        {
            currentHP = MaxHP;
        }
        ShowInfo();
    }

    public void Dead()
    {        
        gameObject.SetActive(false);        
        if (holePrefab != null)        
            Instantiate(holePrefab, transform.position, Quaternion.identity, holePrefabParents.transform);
    }
    private void ShowInfo()
    {
        infoUI.ShowHpBar(currentHP / MaxHP);
        infoUI.ShowBarrier(currentBarrier);
        infoUI.ShowSkip(skipNextTurn);
    }
    public int GetBounceCount()
    {
        return ballBounce + wallBounce;
    }
    public int GetWallBounceCount()
    {
        return wallBounce;
    }
    public int GetBallBounceCount()
    {
        return ballBounce;
    }    
    public void SetInstructionAlly(GameObject obj)
    {
        InstructionAlly = obj;
    }
    public void SetInstructionEnemy(GameObject obj)
    {
        InstructionEnemy = obj;
    }
    public void AddBarrierCount(int amount)
    {
        currentBarrier += amount;
        ShowInfo();
    }
    public void IncreaseDamageMultiplier(float multiplier)
    {
        currentATK *= multiplier;
    }
    public void IncreaseDamage(float amount)
    {
        currentATK += amount;
        ShowInfo();
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // For Enemy.
        E_MonsterState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (ballState == E_MonsterState.Moving)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
            {
                ballBounce++;
                collision.gameObject.GetComponent<BallStat>().TakeDamage(currentATK);
            }
            else
            {
                wallBounce++;
            }
        }
    }
    public void SetSkipNextTurn()
    {
        skipNextTurn = true;
        ShowInfo();
    }
    public bool GetSkipNextTurn()
    {
        return skipNextTurn;
    }
    public virtual void ResetStartCondition()
    {   
        // DO NOTHING
        // Override this function when inherrit this class to child class
    }
    public virtual void ResetEndCondition()
    {        
    }

    public void NotFollowingInstruction()
    {
        if (InstructionAlly != null)
        {
            int rand = UnityEngine.Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    Debug.Log(InstructionAlly.name + " 속도 4배");
                    InstructionAlly.GetComponent<BallController>().IncreasePowerMultiplier(_increasePower);
                    break;
                case 1:
                    Debug.Log(InstructionAlly.name + " 속도 0.25배");
                    InstructionAlly.GetComponent<BallController>().DecreasePowerMultiplier(_decreasePower);
                    break;
                case 2:
                    Debug.Log(InstructionAlly.name + " 턴 스킵");
                    InstructionAlly.GetComponent<BallStat>().SetSkipNextTurn();
                    break;
            }
        }
        
    }

    //protected IEnumerator SlowMotionEffect(float slowDuration, float slowFactor)
    //{
    //    // ���� �ð��� ������ ����
    //    Time.timeScale = slowFactor;
    //    Time.fixedDeltaTime = 0.02f * Time.timeScale; // ���� ������Ʈ �ð��� ����

    //    // �־��� �ð����� ��ٸ�
    //    yield return new WaitForSecondsRealtime(slowDuration);

    //    // �ð� ����
    //    Time.timeScale = 1f;
    //    Time.fixedDeltaTime = 0.02f; // ���� ������Ʈ �ð� ����
    //}
}