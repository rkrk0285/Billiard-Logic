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
            //else if (firstHit.collider.CompareTag("Enemy"))
            //    result--;

            nextDir = CalculateNextDirection(gameObject, firstHit.collider.gameObject, firstHit, nextDir);
            nextPos = firstHit.point + firstHit.normal * BALLRAD;
        }

        RaycastHit2D secondHit = GetCircleCastHit(nextPos, nextDir, gameObject, firstHit.collider.gameObject);
        if (secondHit.collider != null)
        {
            if (secondHit.collider.CompareTag("Player"))
                result++;
            //else if (secondHit.collider.CompareTag("Enemy"))
            //    result--;
        }
        
        return result;        
    }
    public void AIShooting()
    {
        int canHitCount = 0;
        List<Vector2> canHitDirection = new List<Vector2>();
        for (int i = 0; i < 360; i++)
        {
            float angle = Mathf.Deg2Rad * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            int c = CanHitEnemyCount(dir);
            if (c > canHitCount)
            {
                canHitDirection.Clear();
                canHitDirection.Add(dir);
                canHitCount = c;
            }
            else if (c == canHitCount)
            {
                canHitDirection.Add(dir);
            }
        }        

        for(int i = 0; i < canHitDirection.Count; i++)
        {            
            Debug.DrawLine(transform.position, transform.position + new Vector3 (canHitDirection[i].normalized.x, canHitDirection[i].normalized.y, 0) * 20);
        }
        int rand = Random.Range(0, canHitDirection.Count);
        ShootBall(canHitDirection[rand]);
    }
}
