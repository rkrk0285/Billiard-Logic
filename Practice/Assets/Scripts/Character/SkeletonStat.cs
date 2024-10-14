using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStat : MonsterStat
{
    [Header("Skeleton")]
    [SerializeField] private MonsterController monsterController;

    [Space]    
    [SerializeField] private int index = 0;    
    [SerializeField] private float followSpeed = 15f;
    [SerializeField] private bool isHead;

    private Vector2 _targetPos = Vector2.zero;
    private Vector2 _velocity = Vector2.zero;
    public override void ResetStartParameter()
    {        
        transform.Find("Sprite").rotation = Quaternion.Euler(0, 0, 0);
        transform.GetComponent<Collider2D>().enabled = true;
    }
    public override void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Dead();
            return;
        }        
        ShowInfo();
    }

    private IEnumerator SkeletonDelayedDown()
    {
        yield return new WaitForFixedUpdate();        
        transform.Find("Sprite").rotation = Quaternion.Euler(0, 0, 90);        
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;                

        StopCoroutine(SkeletonDelayedDown());
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHead)
            return;

        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Finish"))
        {
            UnitManager.Instance.AddUnit();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<BossScript>().TakeDamage(UnitManager.Instance.GetUnitArmyLength());
        }
    }
    private void Update()
    {
        if (isHead)
            return;
        
        _targetPos = UnitManager.Instance.GetPrevUnitPosition(index);
        transform.position = Vector2.SmoothDamp(transform.position, _targetPos, ref _velocity, 0.4f, followSpeed);
    }
    private void OnDestroy()
    {
        UnitManager.Instance.DelayedUpdateUnit();
    }
    public void SetIndex(int index)
    {
        this.index = index;
    }
    public bool GetIsHead()
    {
        return isHead;
    }
    public void SetHead()
    {
        isHead = true;
    }    
}
