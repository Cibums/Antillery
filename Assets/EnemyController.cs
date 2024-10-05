using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Wave[] Waves;

    public GameObject enemyPrefab;

    private static int waveIndex = 0;
    public static bool waveIsOngoing = false;

    public static EnemyController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartNextWave();
        }
    }

    public void StartNextWave()
    {
        if (waveIsOngoing)
        {
            Debug.LogWarning("Previous wave till ongoing");
            return;
        }

        Debug.LogWarning("Starting next wave");

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
    }

    public void CheckIfWaveEnded()
    {
        var enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Count();
        Debug.Log($"Enemies Left: {enemyCount}");
        if (enemyCount <= 1)
        {
            waveIsOngoing = false;
        }
    }
}
