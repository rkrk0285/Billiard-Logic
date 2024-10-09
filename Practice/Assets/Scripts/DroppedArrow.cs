using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedArrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShootArrowSkill shootArrowSkill = collision.GetComponent<ShootArrowSkill>();

        if (shootArrowSkill != null)
        {
            shootArrowSkill.PickUpArrow(); // 스켈레톤이 화살을 다시 주움
            Destroy(gameObject); // 바닥에 떨어진 화살 제거
        }
    }
}
