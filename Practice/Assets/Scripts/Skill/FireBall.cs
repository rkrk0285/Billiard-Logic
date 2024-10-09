using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float damage = 4f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
        {
            if (collision.gameObject != transform.parent.gameObject)
            {                
                collision.GetComponent<MonsterStat>().TakeDamage(damage);
            }
        }
    }

    public void Initialize(float damage)
    {
        this.damage = damage;
    }
}
