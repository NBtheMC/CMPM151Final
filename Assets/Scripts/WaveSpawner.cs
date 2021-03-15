using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private float spawnRate = 1.0f;
    private float timeBetweenWaves = 10f;

    public int enemyCount = 4; // how many in each wave
    public int wave;

    //used for spawning in enemies
    public GameObject violin;
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
        for(int i = 0; i < 3; i++)
        {
            for (int j = 0; j < enemyCount; j++)
            {
                GameObject violinEnemy = Instantiate(violin);
                violinEnemy.GetComponent<ViolinEnemy>().setType(Random.Range(0, 4));

                yield return new WaitForSeconds(spawnRate);
            }
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        //spawn cello
        GameObject celloEnemy = Instantiate(cello);

        waveIsDone = true;
    }
}
