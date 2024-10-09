using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private Vector2 lastVelocity;
    private Rigidbody2D rb;
    private bool hasBounced = false; // 벽에 한 번 부딪힌 후만 데미지 적용

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 중력 영향 제거
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        lastVelocity = rb.velocity;
    }

    void Update()
    {
        lastVelocity = rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy") && collision.CompareTag("Player"))
        {
            DealDamage(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void DealDamage(GameObject enemy)
    {
        var monsterStat = enemy.GetComponent<MonsterStat>();
        if (monsterStat != null)
        {
            monsterStat.TakeDamage(4);
            Debug.Log("화살이 크리쳐를 맞췄습니다!");
        }
        else
        {
            Debug.LogWarning("MonsterStat 컴포넌트를 찾을 수 없습니다.");
        }
    }
}
