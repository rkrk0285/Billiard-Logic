using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BallStat : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] public float MaxHP;
    [SerializeField] protected float ATK;
    [SerializeField] protected float DEF;

    public float currentHP;
    public float currentATK;
    public float currentDEF;
    public bool Interact;
    public string InteractiveAllyName;

    protected int wallBounce;
    protected int ballBounce;

    [Header("Components")]
    [SerializeField] private HpBar hpBar;

    protected SkillBase skill;
    protected Dictionary<string, Action> InteractiveSkill = new Dictionary<string, Action>();

    private void Start()
    {
        currentHP = MaxHP;
        ResetActionParameter();
        InitializeSkill();
    }
    public virtual void ResetActionParameter()
    {
        currentATK = ATK;
        currentDEF = DEF;
        wallBounce = 0;
        ballBounce = 0;
        Interact = false;
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
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Destroy(this.gameObject);
        }
        ShowDamage();
    }
    public void TakeHeal(float heal)
    {
        currentHP += heal;
        if (currentHP >= MaxHP)
        {
            currentHP = MaxHP;
        }
        ShowDamage();
    }
    private void ShowDamage()
    {
        hpBar.ShowHpBar(currentHP / MaxHP);
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
    public void IncreaseDamageMultiplier(float multiplier)
    {
        currentATK *= multiplier;
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
        E_BallState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (ballState == E_BallState.Attacking)
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
            //skill?.ActivateSkill();
        }
    }
}