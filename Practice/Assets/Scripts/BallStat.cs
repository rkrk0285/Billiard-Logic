using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BallStat : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] protected float MaxHP;
    [SerializeField] protected float ATK;
    [SerializeField] protected float DEF;
    [SerializeField] protected float Heal;
    public GameObject holePrefab; // 구멍 프리팹 참조 변수

    public float currentHP;
    public float currentATK;
    public float currentDEF;
    public float currentHeal;
    public int currentBarrier;
    public bool Interact;

    public string InteractiveAllyName;
    public string InteractiveEnemyName;

    protected int wallBounce;
    protected int ballBounce;

    public bool skipNextTurn;
    [Header("Components")]
    [SerializeField] private InfoUI infoUI;

    protected SkillBase skill;
    protected Dictionary<string, Action> InteractiveSkill = new Dictionary<string, Action>();

    private void Start()
    {
        currentHP = MaxHP;
        currentBarrier = 0;
        ResetEndParameter();
        InitializeSkill();
    }
    public virtual void ResetEndParameter()
    {
        currentATK = ATK;
        currentDEF = DEF;
        currentHeal = Heal;
        wallBounce = 0;
        ballBounce = 0;

        Interact = false;
        InteractiveAllyName = null;
        InteractiveEnemyName = null;
        skipNextTurn = false;
        ShowInfo();
    }
    public void ActiveInteractiveSkill()
    {
        if (InteractiveSkill.ContainsKey(InteractiveAllyName))
        {
            InteractiveSkill[InteractiveAllyName]?.Invoke();
        }
    }
    public void HandsUp()
    {
        transform.Find("HandsUp").gameObject.SetActive(true);
    }
    public void HandsDown()
    {
        transform.Find("HandsUp").gameObject.SetActive(false);
    }
    public void TakeDamage(float damage)
    {
        if (currentBarrier > 0)
        {
            currentBarrier--;
        }
        else
        {
            currentHP -= damage;
            if (currentHP <= 0)
            {
                Dead();
            }
        }
        ShowInfo();
    }
    public void TakeHeal(float heal)
    {
        currentHP += heal;
        if (currentHP >= MaxHP)
        {
            currentHP = MaxHP;
        }
        ShowInfo();
    }

    public void Dead()
    {
        // 구멍 프리팹이 있는지 확인
        if (holePrefab != null)
        {
            // 캐릭터가 죽은 자리에 구멍을 생성
            Instantiate(holePrefab, transform.position, Quaternion.identity);
        }

        // 캐릭터 비활성화
        gameObject.SetActive(false);
    }
    private void ShowInfo()
    {
        infoUI.ShowHpBar(currentHP / MaxHP);
        infoUI.ShowBarrier(currentBarrier);
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
    public void SetInteractiveAllyName(string name)
    {
        InteractiveAllyName = name;
    }
    public void SetInteractiveEnemyName(string name)
    {
        InteractiveEnemyName = name;
    }
    public void AddBarrierCount(int amount)
    {
        currentBarrier += amount;
        ShowInfo();
    }
    public void IncreaseDamageMultiplier(float multiplier)
    {
        currentATK *= multiplier;
    }
    public void IncreaseDamage(float amount)
    {
        currentATK += amount;
        ShowInfo();
    }
    protected virtual void InitializeSkill()
    {
        skill = GetComponent<SkillBase>();
        if (skill != null)
        {
            skill.Initialize(this);
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // For Enemy.
        E_BallState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (ballState == E_BallState.Attacking)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
            {
                ballBounce++;
                collision.gameObject.GetComponent<BallStat>().TakeDamage(currentATK);
            }
            else
            {
                wallBounce++;
            }
        }
    }
    public void SkipNextTurn()
    {
        skipNextTurn = true;
    }
    public virtual void ResetStartCondition()
    {
        if (skipNextTurn)
            GameManager.Instance.GoToNextTurn();
    }
    public virtual void ResetEndCondition()
    {
        // DO NOTHING
        // Override this function when inherrit this class to child class
    }

    //protected IEnumerator SlowMotionEffect(float slowDuration, float slowFactor)
    //{
    //    // ���� �ð��� ������ ����
    //    Time.timeScale = slowFactor;
    //    Time.fixedDeltaTime = 0.02f * Time.timeScale; // ���� ������Ʈ �ð��� ����

    //    // �־��� �ð����� ��ٸ�
    //    yield return new WaitForSecondsRealtime(slowDuration);

    //    // �ð� ����
    //    Time.timeScale = 1f;
    //    Time.fixedDeltaTime = 0.02f; // ���� ������Ʈ �ð� ����
    //}
}