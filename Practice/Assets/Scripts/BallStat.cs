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
    
    private float currentHP;
    public float currentATK;
    public float currentDEF;
    protected int wallBounce;
    protected int ballBounce;

    [Header("Components")]
    [SerializeField] private HpBar hpBar;
    
    protected SkillBase skill;

    private void Start()
    {
        currentHP = MaxHP;
        ResetActionParameter();

        skill = GetComponent<SkillBase>();
        if (skill != null)
        {            
            skill.Initialize(this);
        }
    }
    public void ResetActionParameter()
    {
        currentATK = ATK;
        currentDEF = DEF;
        wallBounce = 0;
        ballBounce = 0;                
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        E_BallState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {            
            E_BallState collisionBallState = collision.gameObject.GetComponent<BallController>().GetBallState();
            if (ballState == E_BallState.Attacking) // Attacking
            {
                ballBounce++;
            }
            else if (collisionBallState == E_BallState.Attacking) // Attacked
            {
                TakeDamage(collision.gameObject.GetComponent<BallStat>().currentATK);
            }
        }
        else
        {            
            if (ballState == E_BallState.Attacking)
                wallBounce++;
        }

        if (ballState == E_BallState.Attacking)
        {            
            skill?.ActivateSkill();
        }
    }
    public virtual void ResetStartCondition()
    {
        transform.GetComponent<SpriteRenderer>().color = Color.white;
        transform.GetComponent<Collider2D>().enabled = true;
    }
    public virtual void ResetEndCondition()
    {
        // DO NOTHING
        // Override this function when inherrit this class to child class
    }
    protected void TakeDamage(float damage)
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
    public void IncreaseDamageMultiplier(float multiplier)
    {
        currentATK *= multiplier;
    }
}
