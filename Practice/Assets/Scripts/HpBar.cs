using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    private Transform fillTransform;
    public void Awake()
    {
        fillTransform = transform.Find("Fill");
    }
    public void ShowHpBar(float hpPercent)
    {
        Vector3 originScale = fillTransform.localScale;

        fillTransform.localScale = new Vector3(hpPercent, originScale.y, originScale.z);
        fillTransform.localPosition = Vector3.zero - new Vector3((1 - hpPercent) / 2, 0, 0);
    }
}
