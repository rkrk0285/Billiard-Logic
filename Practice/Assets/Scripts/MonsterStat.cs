using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonoBehaviour
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

    private float originalATK;
    private float originalDEF;

    protected int wallBounce;
    protected int ballBounce;

    [Header("Components")]
    [SerializeField] private InfoUI infoUI;    
    
    protected Action skill;
    
    private void Awake()
    {
        InitializeParameter();
    }
    public virtual void ResetStartParameter()
    {
        // Do Nothing.
        // Todo. Call this function When this Monster's turn started
    }
    public virtual void ResetEndParameter()
    {
        // Do Nothing.
        // Todo. Call this function When this Monster's turn ended
    }
    private void InitializeParameter()
    {
        currentHP = MaxHP;
        skill += () => GetComponent<SkillBase>().Activate();
    }
    public virtual void TakeDamage(float damage)
    {        
        currentHP -= damage;
        if (currentHP <= 0)
            Dead();
        ShowInfo();
    }
    public void TakeHeal(float heal)
    {        
        currentHP += heal;
        if (currentHP >= MaxHP)
            currentHP = MaxHP;
        ShowInfo();
    }
    protected void ShowInfo()
    {
        infoUI.ShowHpBar(currentHP / MaxHP);
    }    
    public virtual void OnNotifyTurnEnd()
    {        
        skill?.Invoke();
    }
    protected virtual void Dead()
    {
        // Do Nothing.
        // Override this function.
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Do Nothing.
        // Override this function.
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // Do Nothing.
        // Override this function.
    }
    public void IncreaseDamageAmount(float addAmount)
    {
        originalATK = currentATK;
        currentATK += addAmount;
    }

    public void IncreaseDefenseAmount(float addAmount)
    {
        originalDEF = currentDEF;
        currentDEF += addAmount;
    }

    public void DecreaseDamageAmount(float addAmount)
    {
        currentATK -= addAmount;
    }

    public void DecreaseDefenseAmount(float addAmount)
    {
        currentDEF -= addAmount;
    }

    public bool IsAttackBuffedOrDebuffed()
    {
        if(originalATK != currentATK)
        {
            return true;
        }
        return false;
    }

    public bool IsDefenseBuffedOrDebuffed()
    {
        if (originalDEF != currentDEF)
        {
            return true;
        }
        return false;
    }
}
