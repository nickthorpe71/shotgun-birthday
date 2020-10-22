using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Battery battery;
    Shoot shootScript;
    Sword swordScript;
    Engine engine;

    public GameObject clickSound;

    public string weaponClass;
    
    private void Start()
    {
        gameObject.name = "LocalPlayer";

        GameManager.instance.AddObject(gameObject);

        battery = GetComponent<Battery>();
        shootScript = GetComponent<Shoot>();
        swordScript = GetComponent<Sword>();
        engine = GetComponent<Engine>();

        if (weaponClass == "Gun")
        {
            shootScript.gunObj.SetActive(true);
            engine.moveSpeed = 5;
            swordScript.enabled = false;
        }
        else if (weaponClass == "Sword")
        {
            swordScript.swordObj.SetActive(true);
            engine.moveSpeed = 6.65f;
            shootScript.enabled = false;
        }
            
    }

    void Update()
    {
        engine.movement.x = Input.GetAxisRaw("Horizontal");
        engine.movement.z = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            if (weaponClass == "Gun")
                Gun();
            else if (weaponClass == "Sword")
                Sword();
        }
    }

    void Gun()
    {
        if (battery.charges > 0 && !battery.recharging)
            shootScript.Shooting();
        else
            Instantiate(clickSound, transform.position, transform.rotation);
    }

    void Sword()
    {
        if(swordScript.canSlash)
            swordScript.Slash();
    }

}
