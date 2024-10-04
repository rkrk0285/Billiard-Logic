using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedCombat : MonoBehaviour
{
    // 캐릭터 관리 리스트
    public List<GameObject> characters;
    private int currentCharacterIndex = 0;

    // 액션 UI 관련
    public GameObject actionPanel;
    public GameObject attackArrow;
    private Vector3 moveDirection;

    // 이동 속도
    public float speed = 5f;

    void Start()
    {
        // 첫 캐릭터 턴 시작
        StartTurn(characters[currentCharacterIndex]);
    }

    void Update()
    {
        // 공격 화살표 조준 업데이트
        if (attackArrow.activeSelf)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - attackArrow.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            attackArrow.transform.rotation = Quaternion.Euler(0, 0, angle);

            // 마우스 클릭 시 캐릭터 이동 시작
            if (Input.GetMouseButtonDown(0))
            {
                OnArrowClick();
            }
        }

        // 현재 캐릭터 이동 처리
        if (moveDirection != Vector3.zero)
        {
            characters[currentCharacterIndex].transform.position += moveDirection * Time.deltaTime;
        }
    }

    // 턴 시작 시 UI 표시
    void StartTurn(GameObject character)
    {
        // 액션 UI를 캐릭터 근처에 표시
        actionPanel.SetActive(true);
        actionPanel.transform.position = character.transform.position + Vector3.up * 2f;
    }

    // 공격 버튼 클릭 시 화살표 표시
    public void OnAttackButton()
    {
        GameObject character = characters[currentCharacterIndex];
        attackArrow.SetActive(true);
        attackArrow.transform.position = character.transform.position;
    }

    // 공격 방향 설정 후 이동 시작
    void OnArrowClick()
    {
        // 화살표 방향 가져오기
        moveDirection = attackArrow.transform.right * speed;

        // 화살표 및 UI 숨기기
        attackArrow.SetActive(false);
        actionPanel.SetActive(false);
    }

    // 충돌 처리
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌 시 방향 반사
        moveDirection = Vector3.Reflect(moveDirection, collision.contacts[0].normal);

        // 적과 충돌 시 데미지 처리
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 적에게 데미지 주기 (구체적 로직 추가 필요)
            Debug.Log("적에게 데미지!");
        }
    }

    // 턴 종료 및 다음 캐릭터로 전환
    void EndTurn()
    {
        // 현재 캐릭터 이동 종료 처리
        moveDirection = Vector3.zero;

        // 다음 캐릭터로 턴 전환
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        StartTurn(characters[currentCharacterIndex]);
    }
}
