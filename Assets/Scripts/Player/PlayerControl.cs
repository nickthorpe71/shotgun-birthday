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

    public bool gunClass;
    public bool swordClass;
    
    private void Start()
    {
        gameObject.name = "LocalPlayer";

        GameManager.instance.AddObject(gameObject);

        battery = GetComponent<Battery>();
        shootScript = GetComponent<Shoot>();
        swordScript = GetComponent<Sword>();
        engine = GetComponent<Engine>();

        

        if (gunClass)
        {
            shootScript.gunObj.SetActive(true);
            engine.moveSpeed = 5;
        }
        else if (swordClass)
        {
            swordScript.swordObj.SetActive(true);
            engine.moveSpeed = 7.77f;
        }
            
    }

    void Update()
    {
        engine.movement.x = Input.GetAxisRaw("Horizontal");
        engine.movement.z = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            if (gunClass)
                Gun();
            else if (swordClass)
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
