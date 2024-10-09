using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryCircle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Skeleton 태그가 있는 오브젝트와 충돌하면 대포알을 회복
        if (collision.CompareTag("Skeleton"))
        {
            FindObjectOfType<ShootCannonSkill>().RestoreCannonBall();
            Destroy(gameObject); // 회복 후 동그라미 파괴
        }
    }
}
