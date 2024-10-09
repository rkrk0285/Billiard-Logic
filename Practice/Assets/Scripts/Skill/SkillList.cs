using System.Linq;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

partial class SkillList : MonoBehaviour
{
    public static SkillList Instance;
    public bool showRange = true;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public IEnumerator DelayedStopBall(GameObject obj)
    {
        yield return new WaitForFixedUpdate();
        obj.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        StopCoroutine(DelayedStopBall(obj));
    }

    public void RangeAttack(GameObject obj, float range, float damage)
    {
        Vector2 origin = obj.transform.position;
        Collider2D[] ObjectInRange = GetObjectInRange(origin, range);

        for (int i = 0; i < ObjectInRange.Length; i++)
        {
            if (ObjectInRange[i].gameObject != obj)
            {
                Debug.Log(ObjectInRange[i].name);
                ObjectInRange[i].GetComponent<MonsterStat>().TakeDamage(damage);
            }
        }
        DrawCircle(obj, range, Color.red);
    }
    public void RangeAttackWithCount(GameObject obj, float range, float damage, int count, bool ascending, bool enemyAlsoAttack)
    {
        int currCount = 0;
        Vector2 origin = obj.transform.position;
        Collider2D[] ObjectInRange = GetObjectInRange(origin, range, ascending, enemyAlsoAttack);

        for (int i = 0; i < ObjectInRange.Length; i++)
        {
            if (currCount == count)
                break;

            if (ObjectInRange[i].gameObject != obj)
            {
                currCount++;
                ObjectInRange[i].GetComponent<MonsterStat>().TakeDamage(damage);
            }
        }
        DrawCircle(obj, range, Color.red);
    }
    public void RangeHeal(GameObject obj, float range, float heal)
    {
        Vector2 origin = obj.transform.position;
        Collider2D[] ObjectInRange = GetObjectInRange(origin, range);

        for (int i = 0; i < ObjectInRange.Length; i++)
        {
            if (ObjectInRange[i].gameObject != obj)
            {
                Debug.Log(ObjectInRange[i].name);
                ObjectInRange[i].GetComponent<MonsterStat>().TakeHeal(heal);
            }
        }
        DrawCircle(obj, range, Color.green);
    }
    public void RangeHealWithCount(GameObject obj, float range, float heal, int count, bool ascending, bool enemyAlsoAttack)
    {
        int currCount = 0;
        Vector2 origin = obj.transform.position;
        Collider2D[] ObjectInRange = GetObjectInRange(origin, range, ascending, enemyAlsoAttack);

        for (int i = 0; i < ObjectInRange.Length; i++)
        {
            if (currCount == count)
                break;

            if (ObjectInRange[i].gameObject != obj)
            {
                currCount++;
                ObjectInRange[i].GetComponent<MonsterStat>().TakeHeal(heal);
            }
        }
        DrawCircle(obj, range, Color.green);
    }
    private Collider2D[] GetObjectInRange(Vector2 pos, float range, bool ascending = true, bool enemyAlsoAttack = true)
    {
        Collider2D[] result;
        if (enemyAlsoAttack)
            result = Physics2D.OverlapCircleAll(pos, range, LayerMask.GetMask("Monster", "Enemy"));
        else
            result = Physics2D.OverlapCircleAll(pos, range, LayerMask.GetMask("Monster"));

        if (ascending)
        {
            result = result
                .OrderBy(p => Vector2.Distance(pos, p.transform.position))
                .ToArray();
        }
        else
        {
            result = result
                .OrderByDescending(p => Vector2.Distance(pos, p.transform.position))
                .ToArray();
        }
        return result;
    }    
    private void DrawCircle(GameObject obj, float radius, Color color)
    {
        if (!showRange)
            return;

        int segments = 100;
        float angle = 0f;
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            LineRenderer lr = obj.GetComponent<LineRenderer>();
            lr.positionCount = segments;
            lr.startColor = color;
            lr.endColor = color;
            lr.SetPosition(i, obj.transform.position + new Vector3(x, y, 0f));
            lr.enabled = true;
            angle += angleStep;
        }
    }

    public IEnumerator Injection(GameObject obj, float range, float value, int count, bool ascending, bool enemyAlsoAttack)
    {
        int currCount = 0;
        Vector2 origin = obj.transform.position;
        Collider2D[] ObjectInRange = GetObjectInRange(origin, range, ascending, enemyAlsoAttack);

        for (int i = 0; i < ObjectInRange.Length; i++)
        {
            if (currCount == count)
                break;

            if (ObjectInRange[i].gameObject != obj)
            {
                currCount++;
                ObjectInRange[i].GetComponent<MonsterStat>().TakeDamage(value);
                yield return new WaitForSeconds(1);
                if (ObjectInRange[i].gameObject.activeInHierarchy)
                    ObjectInRange[i].GetComponent<MonsterStat>().TakeHeal(value * 2); 
            }
        }
    }
}