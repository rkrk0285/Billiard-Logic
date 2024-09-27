using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BallStat : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] public float MaxHP; // MaxHP를 외부에서 접근 가능하게 수정
    [SerializeField] protected float ATK;
    [SerializeField] protected float DEF;

    private float currentHP;
    public float currentATK;
    public float currentDEF;
    protected int wallBounce;
    protected int ballBounce;

    [Header("Components")]
    [SerializeField] private HpBar hpBar;

    protected SkillBase skill;

    private void Start()
    {
        currentHP = MaxHP;
        ResetActionParameter();

        skill = GetComponent<SkillBase>();
        if (skill != null)
        {
            skill.Initialize(this);
        }
    }
    public void ResetActionParameter()
    {
        currentATK = ATK;
        currentDEF = DEF;
        wallBounce = 0;
        ballBounce = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        E_BallState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            E_BallState collisionBallState = collision.gameObject.GetComponent<BallController>().GetBallState();
            if (ballState == E_BallState.Attacking) // Attacking
            {
                ballBounce++;
            }
            else if (collisionBallState == E_BallState.Attacking) // Attacked
            {
                TakeDamage(collision.gameObject.GetComponent<BallStat>().currentATK);
            }
        }
        else
        {
            if (ballState == E_BallState.Attacking)
                wallBounce++;
        }

        if (ballState == E_BallState.Attacking)
        {
            skill?.ActivateSkill();
        }
    }
    public virtual void ResetStartCondition()
    {

    }
    public virtual void ResetEndCondition()
    {
        // DO NOTHING
        // Override this function when inherrit this class to child class
    }
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            // 캐릭터가 죽었을 경우, 즉시 슬로우 모션을 취소하고 정상 속도로 복귀
            StopAllCoroutines(); // 슬로우 모션 코루틴을 멈춤
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f; // 물리 업데이트 시간 복구
            Destroy(this.gameObject); // 캐릭터 파괴
        }
        else
        {
            // 데미지를 받을 때만 슬로우 모션을 실행
            StartCoroutine(SlowMotionEffect(0.2f, 0.05f)); // 0.2배속으로 0.05초 동안
        }

        ShowDamage();
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

    public void TakeHeal(float heal)
    {
        currentHP += heal;
        if (currentHP >= MaxHP)
        {
            currentHP = MaxHP;
        }
        ShowDamage();
    }
    private void ShowDamage()
    {
        hpBar.ShowHpBar(currentHP / MaxHP);
    }
    public int GetBounceCount()
    {
        return ballBounce + wallBounce;
    }
    public int GetWallBounceCount()
    {
        return wallBounce;
    }
    public int GetBallBounceCount()
    {
        return ballBounce;
    }
    public void IncreaseDamageMultiplier(float multiplier)
    {
        currentATK *= multiplier;
    }
}
