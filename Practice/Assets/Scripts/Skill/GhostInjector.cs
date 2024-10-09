using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostInjector : SkillBase
{
    [Header("Parameters")]
    [SerializeField] private float range = 4;
    [SerializeField] private float[] healRange;
    [SerializeField] private int count = 1;
    [SerializeField] private bool closeFirst = true;
    [SerializeField] private bool enemyAlsoAttack = false;
    [SerializeField] private GameObject injectorImage;
    public override void Activate()
    {
        base.Activate();
        Debug.Log(this.gameObject.name + " 스킬 발동");
        float heal = Random.Range(healRange[0], healRange[1]);
        GameObject img = Instantiate(injectorImage, transform.position, Quaternion.identity);
        StartCoroutine(SkillList.Instance.Injection(gameObject, range, heal, count, closeFirst, enemyAlsoAttack));
        Destroy(img, 2);
    }
}
