using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;

public class InfoUI : MonoBehaviour
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
    public void ShowBarrier(int barrierCount)
    {
        if (barrierCount > 0)
        {
            transform.Find("Barrier").GetChild(0).GetComponent<TextMeshPro>().text = barrierCount.ToString();
            transform.Find("Barrier").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Barrier").gameObject.SetActive(false);
        }
    }
    public void ShowAttack(float currentATK, float ATK)
    {
        if (currentATK > ATK)
            transform.Find("Attack").gameObject.SetActive(true);
        else
            transform.Find("Attack").gameObject.SetActive(false);
    }
    public void ShowPower(float currentPower, float power)
    {
        transform.Find("Accelerate").gameObject.SetActive(false);
        transform.Find("Decelerate").gameObject.SetActive(false);

        if (currentPower > power)
            transform.Find("Accelerate").gameObject.SetActive(true);
        else if (currentPower < power)
            transform.Find("Decelerate").gameObject.SetActive(true);        
    }
    public void ShowMass(float currentMass, float mass)
    {
        if (currentMass > mass)
            transform.Find("WeightUp").gameObject.SetActive(true);
        else
            transform.Find("WeightUp").gameObject.SetActive(false);
    }
    public void ShowSkip(bool toggle)
    {
        transform.Find("Skip").gameObject.SetActive(toggle);
    }
}
