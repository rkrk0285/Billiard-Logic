using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //Damage Function
            collision.gameObject.GetComponent<SkeletonStat>().TakeDamage(100);
        }
    }
}
