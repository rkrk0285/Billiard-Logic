using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallAI : BallController
{
    private int CanHitEnemyCount(Vector2 dir)
    {
        int result = 0;        
        Vector2 nextDir = dir;       
        Vector2 nextPos = transform.position;

        RaycastHit2D firstHit = GetCircleCastHit(nextPos, nextDir, gameObject);
        if (firstHit.collider != null)
        {
            if (firstHit.collider.CompareTag("Player"))
                result++;

            nextDir = CalculateNextDirection(gameObject, firstHit.collider.gameObject, firstHit, nextDir);
            nextPos = firstHit.point + firstHit.normal * BALLRAD;
        }

        RaycastHit2D secondHit = GetCircleCastHit(nextPos, nextDir, gameObject, firstHit.collider.gameObject);
        if (secondHit.collider != null)
        {
            if (secondHit.collider.CompareTag("Player"))
                result++;
        }
        
        return result;        
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
