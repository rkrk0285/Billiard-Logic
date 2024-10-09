using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryCircle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Skeleton �±װ� �ִ� ������Ʈ�� �浹�ϸ� �������� ȸ��
        if (collision.CompareTag("Skeleton"))
        {
            FindObjectOfType<ShootCannonSkill>().RestoreCannonBall();
            Destroy(gameObject); // ȸ�� �� ���׶�� �ı�
        }
    }
}
