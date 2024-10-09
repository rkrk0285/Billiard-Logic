using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcStat : MonsterStat
{
    [SerializeField] protected MonsterController monsterController;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        E_MonsterState monsterState = transform.gameObject.GetComponent<MonsterController>().GetMonsterState();
        if (monsterState == E_MonsterState.Moving)
        {
            if (collision.gameObject.CompareTag("Wall"))
                CallWhenOrcHitWall();
        }
    }

    private void CallWhenOrcHitWall()
    {        
        StartCoroutine(monsterController.DelayedStopBall());
        SkillList.Instance.RangeAttack(gameObject, 1.5f, 3);
    }    
}
