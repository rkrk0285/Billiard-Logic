using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrowSkill : SkillBase
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject droppedArrowPrefab; // 바닥에 떨어진 화살 프리팹
    [SerializeField] private Vector2 spawnOffset; // 화살 발사 위치 오프셋
    [SerializeField] private Transform arrowParents;
    
    [Space]
    [Range(1, 100)][SerializeField] private float ChangeDirectionSpeed;

    private float angleOffset;
    private bool angleIncrease;

    private bool hasArrow = true; // 스켈레톤이 화살을 가지고 있는지 여부
    private bool activeSkill = false;
    protected override void Start()
    {
        base.Start();
        angleOffset = 0f;
        angleIncrease = true;
    }
    private void Update()
    {
        if (activeSkill && hasArrow) // 화살이 있을 때만 발사 가능
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = worldPos - new Vector2(transform.position.x, transform.position.y);
            dir = ChangeAngleByDirection(dir);

            if (Input.GetMouseButtonDown(0))
            {
                activeSkill = false;
                ShootArrow(dir);
                DisableLine();
            }
            else
                DrawLine(dir);
        }
    }

    public override void Activate()
    {
        if (hasArrow) // 화살이 있을 때만 스킬 활성화
        {
            base.Activate();
            Debug.Log(this.gameObject.name + " 스킬 활성화");
            activeSkill = true;
        }
        else
        {
            Debug.Log("화살을 먼저 주워야 합니다!");
        }
    }

    private void ShootArrow(Vector2 dir)
    {
        // 화살이 발사될 위치를 오프셋을 기준으로 설정        
        Vector2 spawnPosition = (Vector2)transform.position + dir.normalized * 1f;
        // 화살 생성
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity, arrowParents.transform);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 중력 영향 제거
        rb.AddForce(dir.normalized * 2000); // 화살 발사

        hasArrow = false; // 화살을 발사했으므로 false로 설정
    }

    private Vector2 ChangeAngleByDirection(Vector2 dir)
    {
        if (angleIncrease)
        {
            angleOffset += ChangeDirectionSpeed * Time.deltaTime;
            if (angleOffset > 45f)
            {
                angleOffset = 45f;
                angleIncrease = false;
            }
        }
        else
        {
            angleOffset -= ChangeDirectionSpeed * Time.deltaTime;
            if (angleOffset < -45f)
            {
                angleOffset = -45f;
                angleIncrease = true;
            }
        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset;
        float radian = angle * Mathf.Deg2Rad;        
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    // 바닥에 떨어진 화살을 주울 때 호출될 메서드
    public void PickUpArrow()
    {
        hasArrow = true;
        Debug.Log("화살을 다시 주웠습니다!");
    }
}
