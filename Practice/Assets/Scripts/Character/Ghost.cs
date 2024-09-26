using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Ghost : BallStat
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        E_BallState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {            
            E_BallState collisionBallState = collision.gameObject.GetComponent<BallController>().GetBallState();
            if (ballState == E_BallState.Attacking) // Attacking
            {
                collision.GetComponent<BallStat>().TakeHeal(currentATK);
            }            
        }
        else
        {            
            if (ballState == E_BallState.Attacking)
                wallBounce++;
        }
    }    
    public override void ResetStartCondition()
    {
        base.ResetStartCondition();
        transform.GetComponent<Collider2D>().isTrigger = true;
    }

    public override void ResetEndCondition()
    {
        base.ResetEndCondition();
        transform.GetComponent<Collider2D>().isTrigger = false;
    }
}
