﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private float spawnRate = 5.0f;
    private float timeBetweenWaves = 10f;

    public int enemyCount = 3; // how many in each wave
    public int wave;

    //used for spawning in enemies
    public GameObject violin1, violin2, violin3, violin4;
    public GameObject cello;

    bool waveIsDone = true;

    // Update is called once per frame
    void Update()
    {
        if (waveIsDone)
        {
            StartCoroutine(waveSpawner());
        }
    }

    IEnumerator waveSpawner()
    {
        waveIsDone = false;

        //3 waves of regular enemies
        for(int i = 0; i < 4; i++)
        {
            for (int j = 0; j < enemyCount; j++)
            {
                GameObject violinEnemy;
                int type = Random.Range(1, 4);
                switch (type)
                {
                    case 1:
                        violinEnemy = Instantiate(violin1);
                        break;
                    case 2:
                        violinEnemy = Instantiate(violin2);
                        break;
                    case 3:
                        violinEnemy = Instantiate(violin3);
                        break;
                    case 4:
                        violinEnemy = Instantiate(violin4);
                        break;
                }
                yield return new WaitForSeconds(spawnRate);
            }
            yield return new WaitForSeconds(timeBetweenWaves);
            //spawn cello between waves
            GameObject celloEnemy = Instantiate(cello);
        }
        waveIsDone = true;
    }
}
