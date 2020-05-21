using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbStorage : MonoBehaviour
{
    public int orbCount = 10;
    Engine engine;
    Shoot shootScript;
    public Light lightSource;

    public GameObject crown;

    UIManager uIManager;

    private void Start()
    {
        engine = GetComponent<Engine>();
        shootScript = GetComponent<Shoot>();

        uIManager = GameManager.instance.uIManager;

        if (gameObject.name == "LocalPlayer")
            uIManager.updateUI(orbCount);
    }

    public void increaseDecreaseOrbs(int amount)
    {
        float increment = (float)amount;
        float multiplier = (float)orbCount / 10  +10;
        orbCount += amount;
        engine.moveSpeed += increment / (5f * multiplier + 25f);
        shootScript.laserScale += increment / (20f * multiplier + 50f);
        lightSource.range += increment / multiplier;
        GameManager.instance.CheckCrown(this, orbCount);

        if (gameObject.name == "LocalPlayer")
        {
            uIManager.updateUI(orbCount);
        }
    }
}
