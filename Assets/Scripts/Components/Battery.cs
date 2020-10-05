using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public int charges = 3;
    int maxCharges;
    public MeshRenderer[] lightObjects;
    public Light lightObj;
    public Color[] batteryColors;

    float rechargeTime = 2.5f;
    [HideInInspector] public bool recharging;

    //public GameObject reloadStartSound;
    public GameObject reloadCompleteSound;

    void Start()
    {
        maxCharges = charges;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !recharging && charges < maxCharges && gameObject.tag == "Player")
            SlowRecharge();
    }

    public void SlowRecharge()
    {
        StartCoroutine(SlowRechargeRoutine());
    }

    IEnumerator SlowRechargeRoutine()
    {
        ChangeColor(maxCharges + 1);
        //Instantiate(reloadStartSound, transform.position, transform.rotation);
        recharging = true;
        yield return new WaitForSeconds(rechargeTime - 0.25f);
        Instantiate(reloadCompleteSound, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.25f);
        recharging = false;
        Recharge();
    }


    public void Recharge()
    {
        charges = maxCharges;
        ChangeColor(maxCharges);
    }

    public void ReduceCharge()
    {
        if (charges > 0)
        {
            charges--;

            ChangeColor(charges);
        }
    }

    public void ChangeColor(int color)
    {
        for (int i = 0; i < lightObjects.Length; i++)
        {
            lightObjects[i].material.color = batteryColors[color];
            lightObj.color = batteryColors[color];
        }

        if (color == 0)
            lightObj.enabled = false;
        else
            lightObj.enabled = true;
    }


}
