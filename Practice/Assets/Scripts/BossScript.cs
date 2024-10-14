using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField] GameObject[] skills;
    [SerializeField] GameObject player;
    [SerializeField] float skillDelay = 2;
    [SerializeField] float skilltime = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UseSkill());
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
    }

    // �÷��̾ �ٶ󺸴� �Լ�
    void LookAtPlayer()
    {
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
}
