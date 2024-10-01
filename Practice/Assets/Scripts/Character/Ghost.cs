using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Ghost : BallStat
{    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        E_BallState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (ballState == E_BallState.Attacking)
        {
            if (collision.CompareTag("Player"))
            {
                ballBounce++;
                if (collision.gameObject.name == InteractiveAllyName)
                {
                    ActiveInteractiveSkill();
                    InteractiveAllyName = null;
                }
                else
                {                    
                    collision.GetComponent<BallStat>().TakeHeal(currentHeal);
                }
            }
            //skill?.ActivateSkill();
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Do Nothing.
        // This object uses OnTriggerEnter2D       
    }
    public override void ResetStartCondition()
    {
        base.ResetStartCondition();
        transform.GetComponent<Collider2D>().isTrigger = true;
    }

    public override void ResetEndCondition()
    {
        base.ResetEndCondition();
        transform.GetComponent<Collider2D>().isTrigger = false;
    }

    protected override void InitializeSkill()
    {
        base.InitializeSkill();
        InteractiveSkill.Add("Goblin", () => { SkillLists.Instance.GhostToGoblin(); });
        InteractiveSkill.Add("Golem", () => { SkillLists.Instance.GhostToGolem(); });
        InteractiveSkill.Add("Skeleton", () => { SkillLists.Instance.GhostToSkeleton(); });
    }
}
