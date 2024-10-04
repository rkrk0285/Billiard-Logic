using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedCombat : MonoBehaviour
{
    // ĳ���� ���� ����Ʈ
    public List<GameObject> characters;
    private int currentCharacterIndex = 0;

    // �׼� UI ����
    public GameObject actionPanel;
    public GameObject attackArrow;
    private Vector3 moveDirection;

    // �̵� �ӵ�
    public float speed = 5f;

    void Start()
    {
        // ù ĳ���� �� ����
        StartTurn(characters[currentCharacterIndex]);
    }

    void Update()
    {
        // ���� ȭ��ǥ ���� ������Ʈ
        if (attackArrow.activeSelf)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - attackArrow.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            attackArrow.transform.rotation = Quaternion.Euler(0, 0, angle);

            // ���콺 Ŭ�� �� ĳ���� �̵� ����
            if (Input.GetMouseButtonDown(0))
            {
                OnArrowClick();
            }
        }

        // ���� ĳ���� �̵� ó��
        if (moveDirection != Vector3.zero)
        {
            characters[currentCharacterIndex].transform.position += moveDirection * Time.deltaTime;
        }
    }

    // �� ���� �� UI ǥ��
    void StartTurn(GameObject character)
    {
        // �׼� UI�� ĳ���� ��ó�� ǥ��
        actionPanel.SetActive(true);
        actionPanel.transform.position = character.transform.position + Vector3.up * 2f;
    }

    // ���� ��ư Ŭ�� �� ȭ��ǥ ǥ��
    public void OnAttackButton()
    {
        GameObject character = characters[currentCharacterIndex];
        attackArrow.SetActive(true);
        attackArrow.transform.position = character.transform.position;
    }

    // ���� ���� ���� �� �̵� ����
    void OnArrowClick()
    {
        // ȭ��ǥ ���� ��������
        moveDirection = attackArrow.transform.right * speed;

        // ȭ��ǥ �� UI �����
        attackArrow.SetActive(false);
        actionPanel.SetActive(false);
    }

    // �浹 ó��
    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹 �� ���� �ݻ�
        moveDirection = Vector3.Reflect(moveDirection, collision.contacts[0].normal);

        // ���� �浹 �� ������ ó��
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // ������ ������ �ֱ� (��ü�� ���� �߰� �ʿ�)
            Debug.Log("������ ������!");
        }
    }

    // �� ���� �� ���� ĳ���ͷ� ��ȯ
    void EndTurn()
    {
        // ���� ĳ���� �̵� ���� ó��
        moveDirection = Vector3.zero;

        // ���� ĳ���ͷ� �� ��ȯ
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        StartTurn(characters[currentCharacterIndex]);
    }
}
