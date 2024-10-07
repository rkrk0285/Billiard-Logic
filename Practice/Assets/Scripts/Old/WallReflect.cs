using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WallReflect : MonoBehaviour
{
    public float ReduceSpeed;

    Vector2 CalculateReflect(Vector2 a, Vector2 n)
    {
        Vector2 p = -Vector2.Dot(a, n) / n.magnitude * n / n.magnitude;
        Vector2 b = a + 2 * p;        
        return b;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D monsterRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 velocity = collision.gameObject.GetComponent<MonsterController>().GetCurrVelocity();
            monsterRb.velocity = CalculateReflect(velocity, -collision.GetContact(0).normal) * ReduceSpeed;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Rigidbody2D monsterRb = collision.GetComponent<Rigidbody2D>();
    //        if (monsterRb != null)
    //        {
    //            Vector2 velocity = monsterRb.velocity;
    //            Vector2 ballCenter = collision.bounds.center;
    //            Vector2 closestPoint = GetClosestPoint(collision);
    //            Vector2 collisionNormal = (ballCenter - closestPoint).normalized;
    //            Vector2 reflectedVelocity = CalculateReflect(velocity, collisionNormal) * ReduceSpeed;                
    //            monsterRb.velocity = reflectedVelocity;
    //        }
    //    }
    //}
    
    private Vector2 GetClosestPoint(Collider2D ballCollider)
    {
        Collider2D wallCollider = GetComponent<Collider2D>();
        return wallCollider.ClosestPoint(ballCollider.bounds.center);
    }
}
