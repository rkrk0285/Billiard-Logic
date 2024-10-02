using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWall : MonoBehaviour
{
    [SerializeField] private int maxHits = 3;        // 최대 충돌 횟수
    [SerializeField] private float fadeAmount = 0.33f; // 한 번 충돌할 때마다 감소할 투명도 비율 (1 / maxHits)

    private int hitCount = 0; // 현재 충돌 횟수
    private SpriteRenderer spriteRenderer; // 벽의 색상을 변경할 SpriteRenderer

    private void Start()
    {
        // SpriteRenderer 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 벽의 초기 색상 불투명 설정
        Color initialColor = spriteRenderer.color;
        initialColor.a = 1f;
        spriteRenderer.color = initialColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 벽에 충돌할 때마다 충돌 횟수 증가
        hitCount++;

        // 투명도 변경
        Color newColor = spriteRenderer.color;
        newColor.a = Mathf.Clamp01(1f - fadeAmount * hitCount); // 투명도 점진적으로 줄이기
        spriteRenderer.color = newColor;

        // 충돌 횟수가 maxHits에 도달하면 벽을 파괴
        if (hitCount >= maxHits)
        {
            Destroy(gameObject);
        }
    }
}
