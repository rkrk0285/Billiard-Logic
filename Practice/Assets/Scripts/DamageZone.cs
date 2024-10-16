using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private List<MonsterStat> creaturesInZone = new List<MonsterStat>();
    [SerializeField] private int damageAmount = 4;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterStat creature = collision.GetComponent<MonsterStat>();
        if (creature != null && !creaturesInZone.Contains(creature))
        {
            creaturesInZone.Add(creature); // 구역에 들어온 크리쳐 추가
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MonsterStat creature = collision.GetComponent<MonsterStat>();
        if (creature != null && creaturesInZone.Contains(creature))
        {
            creaturesInZone.Remove(creature); // 구역을 나간 크리쳐 제거
        }
    }

    // 턴 종료 시 구역 내 모든 크리쳐에게 데미지 적용
    public void ApplyDamage()
    {
        foreach (MonsterStat creature in creaturesInZone)
        {
            creature.TakeDamage(damageAmount);
            Debug.Log($"{creature.name}이(가) {damageAmount}의 데미지를 받았습니다.");
        }
    }
}
