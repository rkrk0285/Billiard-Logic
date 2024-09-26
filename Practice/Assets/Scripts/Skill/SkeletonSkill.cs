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
            Debug.Log("���̷��� �׼� �ߵ�");
            transform.GetComponent<SpriteRenderer>().color = Color.red;
            transform.GetComponent<Collider2D>().enabled = false;
            transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }
}
