using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Warp : MonoBehaviourPun
{
    Vector3 newPosition;

    PlayerControl moveScript;
    Battery battery;
    Engine engine;

    bool dashing;

    void Start()
    {
        newPosition = transform.position;
        moveScript = GetComponent<PlayerControl>();
        battery = GetComponent<Battery>();
        engine = GetComponent<Engine>();
    }

    void Update()
    {
        SpaceDash();
    }

    void SpaceDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !dashing && battery.charges > 0 && !battery.recharging)
        {
            StartCoroutine(DashRoutine());
            if(GetComponent<PlayerControl>())
            {
                if (GetComponent<PlayerControl>().weaponClass == "Sword")
                {
                    GetComponent<Sword>().Slash();
                }
            }
        }
    }

    IEnumerator DashRoutine()
    {
        float oldMoveSpeed = engine.moveSpeed;
        battery.ReduceCharge();
        WarpEffect(transform.position);
        dashing = true;
        engine.moveSpeed = 100;
        yield return new WaitForSeconds(0.07f);
        engine.moveSpeed = oldMoveSpeed;
        dashing = false;

        //GetComponent<OrbStorage>().increaseDecreaseOrbs(-1);
    }

    void WarpEffect(Vector3 pos)
    {
        Vector3 newPos = new Vector3(pos.x, 0, pos.z);
        Quaternion rotation =  Quaternion.Euler (-90, 0, 0);

        GameObject effect = PhotonNetwork.Instantiate("WarpEffect", newPos, rotation);
        PhotonNetwork.Instantiate("WarpSound", newPos, rotation);
        Destroy(effect, 2);
    }
}