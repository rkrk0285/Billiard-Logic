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

    // 플레이어를 바라보는 함수
    void LookAtPlayer()
    {
        if (player == null)
            return;

        // 플레이어와 보스의 위치 차이를 계산
        Vector3 direction = transform.position - player.transform.position;
        // Z축 회전각을 계산 (2D 환경)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // 회전 각도를 설정, 기본적으로 아래(-90도)를 보고 있으므로 추가로 -90도 회전
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
