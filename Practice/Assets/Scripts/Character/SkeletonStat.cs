using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStat : MonsterStat
{
    [Header("Skeleton")]
    [SerializeField] private MonsterController monsterController;

    [Space]
    [SerializeField] private int unitID = 0;
    [SerializeField] private int index = 0;    
    [SerializeField] private float followSpeed = 15f;
    [SerializeField] private bool isHead;

    [SerializeField] private GameObject BonePrefab;    

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
    private void OnDestroy()
    {
        InstantiateBone();
        UnitManager.Instance.DelayedUpdateUnit();
    }
    private void InstantiateBone()
    {        
        for(int i = 0; i < 3; i++)
        {
            float randX = Random.Range(-11f, 11f);
            float randY = Random.Range(-6f, 6f);
            GameObject clone = Instantiate(BonePrefab, new Vector2(randX, randY), Quaternion.identity);
            clone.GetComponent<SkeletonBone>().SetSkeletonID(unitID);
        }
    }
    private IEnumerator SkeletonDelayedDown()
    {
        yield return new WaitForFixedUpdate();        
        transform.Find("Sprite").rotation = Quaternion.Euler(0, 0, 90);        
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;                

        StopCoroutine(SkeletonDelayedDown());
    }

    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!isHead)
    //        return;

    //    base.OnTriggerEnter2D(collision);
    //    if (collision.CompareTag("Finish"))
    //    {
    //        //UnitManager.Instance.AddUnit();
    //    }
    //}

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
