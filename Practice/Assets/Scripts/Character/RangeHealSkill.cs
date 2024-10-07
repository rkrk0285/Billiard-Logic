using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeHealSkill : SkillBase
{
    [Header("Parameters")]
    [SerializeField] private float range = 4;
    [SerializeField] private float heal = 2;
    [SerializeField] private int count = 2;
    [SerializeField] private bool closeFirst = true;
    [SerializeField] private bool enemyAlsoAttack = false;
    public override void Activate()
    {
        base.Activate();
        Debug.Log(this.gameObject.name + " 스킬 발동");
        SkillList.Instance.RangeHealWithCount(gameObject, range, heal, count, closeFirst, enemyAlsoAttack);
    }
}
