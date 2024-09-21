using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WallReflect : MonoBehaviour
{
    Vector2 calculateReflect(Vector2 a, Vector2 n)
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
            ballRb.velocity = calculateReflect(velocity, -collision.GetContact(0).normal);
        }
    }
}
