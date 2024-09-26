using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : BallStat
{
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
}