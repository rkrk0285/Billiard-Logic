using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneArrow : MonoBehaviour
{
    private float damage = 4f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.gameObject != transform.parent.gameObject)
            {
                Vector2 dir = collision.gameObject.transform.position - transform.position;
                collision.GetComponent<Rigidbody2D>().AddForce(dir * 1000);
                collision.GetComponent<BossScript>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
    public void Initialize(float damage)
    {
        this.damage = damage;
    }
}
