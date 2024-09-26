using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSkill : SkillBase
{
    [SerializeField] private int requiredWallBounceCount = 3;
    [SerializeField] private float criticalMultiplier = 2f;

    public override void ActivateSkill()
    {                
        if (ballStat.GetWallBounceCount() >= requiredWallBounceCount)
        {
            ballStat.IncreaseDamageMultiplier(criticalMultiplier);
            Debug.Log("고블린 액션 발동");
        }
    }
}
