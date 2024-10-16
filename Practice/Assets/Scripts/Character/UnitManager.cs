using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private void Awake()
    {
        if (Instance == null)        
            Instance = this;
        else
            Destroy(Instance);
    }

    [SerializeField] private Transform AllyTransform;
    [SerializeField] private GameObject[] units;        

    private List<GameObject> UnitArmy = new List<GameObject>();
    private Dictionary<int, int> boneAcquisitionStatus = new Dictionary<int, int>();
    private GameObject unitHead;
    private void Start()
    {
        unitHead = AllyTransform.GetChild(0).gameObject;        
        for(int i = 0; i < AllyTransform.childCount; i++)
        {
            UnitArmy.Add(AllyTransform.GetChild(i).gameObject);
        }
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
    //public void AddUnit()
    //{
    //    GameObject clone = Instantiate(unit, AllyTransform);
    //    UnitArmy.Add(clone);

    //    int count = UnitArmy.Count - 1;
    //    clone.GetComponent<SkeletonStat>().SetIndex(count);
    //}
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
    public void GetBone(int unitID)
    {
        if (!boneAcquisitionStatus.ContainsKey(unitID))        
            boneAcquisitionStatus.Add(unitID, 1);        
        else        
            boneAcquisitionStatus[unitID]++;

        Debug.Log(unitID + " " + boneAcquisitionStatus[unitID]);
        if (boneAcquisitionStatus[unitID] == 3)
        {
            boneAcquisitionStatus[unitID] = 0;
            ReviveUnit(unitID);
        }
    }
    public void ReviveUnit(int unitID)
    {        
        GameObject clone = Instantiate(units[unitID], AllyTransform);
        UnitArmy.Add(clone);

        int count = UnitArmy.Count - 1;
        clone.GetComponent<SkeletonStat>().SetIndex(count);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (unitHead != null)            
                unitHead.GetComponent<MonsterController>().ChangeState(E_MonsterState.Ready);                            
        }
    }
}