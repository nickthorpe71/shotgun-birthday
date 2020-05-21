﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public GameObject enemyPrefab;
    public GameObject spawnPrefab;
    public GameObject spawnSound;

    GameManager manager;

    bool respawning;

    private void Start()
    {
        InvokeRepeating("ReSpawn", 0, 0.25f);

        manager = GameManager.instance;
    }

    void ReSpawn()
    {
        if (manager.numActive < manager.maxNumActive && !respawning)
        {
            StartCoroutine(Spawn(manager.maxNumActive - manager.numActive / 3));
            StartCoroutine(Spawn(manager.maxNumActive - manager.numActive / 3));
            StartCoroutine(Spawn(manager.maxNumActive - manager.numActive / 3));
        }
    }

    IEnumerator Spawn(int numToSpawn)
    {
        respawning = true;

        for (int i = 0; i < numToSpawn; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-manager.arenaX, manager.arenaX), 1, Random.Range(-manager.arenaZ, manager.arenaZ));
            Quaternion rot = Quaternion.Euler(-90, 0, 0);

            Instantiate(spawnPrefab, pos, rot);
            Instantiate(spawnSound, pos, rot);

            yield return new WaitForSeconds(2);

            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }

        respawning = false;
    }
}