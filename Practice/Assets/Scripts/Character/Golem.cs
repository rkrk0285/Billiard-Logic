using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Golem : BallStat
{
    [SerializeField] private float IncreaseMass = 2f;
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        E_BallState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (ballState == E_BallState.Attacking)
        {
            if (collision.gameObject.CompareTag("Enemy"))
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
            else if (collision.gameObject.CompareTag("Player"))
            {
                ballBounce++;
                collision.gameObject.GetComponent<BallController>().IncreaseMassMultiplier(IncreaseMass);
            }
            else
                wallBounce++;
        }
    }

    protected override void InitializeSkill()
    {
        base.InitializeSkill();
        InteractiveSkill.Add("Goblin", () => { SkillLists.Instance.GolemToGoblin(); });
        InteractiveSkill.Add("Ghost", () => { SkillLists.Instance.GolemToGhost(); });
        InteractiveSkill.Add("Skeleton", () => { SkillLists.Instance.GolemToSkeleton(); });
    }
}
