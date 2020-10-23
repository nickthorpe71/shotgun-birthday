using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shoot : MonoBehaviourPun
{
    public Transform firePoint;
    public GameObject gunObj;
    [HideInInspector] public float laserScale = 0.3f;
    public float laserForce = 50;

    Battery battery;

    private void Start()
    {
        battery = GetComponent<Battery>();
    }

    public void Shooting()
    {
        GameObject laser = PhotonNetwork.Instantiate("Laser", firePoint.transform.position, Quaternion.identity);
        PhotonNetwork.Instantiate("GunSound", firePoint.position, firePoint.rotation);
        laser.GetComponent<Laser>().shooter = gameObject;
        Rigidbody rb = laser.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * laserForce, ForceMode.Impulse);
        laser.transform.localScale = new Vector3(laserScale, laserScale, laserScale);
        battery.ReduceCharge();
    }
}
