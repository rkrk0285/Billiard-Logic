using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CerberusStat : MonsterStat
{
    [SerializeField] private Transform arrowParents;
    [SerializeField] private Transform SkeletonTrans;

    private E_CerberusHead currentHead = E_CerberusHead.Fire;
    private enum E_CerberusHead
    {
        Fire = 0,
        Bite = 1,
        Dig = 2,
    }
    public override void ResetStartParameter()
    {
        // Do Nothing.
        // Todo. Call this function When this Monster's turn started
        SelectCerberusHead();
        SetCerberusSkill();
    }
    private void SelectCerberusHead()
    {
        int rand = Random.Range(0, 3);
        currentHead = (E_CerberusHead)rand;
    }
    private void SetCerberusSkill()
    {
        // Reset Action
        skill = () => { };

        switch (currentHead)
        {
            case E_CerberusHead.Fire:
                skill += () => GetComponent<ShootCannonSkill>().Activate();
                break;
            case E_CerberusHead.Bite:
                skill += () => GetComponent<BiteSkill>().Activate();
                break;
            case E_CerberusHead.Dig:                
                skill += () => BringBoneToSkeleton();
                break;
        }
        Debug.Log(currentHead);
    }

    private void BringBoneToSkeleton()
    {
        float closestDist = float.MaxValue;
        GameObject closestBone = null;
        for (int i = 0; i < arrowParents.childCount; i++)
        {
            float dist = Vector2.Distance(transform.position, arrowParents.GetChild(i).position);
            if (closestDist > dist)
            {
                closestDist = dist;
                closestBone = arrowParents.GetChild(i).gameObject;
            }
        }

        if (closestBone != null)
        {
            StartCoroutine(MoveToSkeleton(closestBone.transform.position, SkeletonTrans.position));
            Destroy(closestBone.gameObject);
        }
    }
    
    public IEnumerator MoveToSkeleton(Vector2 BonePos, Vector2 SkeletonPos)
    {
        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 2)
        {            
            float t = elapsedTime / 1;            
            transform.position = Vector2.Lerp(startPosition, BonePos, t);            
            elapsedTime += Time.deltaTime;
            yield return null;
        }        
        transform.position = BonePos;
        
        startPosition = transform.position;
        elapsedTime = 0f;

        while (elapsedTime < 2)
        {
            float t = elapsedTime / 1;
            transform.position = Vector2.Lerp(startPosition, SkeletonPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = SkeletonPos;
        SkeletonTrans.GetComponent<ShootArrowSkill>().PickUpArrow();
    }    
}