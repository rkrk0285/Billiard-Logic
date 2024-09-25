using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallAI : BallController
{
    public int CanHitEnemyCount(Vector2 dir)
    {                
        RaycastHit2D firstHit = GetCircleCastHit(new Vector2(transform.position.x, transform.position.y), dir, gameObject);
        if (firstHit.collider != null && firstHit.collider.CompareTag("Player"))
        {
            Vector2 n = firstHit.normal;

            float m1 = rb.mass;
            float m2 = firstHit.collider.gameObject.GetComponent<Rigidbody2D>().mass;
            Vector2 v1 = dir;
            Vector2 v2 = Vector2.zero;
            Vector2 p1 = firstHit.point + n * BALLRAD;
            Vector2 p2 = firstHit.point - n * BALLRAD;

            Vector2 basisX = (p2 - p1).normalized;
            Vector2 basisY = Vector2.Perpendicular(basisX);
            float sin1, sin2, cos1, cos2;

            if (v1.magnitude < 0.001f)
            {
                sin1 = 0;
                cos1 = 1;
            }
            else
            {
                cos1 = Vector2.Dot(v1, basisX) / v1.magnitude;
                Vector3 cross = Vector3.Cross(v1, basisX);

                if (cross.z > 0)
                    sin1 = cross.magnitude / v1.magnitude;
                else
                    sin1 = -cross.magnitude / v1.magnitude;
            }

            if (v2.magnitude < 0.0001f)
            {
                sin2 = 0;
                cos2 = 1;
            }
            else
            {
                cos2 = Vector2.Dot(v2, basisX) / v2.magnitude;
                Vector3 cross = Vector3.Cross(v2, basisX);
                if (cross.z > 0)
                    sin2 = cross.magnitude / v2.magnitude;
                else
                    sin2 = -cross.magnitude / v2.magnitude;
            }

            Vector2 u1, u2;
            u1 = ((m1 - epsilon * m2) * v1.magnitude * cos1 + m2 * (1 + epsilon) * v2.magnitude * cos2) / (m1 + m2) * basisX - v1.magnitude * sin1 * basisY;
            u2 = (m1 * (1 + epsilon) * v1.magnitude * cos1 + (m2 - epsilon * m1) * v2.magnitude * cos2) / (m1 + m2) * basisX - v2.magnitude * sin2 * basisY;

            RaycastHit2D secondHit = GetCircleCastHit(firstHit.point + n * BALLRAD, u1, gameObject, firstHit.collider.gameObject);
            if (secondHit.collider != null && secondHit.collider.CompareTag("Player"))            
                return 2;            
            else            
                return 1;                     
        }
        return 0;
    }
    public void AIShooting()
    {
        int canHitCount = 0;
        List<Vector2> canHitDirection = new List<Vector2>();
        for (int i = 0; i < 180; i++)
        {
            Vector2 dir1 = new Vector2(1, Mathf.Tan(Mathf.Deg2Rad * i));
            Vector2 dir2 = new Vector2(-1, Mathf.Tan(Mathf.Deg2Rad * i));

            int c1 = CanHitEnemyCount(dir1);
            int c2 = CanHitEnemyCount(dir2);
            
            if (c1 > canHitCount)
            {
                canHitDirection.Clear();
                canHitDirection.Add(dir1);
                canHitCount = c1;
            }
            else if (c1 == canHitCount)
            {
                canHitDirection.Add(dir1);
            }

            if (c2 > canHitCount)
            {
                canHitDirection.Clear();
                canHitDirection.Add(dir2);
                canHitCount = c2;
            }
            else if (c2 == canHitCount)
            {
                canHitDirection.Add(dir2);
            }
        }        

        int rand = Random.Range(0, canHitDirection.Count);
        ShootBall(canHitDirection[rand]);
    }
}
