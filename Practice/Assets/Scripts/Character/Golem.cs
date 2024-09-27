using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Golem : BallStat
{ 
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        E_BallState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (ballState == E_BallState.Attacking)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
            {
                ballBounce++;
                if (GameManager.Instance.isPossibleToUseInteractive(transform.gameObject.name, collision.gameObject.name))                                    
                    ActiveInteractiveSkill();                
                else
                    collision.gameObject.GetComponent<BallStat>().TakeDamage(currentATK);
            }
            else
            {
                wallBounce++;                
            }
        }
    }

    protected override void InitializeSkill()
    {
        base.InitializeSkill();
        InteractiveSkill.Add("Skeleton", () => { SkillLists.Instance.GolemToSkeleton(); });
    }
}
