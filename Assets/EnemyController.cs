using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Wave[] Waves;

    public GameObject enemyPrefab;

    private static int waveIndex = 0;
    private static bool waveIsOngoing = false;

    public static EnemyController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StartNextWave()
    {
        if (waveIsOngoing)
        {
            return;
        }

        waveIsOngoing = true;

        StartCoroutine(StartWaveClock());
    }

    private IEnumerator StartWaveClock()
    {
        foreach (var enemy in Waves[waveIndex].enemies) 
        {
            Instantiate(enemyPrefab).GetComponent<EnemyBehaviour>().StartEnemy(enemy);

            yield return new WaitForSeconds(1);
        }

        waveIndex++;
        waveIsOngoing = false;
    }
}
