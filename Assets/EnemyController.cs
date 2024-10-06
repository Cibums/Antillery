using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy[] allEnemies;

    public List<Wave> Waves;

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

    public void StartNextWave()
    {
        if (waveIsOngoing)
        {
            Debug.LogWarning("Previous wave till ongoing");
            return;
        }

        Debug.LogWarning("Starting next wave");

        AudioController.instance.UpdateMusic();
        waveIsOngoing = true;
        InterfaceController.Instance.SetBuyPanelState(false);
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
            if (waveIndex >= 0 && waveIndex == Waves.Count - 1)
            {
                var newWave = GenerateNewWave();
                Waves.Add(newWave);
            }

            waveIsOngoing = false;
            InterfaceController.Instance.SetBuyPanelState(true);
            AudioController.instance.UpdateMusic();
        }
    }

    private Wave GenerateNewWave()
    {
        var newWave = Waves[waveIndex];

        int count = Waves[waveIndex].enemies.Count;

        for (int i = 0; i < Mathf.RoundToInt(count / 3); i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, allEnemies.Length);
            var enemy = allEnemies[randomIndex];
            newWave.enemies.Add(enemy);
        }

        return newWave;
    }
}
