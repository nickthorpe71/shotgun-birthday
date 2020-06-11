using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public GameObject playerPrefab;
    public GameObject spawnPrefab;
    public GameObject spawnSound;

    //bool respawning;

    public void SpawnPlayerGun()
    {
        StartCoroutine(SpawnRoutine("Gun"));
    }

    public void SpawnPlayerSword()
    {
        StartCoroutine(SpawnRoutine("Sword"));
    }

    IEnumerator SpawnRoutine(string weaponClass)
    {
        GameObject mainCam = GameManager.instance.mainCam;

        //respawning = true;

        GameManager.instance.startButton.SetActive(false);

        Vector3 pos = new Vector3(Random.Range(-75f, 75f), 1, Random.Range(-75f, 75f));
        Quaternion rot = Quaternion.Euler(-90, 0, 0);

        mainCam.GetComponent<CameraFollow>().following = false;
        mainCam.transform.position = new Vector3(pos.x, 35, pos.z);

        Instantiate(spawnPrefab, pos, rot);
        Instantiate(spawnSound, pos, rot);

        yield return new WaitForSeconds(2);

        mainCam.GetComponent<CameraFollow>().following = true;
        GameObject newPlayer = Instantiate(playerPrefab, pos, Quaternion.identity);
        newPlayer.GetComponent<PlayerControl>().weaponClass = weaponClass;
        mainCam.GetComponent<CameraFollow>().target = newPlayer.transform;

        //respawning = false;
    }


}
