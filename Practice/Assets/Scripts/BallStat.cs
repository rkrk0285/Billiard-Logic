using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BallStat : MonoBehaviour
{
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
    public bool Interact;
    public string InteractiveAllyName;

    protected int wallBounce;
    protected int ballBounce;

    [Header("Components")]
    [SerializeField] private InfoUI infoUI;

    protected SkillBase skill;
    protected Dictionary<string, Action> InteractiveSkill = new Dictionary<string, Action>();

    private void Start()
    {
        currentHP = MaxHP;
        currentBarrier = 0;
        ResetEndParameter();
        InitializeSkill();
    }
    public virtual void ResetEndParameter()
    {
        currentATK = ATK;
        currentDEF = DEF;
        currentHeal = Heal;
        wallBounce = 0;
        ballBounce = 0;
        Interact = false;
        ShowInfo();
    }
    public void ActiveInteractiveSkill()
    {
        if (InteractiveSkill.ContainsKey(InteractiveAllyName))
        {
            InteractiveSkill[InteractiveAllyName]?.Invoke();
        }
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
        {
            currentBarrier--;
        }
        else
        {
            GameManager.Instance.AddPoint(damage);
            if (currentHP <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        ShowInfo();
    }
    private IEnumerator SlowMotionEffect(float slowDuration, float slowFactor)
    {
        // 게임 시간을 느리게 설정
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // 물리 업데이트 시간도 맞춤

        // 주어진 시간동안 기다림
        yield return new WaitForSecondsRealtime(slowDuration);

        // 시간 복구
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; // 물리 업데이트 시간 복구
    }
    public void TakeHeal(float heal)
    {
        GameManager.Instance.AddPoint(heal);
        currentHP += heal;
        if (currentHP >= MaxHP)
        {
            currentHP = MaxHP;
        }
        ShowInfo();
    }
    private void ShowInfo()
    {
        //hpBar.ShowHpBar(currentHP / MaxHP);                
        //infoUI.ShowAttack(currentATK, ATK);
        infoUI.ShowBarrier(currentBarrier);
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
    public void SetInteractiveAllyName(string name)
    {
        InteractiveAllyName = name;
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
    protected virtual void InitializeSkill()
    {
        skill = GetComponent<SkillBase>();
        if (skill != null)
        {
            skill.Initialize(this);
        }
    }
    public virtual void ResetStartCondition()
    {
        // DO NOTHING
        // Override this function when inherrit this class to child class
    }
    public virtual void ResetEndCondition()
    {
        // DO NOTHING
        // Override this function when inherrit this class to child class
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // For Enemy.
        E_BallState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (ballState == E_BallState.Attacking)
        {
            if (collision.gameObject.CompareTag("Player"))
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
}