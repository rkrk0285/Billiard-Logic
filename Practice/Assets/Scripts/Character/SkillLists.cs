using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class SkillLists : MonoBehaviour
{
    public static SkillLists Instance;

    [SerializeField] private GameObject goblin;
    [SerializeField] private GameObject golem;
    [SerializeField] private GameObject ghost;
    [SerializeField] private GameObject skeleton;

    //[Header("Goblin To Golem")]
    //[SerializeField] public float GoblinToGolemSpeedMultiplier = 2f;

    //[Header("Golem To Skeleton")]
    //[SerializeField] private int GolemToSkeletonAttackRange = 2;
    //[SerializeField] private float GolemToSkeletonAttackDamage = 2;    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void SkeletonDown()
    {
        if (skeleton != null)
            StartCoroutine(SkeletonDelayedDown());
    }


    public IEnumerator SkeletonDelayedDown()
    {
        yield return new WaitForFixedUpdate();
        skeleton.transform.GetComponent<SpriteRenderer>().color = Color.red;
        skeleton.transform.GetComponent<Collider2D>().enabled = false;
        skeleton.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        StopCoroutine(SkeletonDelayedDown());
    }

    public IEnumerator DelayedStopBall(GameObject obj)
    {
        yield return new WaitForFixedUpdate();
        obj.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        StopCoroutine(DelayedStopBall(obj));
    }

    private void RangeAttack(GameObject obj, int range, float damage, GameObject ignore = null)
    {
        Vector2 origin = obj.transform.position;
        Collider2D[] ObjectInRange = Physics2D.OverlapCircleAll(origin, range, LayerMask.GetMask("Ball"));

        for (int i = 0; i < ObjectInRange.Length; i++)
        {
            if (ObjectInRange[i].gameObject != obj && ObjectInRange[i].gameObject != ignore)
            {
                Debug.Log(ObjectInRange[i].name);
                ObjectInRange[i].GetComponent<BallStat>().TakeDamage(damage);
            }
        }
    }
}

// Interactive Skill Part
partial class SkillLists
{
    //public void GolemToSkeleton()
    //{
    //    DelayedStopBall(golem);        
    //    StartCoroutine(SkeletonDelayedDown());
    //    RangeAttack(golem, GolemToSkeletonAttackRange, GolemToSkeletonAttackDamage, skeleton);
    //}

    //public void GoblinToGolem()
    //{
    //    StartCoroutine(DelayedStopBall(goblin));
    //    StartCoroutine(DelayedStopBall(golem));
    //    goblin.GetComponent<BallStat>().ResetActionParameter();
    //    goblin.GetComponent<BallController>().IncreasePowerMultiplier(GoblinToGolemSpeedMultiplier);
    //    GameManager.Instance.GoToExtraTurn();
    //}

    //public void GhostToSkeleton()
    //{
    //    StartCoroutine(DelayedStopBall(ghost));
    //    ghost.GetComponent<BallStat>().ResetActionParameter();
    //    GameManager.Instance.GoToExtraTurn();
    //}
    //public void GoblinToGolem()
    //{
    //    golem.GetComponent<BallStat>().IncreaseDamage(AttackPowerGivenByGoblin);
    //}
    //public void GoblinToGhost()
    //{
    //    ghost.GetComponent<BallStat>().IncreaseDamage(AttackPowerGivenByGoblin);
    //}
    //public void GoblinToSkeleton()
    //{
    //    skeleton.GetComponent<BallStat>().IncreaseDamage(AttackPowerGivenByGoblin);
    //}
    //public void GolemToGoblin()
    //{
    //    GolemInteractiveSkill(goblin);
    //}
    //public void GolemToGhost()
    //{
    //    GolemInteractiveSkill(ghost);
    //}
    //public void GolemToSkeleton()
    //{        
    //    GolemInteractiveSkill(skeleton);
    //}
    //public void GolemInteractiveSkill(GameObject obj)
    //{
    //    StartCoroutine(DelayedStopBall(golem));
    //    StartCoroutine(DelayedStopBall(obj));
    //    obj.GetComponent<BallStat>().ResetEndParameter();
    //    GameManager.Instance.GoToExtraTurn(obj);
    //}
    //public void GhostToGoblin()
    //{        
    //    ghost.GetComponent<BallStat>().TakeHeal(AdvancedHealPower);
    //    goblin.GetComponent<BallStat>().TakeHeal(AdvancedHealPower);
    //}
    //public void GhostToGolem()
    //{        
    //    ghost.GetComponent<BallStat>().TakeHeal(AdvancedHealPower);
    //    golem.GetComponent<BallStat>().TakeHeal(AdvancedHealPower);
    //}
    //public void GhostToSkeleton()
    //{
    //    ghost.GetComponent<BallStat>().TakeHeal(AdvancedHealPower);
    //    skeleton.GetComponent<BallStat>().TakeHeal(AdvancedHealPower);
    //}
    //public void SkeletonToGoblin()
    //{        
    //    goblin.GetComponent<BallStat>().AddBarrierCount(BarrierGivenBySkeleton);
    //}
    //public void SkeletonToGolem()
    //{
    //    golem.GetComponent<BallStat>().AddBarrierCount(BarrierGivenBySkeleton);
    //}
    //public void SkeletonToGhost()
    //{        
    //    ghost.GetComponent<BallStat>().AddBarrierCount(BarrierGivenBySkeleton);
    //}
}

