using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Battery battery;
    Shoot shootScript;
    Engine engine;

    public GameObject clickSound;
    
    private void Start()
    {
        gameObject.name = "LocalPlayer";

        GameManager.instance.AddObject(gameObject);

        battery = GetComponent<Battery>();
        shootScript = GetComponent<Shoot>();
        engine = GetComponent<Engine>();

        engine.moveSpeed = 5;
    }

    void Update()
    {
        engine.movement.x = Input.GetAxisRaw("Horizontal");
        engine.movement.z = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            if (battery.charges > 0 && !battery.recharging)
                shootScript.Shooting();
            else
                Instantiate(clickSound, transform.position, transform.rotation);

        }
    }

}
