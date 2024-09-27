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
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
            {
                ballBounce++;
                if (collision.gameObject.name == InteractiveAllyName)
                {
                    Interact = true;
                    transform.gameObject.GetComponent<BallController>().StopBall();
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
        InteractiveSkill.Add("Golem", () => { SkillLists.Instance.GoblinToGolem(); });
    }
}
