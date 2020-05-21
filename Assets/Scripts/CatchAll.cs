using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchAll : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Player")
            other.GetComponent<Death>().Die();

        Destroy(other.gameObject);
    }
}
