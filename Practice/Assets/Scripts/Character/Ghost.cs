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
            if (!GameManager.Instance.isExtraTurn)
            {
                if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
                {
                    ballBounce++;
                    if (collision.gameObject.name == InteractiveAllyName)
                    {
                        Interact = true;
                        transform.gameObject.GetComponent<BallController>().StopBall();
                    }      
                    else
                        collision.GetComponent<BallStat>().TakeHeal(currentATK);
                }                                             
            }
            else
            {
                if (collision.CompareTag("Player"))
                {
                    ballBounce++;
                    collision.GetComponent<BallStat>().TakeHeal(currentATK);
                }
                else if (collision.CompareTag("Enemy"))
                {
                    ballBounce++;
                    collision.GetComponent<BallStat>().TakeDamage(currentATK);
                }                
            }
            //skill?.ActivateSkill();
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Do Nothing
        // this object uses OnTriggerEnter2D       
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
        InteractiveSkill.Add("Skeleton", () => { SkillLists.Instance.GhostToSkeleton(); });
    }
}
