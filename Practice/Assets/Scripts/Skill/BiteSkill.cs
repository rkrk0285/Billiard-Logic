using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteSkill : SkillBase
{
    [SerializeField] private float damage = 4f;
    public override void Activate()
    {
        base.Activate();
        Debug.Log(this.gameObject.name + " 스킬 발동");
        
        GameObject target = GetClosestMonster(this.gameObject);
        if (target != null)
        {
            target.GetComponent<MonsterStat>().TakeDamage(damage);
            Vector3 dir = target.transform.position - transform.position;
            transform.position = target.transform.position - dir.normalized;
        }
    }
}
