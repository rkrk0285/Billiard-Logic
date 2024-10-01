using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : BallStat
{
    [SerializeField] private float IncreasePowerAmount = 2f;
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
                    ActiveInteractiveSkill();
                    InteractiveAllyName = null;
                }
                else
                    collision.gameObject.GetComponent<BallStat>().TakeDamage(currentATK);
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                ballBounce++;
                collision.gameObject.GetComponent<BallController>().IncreasePowerMultiplier(IncreasePowerAmount);
            }
            else
                wallBounce++;                            

            skill?.ActivateSkill();
        }
    }

    public override void ResetStartCondition()
    {
        base.ResetStartCondition();
        transform.GetComponent<SpriteRenderer>().color = Color.white;
        transform.GetComponent<Collider2D>().enabled = true;
    }

    public override void ResetEndCondition()
    {
        base.ResetEndCondition();               
    }

    protected override void InitializeSkill()
    {
        base.InitializeSkill();
        InteractiveSkill.Add("Goblin", () => { SkillLists.Instance.SkeletonToGoblin(); });
        InteractiveSkill.Add("Golem", () => { SkillLists.Instance.SkeletonToGolem(); });
        InteractiveSkill.Add("Ghost", () => { SkillLists.Instance.SkeletonToGhost(); });        
    }
}