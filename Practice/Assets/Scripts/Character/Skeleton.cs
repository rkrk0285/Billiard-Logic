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
                StartCoroutine(SlowMotionEffect(0.2f, 0.05f)); // 0.2배속으로 0.05초 동안

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