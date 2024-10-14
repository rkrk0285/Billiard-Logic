using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{
    public GameObject cannonBallPrefab; // CannonBall 프리팹
    public Transform shootPoint; // 발사 위치
    public float shootInterval = 1f; // 발사 간격 (1초)
    public float cannonBallDamage = 4f; // CannonBall의 데미지

    void Start()
    {
        // 지정된 간격으로 FireCannonBall 메서드를 반복 호출
        InvokeRepeating("FireCannonBall", 0f, shootInterval);
    }

    void FireCannonBall()
    {
        // CannonBall 인스턴스 생성
        GameObject cannonBall = Instantiate(cannonBallPrefab, shootPoint.position, shootPoint.rotation);
        cannonBall.transform.SetParent(transform);
        // 생성된 CannonBall에 데미지 값을 설정
        cannonBall.GetComponent<BoneArrow>().Initialize(cannonBallDamage);

        // 가장 가까운 Enemy 찾기
        Transform target = FindClosestEnemy();
        Vector2 direction;

        if (target != null)
        {
            // 가장 가까운 적의 방향으로 설정
            direction = (target.position - shootPoint.position).normalized;
        }
        else
        {
            // 적이 없으면 기본적으로 발사 위치의 up 방향으로 설정
            direction = shootPoint.up;
        }

        // Rigidbody2D를 이용해 방향으로 속도를 설정하여 발사
        Rigidbody2D rb = cannonBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 10f; // 발사 속도 설정
        }
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(shootPoint.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }
}
