using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    private float ballRadius;
    protected virtual void Start()
    {
        ballRadius = transform.localScale.x / 2;
    }    
    protected void DrawLine(Vector2 dir)
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        RaycastHit2D hit = GetCircleCastHit(transform.position, dir, gameObject);

        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, hit.point + hit.normal * ballRadius);

        lr.startColor = Color.white;
        lr.endColor = Color.white;        
        lr.enabled = true;
    }

    protected RaycastHit2D GetCircleCastHit(Vector2 pos, Vector2 dir, GameObject obj)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(pos, ballRadius, dir, 100f);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject != obj)
                return hit[i];
        }
        return new RaycastHit2D();
    }

    protected GameObject GetClosestMonster(GameObject currObj)
    {
        Transform AllyTransform = GameManager.Instance.GetAllyTransform();
        Transform EnemyTransform = GameManager.Instance.GetEnemyTransform();

        GameObject result = null;
        float closestDist = float.MaxValue;
        for (int i = 0; i < AllyTransform.childCount; i++)
        {
            if (AllyTransform.GetChild(i).gameObject != currObj && AllyTransform.GetChild(i).gameObject.activeSelf)
            {
                float dist = Vector2.Distance(currObj.transform.position, AllyTransform.GetChild(i).position);
                if (closestDist > dist)
                {
                    closestDist = dist;
                    result = AllyTransform.GetChild(i).gameObject;
                }
            }
        }

        for (int i = 0; i < EnemyTransform.childCount; i++)
        {
            if (EnemyTransform.GetChild(i).gameObject != currObj && EnemyTransform.GetChild(i).gameObject.activeSelf)
            {
                float dist = Vector2.Distance(currObj.transform.position, EnemyTransform.GetChild(i).position);
                if (closestDist > dist)
                {
                    closestDist = dist;
                    result = EnemyTransform.GetChild(i).gameObject;
                }
            }
        }
        return result;
    }

    protected void DisableLine()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }
    public virtual void Activate() { }
}