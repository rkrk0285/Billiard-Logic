using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // 각 스켈레톤이 죽었을 때 불리는 함수.    

    // 스켈레톤 관리 자료구조 => 리스트 예상.

    public static UnitManager Instance;

    private void Awake()
    {
        if (Instance == null)        
            Instance = this;
        else
            Destroy(Instance);
    }

    [SerializeField] private Transform AllyTransform;
    [SerializeField] private GameObject unit;
    [SerializeField] private GameObject firstHead;

    private List<GameObject> UnitArmy = new List<GameObject>();    
    private GameObject unitHead;
    private void Start()
    {
        unitHead = firstHead;
        UnitArmy.Add(firstHead);
        UpdateUnitIndex();
    }
    private void UpdateUnitIndex()
    {
        Stack<int> deleteIndex = new Stack<int>();

        // Remove Dead Unit
        for(int i = 0; i < UnitArmy.Count; i++)
        {
            if (UnitArmy[i] == null)            
                deleteIndex.Push(i);            
        }
        while (deleteIndex.Count > 0)
        {
            UnitArmy.RemoveAt(deleteIndex.Pop());
        }

        // Update Index
        bool existHead = false;
        for(int i = 0; i < UnitArmy.Count; i++)
        {
            SkeletonStat unitStat = UnitArmy[i].GetComponent<SkeletonStat>();
            if (unitStat.GetIsHead())            
                existHead = true;            
            unitStat.SetIndex(i);
        }

        // Check Head Unit Alive
        if (!existHead && UnitArmy.Count != 0)
        {
            UnitArmy[0].GetComponent<SkeletonStat>().SetHead();
            unitHead = UnitArmy[0];
        }
    }    
    public void AddUnit()
    {
        GameObject clone = Instantiate(unit, AllyTransform);        
        UnitArmy.Add(clone);

        int count = UnitArmy.Count - 1;
        clone.GetComponent<SkeletonStat>().SetIndex(count);
    }
    public Vector3 GetPrevUnitPosition(int index)
    {
        if (index >= UnitArmy.Count)
            return Vector3.zero;

        if (UnitArmy[index - 1] == null)
            return Vector3.zero;

        return UnitArmy[index - 1].transform.position;
    }    
    public void DelayedUpdateUnit()
    {        
        StartCoroutine(DelayedUpdate());
    }
    IEnumerator DelayedUpdate()
    {
        yield return new WaitForFixedUpdate();
        UpdateUnitIndex();
        StopCoroutine(DelayedUpdate());
    }
    public void OnClickHeadDead()
    {
        Destroy(unitHead);
    }
    public GameObject GetUnitHead()
    {
        if (unitHead != null)
            return unitHead;
        else
            return null;
    }
    public int GetUnitArmyLength()
    {
        return UnitArmy.Count;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (unitHead != null)
            {
                unitHead.GetComponent<MonsterController>().ChangeState(E_MonsterState.Ready);
            }
        }
    }
}
