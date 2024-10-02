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
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 velocity = collision.gameObject.GetComponent<BallController>().GetCurrVelocity();
            ballRb.velocity = CalculateReflect(velocity, -collision.GetContact(0).normal) * ReduceSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D ballRb = collision.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                Vector2 velocity = ballRb.velocity;
                Vector2 ballCenter = collision.bounds.center;
                Vector2 closestPoint = GetClosestPoint(collision);
                Vector2 collisionNormal = (ballCenter - closestPoint).normalized;
                Vector2 reflectedVelocity = CalculateReflect(velocity, collisionNormal) * ReduceSpeed;                
                ballRb.velocity = reflectedVelocity;
            }
            //Rigidbody2D ballRb = collision.GetComponent<Rigidbody2D>();
            //if (ballRb != null)
            //{
            //    Vector2 velocity = ballRb.velocity;
            //    Vector2 ballCenter = collision.bounds.center;
            //    Vector2 RayPosition = ballCenter - velocity.normalized * 1;
            //    // 공에서 벽 방향으로 레이캐스트하여 충돌 지점과 노멀을 추정
            //    RaycastHit2D hit = Physics2D.Raycast(RayPosition, velocity.normalized, 3f, LayerMask.GetMask("Default"));

            //    if (hit.collider != null)
            //    {
            //        // 레이캐스트를 통해 얻은 충돌 지점과 노멀을 사용
            //        Vector2 collisionNormal = hit.normal;                    
            //        Vector2 reflectedVelocity = CalculateReflect(velocity, collisionNormal) * ReduceSpeed;
                                        
            //        ballRb.velocity = reflectedVelocity;
            //        Debug.Log("collisionNormal : " + collisionNormal + "\nreflected : " + reflectedVelocity);
            //    }
            //    else
            //    {
            //        Debug.LogWarning("Raycast hit nothing, check setup or collider.");
            //    }
            //}
        }
    }
    
    private Vector2 GetClosestPoint(Collider2D ballCollider)
    {
        Collider2D wallCollider = GetComponent<Collider2D>();
        return wallCollider.ClosestPoint(ballCollider.bounds.center);
    }
}
