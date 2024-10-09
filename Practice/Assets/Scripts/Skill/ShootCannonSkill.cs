using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCannonSkill : SkillBase
{
    [SerializeField] private GameObject CannonBall;
    [SerializeField] private GameObject recoveryCirclePrefab; // 대포알 회복용 동그라미 프리팹
    [SerializeField] private int maxCannonBalls = 3; // 최대 대포알 수
    private int currentCannonBalls;                 // 현재 남은 대포알 수

    private bool activeSkill = false;

    protected override void Start()
    {
        currentCannonBalls = maxCannonBalls;  // 대포알 수 초기화
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
        Debug.Log(this.gameObject.name + " 스킬 발동");
        activeSkill = true;
    }

    private void ShootCannon(Vector2 dir)
    {
        // 대포알이 남아있지 않으면 발사 불가
        if (currentCannonBalls <= 0)
        {
            Debug.Log("대포알이 부족합니다!");
            return;
        }

        GameObject clone = Instantiate(CannonBall, transform.position, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 2000);

        currentCannonBalls--;  // 대포알 수 감소
        Debug.Log("남은 대포알 수: " + currentCannonBalls);

        // 대포알이 파괴되기 직전에 즉시 회복용 동그라미 생성
        CreateRecoveryCircle(clone.transform.position);
    }

    // 동그라미를 즉시 생성하는 함수
    private void CreateRecoveryCircle(Vector3 position)
    {
        // 대포알이 파괴된 위치에 즉시 동그라미 생성
        Debug.Log("동그라미 생성됨! 위치: " + position);
        GameObject recoveryCircle = Instantiate(recoveryCirclePrefab, position, Quaternion.identity);

        // 동그라미는 Skeleton과 충돌했을 때만 사라짐
    }

    // 대포알 회복 함수
    public void RestoreCannonBall()
    {
        if (currentCannonBalls < maxCannonBalls)
        {
            currentCannonBalls++;
            Debug.Log("대포알을 회복했습니다! 현재 대포알 수: " + currentCannonBalls);
        }
    }
}
