using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrowSkill : SkillBase
{
    [SerializeField] private GameObject arrowPrefab;
    private bool activeSkill = false;

    private void Update()
    {
        if (activeSkill)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = worldPos - new Vector2(transform.position.x, transform.position.y);

            if (Input.GetMouseButtonDown(0))
            {
                activeSkill = false;
                ShootArrow(dir);
            }
            else
                DrawLine(dir);
        }
    }

    public override void Activate()
    {
        base.Activate();
        Debug.Log(this.gameObject.name + " 스킬 활성화");
        activeSkill = true;
    }

    private void ShootArrow(Vector2 dir)
    {
        // 화살 생성
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 중력 영향 제거
        rb.AddForce(dir.normalized * 2000); // 화살 발사
    }

}
