using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStat : MonoBehaviour
{
    [SerializeField] protected float MaxHP;
    [SerializeField] protected float ATK;
    [SerializeField] protected float DEF;

    [SerializeField] private HpBar hpBar;
    private float currentHP;

    public void Start()
    {
        currentHP = MaxHP;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
            && collision.gameObject.GetComponent<BallController>().GetBallState() == E_BallState.Attacking)
        {
            TakeDamage(collision.gameObject.GetComponent<BallStat>().ATK);
        }
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

    private void ShowDamage()
    {
        hpBar.ShowHpBar(currentHP / MaxHP);
    }
}
