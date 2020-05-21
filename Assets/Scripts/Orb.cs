using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [HideInInspector] public int value = 1;
    
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = new Color(
          Random.Range(0f, 1f),
          Random.Range(0f, 1f),
          Random.Range(0f, 1f)
        );

        GameManager.instance.AddObject(gameObject);

        Destroy(gameObject, 15 + value);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<OrbStorage>().increaseDecreaseOrbs(value);
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.GetComponent<EnemyControl>().moveTarget == gameObject)
                collision.gameObject.GetComponent<EnemyControl>().moveTarget = null;
        }

        if (collision.gameObject.tag == "Barrier")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.RemoveObject(gameObject);
    }

}
