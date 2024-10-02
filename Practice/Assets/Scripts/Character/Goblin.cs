using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Goblin : BallStat
{    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        E_BallState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (ballState == E_BallState.Attacking)
        {
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
            {
                ballBounce++;
                if (collision.gameObject.name == InteractiveAllyName)
                {
                    ActiveInteractiveSkill();
                    InteractiveAllyName = null;
                }
                else
                    collision.gameObject.GetComponent<BallStat>().TakeDamage(currentATK);
            }            
            else
            {
                wallBounce++;
                skill?.ActivateSkill();
            }
        }
    }

    protected override void InitializeSkill()
    {
        base.InitializeSkill();
        //InteractiveSkill.Add("Golem", () => { SkillLists.Instance.GoblinToGolem(); });
        //InteractiveSkill.Add("Ghost", () => { SkillLists.Instance.GoblinToGhost(); });
        //InteractiveSkill.Add("Skeleton", () => { SkillLists.Instance.GoblinToSkeleton(); });
    }    
}