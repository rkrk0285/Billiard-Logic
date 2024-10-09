using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public enum E_TileType
    {
        attack,
        attackBuff,
        defenseBuff
        
    }
    public E_TileType currentTileType;
    private float attackTileDamage = 3f;
    private float attackBuffAmount = 3f;
    private float defenseBuffAmount = 1f;

    private MonsterStat monsterStat;
    public void CallCorrespondingTileFunction(GameObject unit)
    {
        if (currentTileType == E_TileType.attack)
        {
            AttackTile(unit);
        }
        else if (currentTileType == E_TileType.attackBuff)
        {
            AttackBuffTile(unit);
        }
        else if(currentTileType == E_TileType.defenseBuff)
        {
            DefenseBuffTile(unit);
        }
    }

    public void AttackTile(GameObject unit)
    {
        monsterStat = unit.gameObject.GetComponent<MonsterStat>();
        monsterStat.TakeDamage(attackTileDamage);
        Debug.Log(unit.gameObject.name + " took damage");
    }
    public void AttackBuffTile(GameObject unit)
    {
        monsterStat = unit.gameObject.GetComponent<MonsterStat>();
        monsterStat.IncreaseDamageAmount(attackBuffAmount);
        Debug.Log(unit.gameObject.name + " took damage buff");
    }
    public void DefenseBuffTile(GameObject unit)
    {
        monsterStat = unit.gameObject.GetComponent<MonsterStat>();
        monsterStat.IncreaseDefenseAmount(defenseBuffAmount);
        Debug.Log(unit.gameObject.name + " took defense buff");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Player")
        {
            CallCorrespondingTileFunction(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Player")
        {
            monsterStat = collision.gameObject.GetComponent<MonsterStat>();
            if (monsterStat.IsAttackBuffedOrDebuffed())
            {
                monsterStat.DecreaseDamageAmount(attackBuffAmount);
                Debug.Log(collision.gameObject.name + " attack buff eliminated");
            }
            else if (monsterStat.IsDefenseBuffedOrDebuffed())
            {
                monsterStat.DecreaseDefenseAmount(defenseBuffAmount);
                Debug.Log(collision.gameObject.name + " defense buff eliminated");
            }
        }
    }

}
