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
    private IEnumerator SlowMotionEffect(float slowDuration, float slowFactor)
    {
        // 게임 시간을 느리게 설정
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // 물리 업데이트 시간도 맞춤

        // 주어진 시간동안 기다림
        yield return new WaitForSecondsRealtime(slowDuration);

        // 시간 복구
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; // 물리 업데이트 시간 복구
    }

}
