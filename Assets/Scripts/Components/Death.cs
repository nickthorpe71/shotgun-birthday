using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    OrbStorage orbStore;
    Battery battery;

    public GameObject orbPrefab;
    
    private void Start()
    {
        orbStore = GetComponent<OrbStorage>();
        battery = GetComponent<Battery>();
    }

    public void Die()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        DropOrbs();

        GetComponent<Battery>().charges = 0;
        GetComponent<Battery>().ChangeColor(5);
        GetComponent<Battery>().enabled = false;
        GetComponent<Engine>().enabled = false;
        GetComponent<Battery>().StopAllCoroutines();

        if (GameManager.instance.winningPlayer == gameObject)
        {
            GameManager.instance.winningPlayer = null;
            GameManager.instance.highestScore = 100;
            orbStore.crown.SetActive(false);
        }
        GameManager.instance.RemoveObject(gameObject);

        Destroy(gameObject, 30);
    }

    void DropOrbs()
    {
        int orbs = orbStore.orbCount;
        float radius = battery.lightObj.range /2 + 1;
        while (orbs > 0)
        {
            int value = Random.Range(1, orbs);
            if (value > 15)
                value = 15;

            float randX = Random.Range(Random.Range(-radius, -1), Random.Range(0, radius));
            float randZ = Random.Range(Random.Range(-radius, -1), Random.Range(0, radius));
            float scale = (value / 10f) + 0.3f;
            Vector3 pos = new Vector3(transform.position.x + randX, scale / 2 + 1, transform.position.z + randZ);
            GameObject newOrb = Instantiate(orbPrefab, pos, transform.rotation);
            newOrb.GetComponent<Orb>().value = value;
            newOrb.transform.localScale = new Vector3(scale, scale, scale);

            orbs -= value;
        }
    }
}
