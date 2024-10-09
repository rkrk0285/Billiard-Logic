using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] private GameObject droppedArrowPrefab; // 바닥에 떨어진 화살 프리팹
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
        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
        {
            DealDamage(collision.gameObject);

            // 바닥에 떨어진 화살을 생성
            Instantiate(droppedArrowPrefab, transform.position, Quaternion.identity, GameObject.Find("Objects").transform);

            Destroy(this.gameObject); // 현재 화살 제거
        }
    }

    private void DealDamage(GameObject target)
    {
        var monsterStat = target.GetComponent<MonsterStat>();
        if (monsterStat != null)
        {
            monsterStat.TakeDamage(4);
            Debug.Log("화살이 대상을 맞췄습니다!");
        }
        else
        {
            Debug.LogWarning("MonsterStat 컴포넌트를 찾을 수 없습니다.");
        }
    }
}
