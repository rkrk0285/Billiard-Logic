using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Old : BallStat
{    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        E_MonsterState ballState = transform.gameObject.GetComponent<BallController>().GetBallState();
        if (ballState == E_MonsterState.Moving)
        {
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
            {
                ballBounce++;
                if (collision.gameObject == InstructionEnemy)
                {                    
                    InstructionAlly = null;
                    InstructionEnemy = null;
                    collision.gameObject.GetComponent<BallStat>().TakeDamage(currentATK * _InstructionDamageMultiplier);
                }
                else
                    collision.gameObject.GetComponent<BallStat>().TakeDamage(currentATK);
            }            
            else
            {
                wallBounce++;                
            }
        }
    }
}