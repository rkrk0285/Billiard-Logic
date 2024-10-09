using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSkill : SkillBase
{
    [Header("Parameters")]
    [SerializeField] private float range = 3;
    [SerializeField] private float damage = 2;

    [SerializeField] private GameObject poisonPrefab;

    private GameObject currentPoison;
    protected override void Start()
    {
        currentPoison = null;
    }
    public override void Activate()
    {
        base.Activate();
        Debug.Log(this.gameObject.name + " 스킬 발동");
        if (currentPoison != null)
            Destroy(currentPoison);

        currentPoison = Instantiate(poisonPrefab, transform.position, Quaternion.identity);
        currentPoison.GetComponent<Poison>().Initialize(range, damage, this.gameObject);
    }
}
