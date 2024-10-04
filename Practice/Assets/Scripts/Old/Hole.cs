using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private float speedThreshold = 0.5f;  // 공이 죽을 속도 임계값    
    [SerializeField] private float damage = 20f;

    private void OnTriggerStay2D(Collider2D other)
    {
        // 공이 구멍 안에 들어와 있는 동안
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 공의 속도가 특정 임계치보다 낮은 경우
                if (rb.velocity.magnitude <= speedThreshold)
                {
                    // 일정 시간 동안 속도가 낮다면 공을 죽임
                    other.GetComponent<BallStat>().TakeDamage(damage);                    
                }
            }
        }
    }
}
