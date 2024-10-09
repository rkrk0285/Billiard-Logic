using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStat : MonsterStat
{
    [SerializeField] private MonsterController monsterController;
    [SerializeField] private float knockDownDamage = 3;

    private const int knockDownTurn = 1;
    public override void ResetStartParameter()
    {        
        transform.Find("Sprite").rotation = Quaternion.Euler(0, 0, 0);
        transform.GetComponent<Collider2D>().enabled = true;
    }
    public override void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            Dead();

        if (damage >= knockDownDamage)
            StartCoroutine(SkeletonDelayedDown());
        ShowInfo();
    }

    private IEnumerator SkeletonDelayedDown()
    {
        yield return new WaitForFixedUpdate();        
        transform.Find("Sprite").rotation = Quaternion.Euler(0, 0, 90);        
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        
        monsterController.SetSkipTurn(knockDownTurn);

        StopCoroutine(SkeletonDelayedDown());
    }
}
