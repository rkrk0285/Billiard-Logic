using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField] GameObject[] skills;
    [SerializeField] GameObject player;
    [SerializeField] float skillDelay = 2;
    [SerializeField] float skilltime = 1;
    
    [Space]
    public float currentHP = 100;

    [Header("Components")]
    [SerializeField] private InfoUI infoUI;
    
    void Start()
    {
        StartCoroutine(UseSkill());
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
        player = UnitManager.Instance.GetUnitHead();
    }

    // �÷��̾ �ٶ󺸴� �Լ�
    void LookAtPlayer()
    {
        if (player == null)
            return;

        // �÷��̾�� ������ ��ġ ���̸� ���
        Vector3 direction = transform.position - player.transform.position;
        // Z�� ȸ������ ��� (2D ȯ��)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // ȸ�� ������ ����, �⺻������ �Ʒ�(-90��)�� ���� �����Ƿ� �߰��� -90�� ȸ��
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    IEnumerator UseSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillDelay);
            int index = Random.Range(0, skills.Length);
            skills[index].SetActive(true);
            yield return new WaitForSeconds(skilltime);
            skills[index].SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {        
        currentHP -= damage;
        if (currentHP <= 0)
            Dead();
        ShowInfo();
    }

    protected void ShowInfo()
    {
        infoUI.ShowHpBar(currentHP / 100);
    }
    protected void Dead()
    {
        gameObject.SetActive(false);
    }
}
