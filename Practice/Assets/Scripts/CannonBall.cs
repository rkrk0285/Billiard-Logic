using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Player") || collision.CompareTag("Skeleton"))
            {
            if (collision.gameObject != transform.parent.gameObject)
            {
                Vector2 dir = collision.gameObject.transform.position - transform.position;
                collision.GetComponent<Rigidbody2D>().AddForce(dir * 1000);
                collision.GetComponent<MonsterStat>().TakeDamage(4);
                Destroy(this.gameObject);
            } 
        }
    }
}
