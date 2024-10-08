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

    protected int wallBounce;
    protected int ballBounce;

    [Header("Components")]
    [SerializeField] private InfoUI infoUI;
    private Action skill;
    
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
    public void TakeDamage(float damage)
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
    private void ShowInfo()
    {
        infoUI.ShowHpBar(currentHP / MaxHP);
    }
    private void Dead()
    {
        // Todo. Dead Function
        gameObject.SetActive(false);
    }
    public virtual void OnNotifyTurnEnd()
    {
        // Todo. Call this funcion When Any Character's turn ended.
        skill?.Invoke();
    }
    public void IncreaseDamageMultiplier(float multiplier)
    {
        currentATK *= multiplier;
    }
}
