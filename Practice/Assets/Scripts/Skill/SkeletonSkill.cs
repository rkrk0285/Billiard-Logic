using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSkill : SkillBase
{
    [SerializeField] private int requiredBounceCount = 3;

    public override void ActivateSkill()
    {
        if (ballStat.GetBounceCount() >= requiredBounceCount)
        {
            Debug.Log("½ºÄÌ·¹Åæ ¾×¼Ç ¹ßµ¿");
            StartCoroutine(DelayedProcess());
        }
    }

    public IEnumerator DelayedProcess()
    {
        yield return new WaitForFixedUpdate();
        transform.GetComponent<SpriteRenderer>().color = Color.red;
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        StopCoroutine(DelayedProcess());
    }
}
