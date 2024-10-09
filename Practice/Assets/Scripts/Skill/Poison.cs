using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    private float damage = 0;
    private GameObject IgnoreObject;
    private List<GameObject> triggeredObj;

    private void Start()
    {
        triggeredObj = new List<GameObject>();
        GameManager.Instance.TurnEndEvent += () => DamagedByPoision();
    }
    private void OnDestroy()
    {
        GameManager.Instance.TurnEndEvent -= () => DamagedByPoision();
    }
    public void Initialize(float range, float damage, GameObject ignore)
    {        
        this.damage = damage;
        IgnoreObject = ignore;
        transform.localScale = new Vector3(range, range, range);        
    }    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
        {
            if (collision.gameObject != IgnoreObject)
                triggeredObj.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
        {
            if (collision.gameObject != IgnoreObject && triggeredObj.Contains(collision.gameObject))
                triggeredObj.Remove(collision.gameObject);
        }
    }
    private void DamagedByPoision()
    {
        for (int i = 0; i < triggeredObj.Count; i++)
        {
            triggeredObj[i].GetComponent<MonsterStat>().TakeDamage(damage);
        }
    }    
}
