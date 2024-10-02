using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private float speedThreshold = 0.5f;  // 공이 죽을 속도 임계값
    [SerializeField] private float stayDuration = 0.1f;      // 공이 구멍에 있을 때 죽을 때까지 기다리는 시간

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
                    StartCoroutine(CheckForDeath(other.gameObject));
                }
            }
        }
    }

    private IEnumerator CheckForDeath(GameObject ball)
    {
        // 일정 시간 기다림
        yield return new WaitForSeconds(stayDuration);

        // ball이 파괴되었는지 확인
        if (ball == null)
        {
            yield break; // 오브젝트가 사라졌으면 코루틴 종료
        }

        // 구멍 안에서 공이 여전히 느린 속도면 공을 제거
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null && rb.velocity.magnitude <= speedThreshold)
        {
            // 공을 죽이는 처리 (BallStat 스크립트의 TakeDamage 메서드를 사용해도 됨)
            BallStat ballStat = ball.GetComponent<BallStat>();
            if (ballStat != null)
            {
                ballStat.TakeDamage(damage); // MaxHP 사용
                Debug.Log("구멍!dp QKwls");
            }
            else
            {
                Destroy(ball); // BallStat이 없으면 바로 파괴
                Debug.Log("구멍!");
            }
        }
    }

}
