using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    protected BallStat ballStat;

    public virtual void Initialize(BallStat ballStat)
    {
        this.ballStat = ballStat;
    }

    public abstract void ActivateSkill();
}
