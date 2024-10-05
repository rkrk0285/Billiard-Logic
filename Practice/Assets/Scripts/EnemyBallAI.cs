using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallAI : MonsterController
{
    [SerializeField] private Transform AllyTrans;

    //private int CanHitEnemyCount(Vector2 dir)
    //{
    //    int result = 0;        
    //    Vector2 nextDir = dir;       
    //    Vector2 nextPos = transform.position;

    //    RaycastHit2D firstHit = GetCircleCastHit(nextPos, nextDir, gameObject);
    //    if (firstHit.collider != null)
    //    {
    //        if (firstHit.collider.CompareTag("Player"))
    //            result++;            

    //        nextDir = CalculateNextDirection(gameObject, firstHit.collider.gameObject, firstHit, nextDir);
    //        nextPos = firstHit.point + firstHit.normal * BALLRAD;
    //    }

    //    RaycastHit2D secondHit = GetCircleCastHit(nextPos, nextDir, gameObject, firstHit.collider.gameObject);
    //    if (secondHit.collider != null)
    //    {
    //        if (secondHit.collider.CompareTag("Player"))
    //            result++;            
    //    }
        
    //    return result;        
    //}
    //public void AIShooting()
    //{
    //    int canHitCount = 0;
    //    List<Vector2> canHitDirection = new List<Vector2>();
    //    for (int i = 0; i < 360; i++)
    //    {
    //        float angle = Mathf.Deg2Rad * i;
    //        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

    //        int c = CanHitEnemyCount(dir);
    //        if (c > canHitCount)
    //        {
    //            canHitDirection.Clear();
    //            canHitDirection.Add(dir);
    //            canHitCount = c;
    //        }
    //        else if (c == canHitCount)
    //        {
    //            canHitDirection.Add(dir);
    //        }
    //    }        

    //    for(int i = 0; i < canHitDirection.Count; i++)
    //    {            
    //        Debug.DrawLine(transform.position, transform.position + new Vector3 (canHitDirection[i].normalized.x, canHitDirection[i].normalized.y, 0) * 20);
    //    }
    //    int rand = Random.Range(0, canHitDirection.Count);
    //    MoveMonster(canHitDirection[rand]);
    //}
    public void AIStraightShooting()
    {
        Vector2 finalDir = Vector2.zero;
        float finalDistance = 1000f;
        for(int i = 0; i < AllyTrans.childCount; i++)
        {            
            if (AllyTrans.GetChild(i).gameObject.activeSelf)
            {
                Vector2 posA = AllyTrans.GetChild(i).position;
                Vector2 posB = transform.position;
                Vector2 dir = posA - posB;

                RaycastHit2D hit = GetCircleCastHit(transform.position, dir, gameObject);
                if (hit.collider.CompareTag("Player"))
                {
                    float dist = Vector2.Distance(posA, posB);
                    if (dist < finalDistance)
                    {
                        finalDistance = dist;
                        finalDir = dir;
                    }
                }
            }
        }

        if (finalDir == Vector2.zero)
        {
            int rand = Random.Range(0, 360);
            float angle = Mathf.Deg2Rad * rand;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            MoveMonster(dir);
            Debug.Log("ทฃดý น฿ป็");
        }
        else        
            MoveMonster(finalDir);               
    }
}
