using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LaserBlade : MonoBehaviourPun
{
    public GameObject holder;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject != holder && collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Enemy"))
        {

            GameObject effect = PhotonNetwork.Instantiate("LaserCollision", transform.position, transform.rotation);

            Destroy(effect, 2);

            collider.gameObject.GetComponent<Death>().Die();

            PhotonNetwork.Instantiate("ImpactSound1", transform.position, transform.rotation);

            if (collider.gameObject.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<EnemyControl>().enabled = false;
            }
            else
            {
                collider.gameObject.GetComponent<PlayerControl>().enabled = false;
                collider.gameObject.GetComponent<Rotation>().enabled = false;

                //if (collider.gameObject.name == "LocalPlayer")
                //    GameManager.instance.startButton.SetActive(true);
            }

            collider.gameObject.tag = "Dead";

            if (holder.CompareTag("Enemy") && holder != null)
            {
                if (collider.gameObject == holder.GetComponent<EnemyControl>().lookTarget)
                    holder.GetComponent<EnemyControl>().lookTarget = null;
            }
        }
        else if(!collider.gameObject.CompareTag("Bullet"))
        {
            //PhotonNetwork.Instantiate(impactSound2, transform.position, transform.rotation);
        }
    }
}
