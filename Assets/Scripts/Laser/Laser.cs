using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Laser : MonoBehaviourPun
{
    public float laserDuration = 0.6f;
    [HideInInspector] public GameObject shooter;

    private void Start()
    {
        Destroy(gameObject, laserDuration);
    }

    void OnCollisionEnter(Collision collider)
	{
        if(collider.gameObject != shooter && collider.gameObject.tag != "Collect" && collider.gameObject.tag != "Ground")
        {
            GameObject effect = PhotonNetwork.Instantiate("LaserCollision", transform.position, transform.rotation);

            Destroy(effect, 2);
            Destroy(gameObject);

            if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<Death>().Die();

                PhotonNetwork.Instantiate("ImpactSound1", transform.position, transform.rotation);

                //shooter.GetComponent<Battery>().Recharge();

                if (collider.gameObject.tag == "Enemy")
                {
                    collider.gameObject.GetComponent<EnemyControl>().enabled = false;
                }
                else
                {
                    collider.gameObject.GetComponent<PlayerControl>().enabled = false;
                    collider.gameObject.GetComponent<Rotation>().enabled = false;

                    //if(collider.gameObject.name == "LocalPlayer")
                    //    GameManager.instance.startButton.SetActive(true);
                }

                collider.gameObject.tag = "Dead";

                if (shooter.tag == "Enemy" && shooter != null)
                {
                    if(collider.gameObject == shooter.GetComponent<EnemyControl>().lookTarget)
                        shooter.GetComponent<EnemyControl>().lookTarget = null;
                }
            }
            else
            {
                PhotonNetwork.Instantiate("ImpactSound2", transform.position, transform.rotation);
            }

        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject != shooter && collider.gameObject.tag != "Collect" && collider.gameObject.tag != "Ground")
        {
            GameObject effect = PhotonNetwork.Instantiate("LaserCollision", transform.position, transform.rotation);

            Destroy(effect, 2);
            Destroy(gameObject);

            if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<Death>().Die();
                collider.gameObject.GetComponent<Battery>().charges = 0;
                collider.gameObject.GetComponent<Battery>().ChangeColor(0);
                collider.gameObject.GetComponent<Battery>().enabled = false;
                collider.gameObject.GetComponent<Engine>().enabled = false;
                collider.gameObject.GetComponent<Battery>().StopAllCoroutines();

                PhotonNetwork.Instantiate("ImpactSound1", transform.position, transform.rotation);

                //shooter.GetComponent<Battery>().Recharge();

                if (collider.gameObject.tag == "Enemy")
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

                if (shooter.tag == "Enemy" && shooter != null)
                {
                    if (collider.gameObject == shooter.GetComponent<EnemyControl>().lookTarget)
                        shooter.GetComponent<EnemyControl>().lookTarget = null;
                }
            }
            else
            {
                PhotonNetwork.Instantiate("ImpactSound2", transform.position, transform.rotation);
            }

        }
    }
}
