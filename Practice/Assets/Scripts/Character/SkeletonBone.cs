using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBone : MonoBehaviour
{
    private int unitID;
    public void SetSkeletonID(int unitID)
    {
        this.unitID = unitID;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnitManager.Instance.GetBone(this.unitID);
            Destroy(this.gameObject);
        }
    }
}
