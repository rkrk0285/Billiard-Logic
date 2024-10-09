using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCannonSkill : SkillBase
{
    [SerializeField] private GameObject CannonBall;
    [SerializeField] private GameObject recoveryCirclePrefab; // ������ ȸ���� ���׶�� ������
    [SerializeField] private int maxCannonBalls = 3; // �ִ� ������ ��
    private int currentCannonBalls;                 // ���� ���� ������ ��

    private bool activeSkill = false;

    protected override void Start()
    {
        currentCannonBalls = maxCannonBalls;  // ������ �� �ʱ�ȭ
    }

    private void Update()
    {
        if (activeSkill)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = worldPos - new Vector2(transform.position.x, transform.position.y);

            if (Input.GetMouseButtonDown(0))
            {
                activeSkill = false;
                ShootCannon(dir);
            }
            else
                DrawLine(dir);
        }
    }

    public override void Activate()
    {
        base.Activate();
        Debug.Log(this.gameObject.name + " ��ų �ߵ�");
        activeSkill = true;
    }

    private void ShootCannon(Vector2 dir)
    {
        // �������� �������� ������ �߻� �Ұ�
        if (currentCannonBalls <= 0)
        {
            Debug.Log("�������� �����մϴ�!");
            return;
        }

        GameObject clone = Instantiate(CannonBall, transform.position, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 2000);

        currentCannonBalls--;  // ������ �� ����
        Debug.Log("���� ������ ��: " + currentCannonBalls);

        // �������� �ı��Ǳ� ������ ��� ȸ���� ���׶�� ����
        CreateRecoveryCircle(clone.transform.position);
    }

    // ���׶�̸� ��� �����ϴ� �Լ�
    private void CreateRecoveryCircle(Vector3 position)
    {
        // �������� �ı��� ��ġ�� ��� ���׶�� ����
        Debug.Log("���׶�� ������! ��ġ: " + position);
        GameObject recoveryCircle = Instantiate(recoveryCirclePrefab, position, Quaternion.identity);

        // ���׶�̴� Skeleton�� �浹���� ���� �����
    }

    // ������ ȸ�� �Լ�
    public void RestoreCannonBall()
    {
        if (currentCannonBalls < maxCannonBalls)
        {
            currentCannonBalls++;
            Debug.Log("�������� ȸ���߽��ϴ�! ���� ������ ��: " + currentCannonBalls);
        }
    }
}
