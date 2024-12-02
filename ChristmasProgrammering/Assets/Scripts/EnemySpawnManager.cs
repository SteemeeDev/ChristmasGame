using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Wave;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    [SerializeField] GameObject spawnParent;
    [SerializeField] TMP_Text healthText;
    Wave currentWave;
    int waveIndex = 0;

    int totalEnemies = 0;
    public int enemiesKilled = 0;
    public int health = 10;

    [SerializeField] float waveDifficulty = 1;
    private void Awake()
    {
        currentWave = waves[0];
        foreach (EnemyInfo enemy in currentWave.enemies)
        {
            totalEnemies += enemy.count;
        }
        Debug.Log(totalEnemies);
        StartCoroutine(spawnNewWave(currentWave));
    }
    float GlobalElapsedTime = 0;
    private void Update()
    {
        healthText.text = health.ToString();
        GlobalElapsedTime += Time.deltaTime;
        if ((GlobalElapsedTime > currentWave.totalTimeToBeat || enemiesKilled >= totalEnemies) && waveIndex < waves.Length)
        {
            waveIndex++;
            if (waveIndex >= waves.Length)
            {
                Debug.LogWarning("MAX WAVES REACHED!");
                waveIndex = 0;
                waveDifficulty++;  
                currentWave = waves[waveIndex];
            }
            if ( waveIndex < waves.Length)
            {
                if (enemiesKilled >= totalEnemies)
                {
                    Debug.Log("All enemies killed - spawning new wave");
                }
                enemiesKilled = 0;
                totalEnemies = 0;
                currentWave = waves[waveIndex];
                foreach (EnemyInfo enemy in currentWave.enemies)
                {
                    totalEnemies += enemy.count;
                }
                Debug.Log(totalEnemies);
                GlobalElapsedTime = 0;
                StartCoroutine(spawnNewWave(currentWave));
            }           

        }

    }
    IEnumerator spawnNewWave(Wave wave)
    {
        Debug.Log("Spawning wave " + waveIndex);
        foreach(EnemyInfo enemy in wave.enemies)
        {
            yield return spawnEnemies(enemy);
        }
        
        //Debug.Log("All Enemies spawned!");
    }

    IEnumerator spawnEnemies(Wave.EnemyInfo enemyInfo)
    {
        int enemySpawnCount = 0;
        float elapsedTime = 0;

        float randomTimeModifier = Random.Range(enemyInfo.spawnRandomness[0], enemyInfo.spawnRandomness[1]);


        yield return new WaitForSeconds(enemyInfo.waitTime * 1/waveDifficulty);

        //Debug.Log("Wave enemy: " + enemySpawnInfo.enemyPrefab.name);

        while (enemySpawnCount < enemyInfo.count)
        {
            elapsedTime += Time.deltaTime;

            while (elapsedTime > enemyInfo.timeBetweenSpawns * randomTimeModifier * (1f/waveDifficulty))
            {
                //Debug.Log("Spawning enemy: " + enemySpawnInfo.enemyPrefab.name);
                enemySpawnCount++;
                elapsedTime = 0;
                Vector3 spawnPos = new Vector3(Random.Range(-16.5f, 16.5f), 17, 12.5f);
                GameObject enemy = Instantiate(enemyInfo.enemyPrefab, spawnPos, Quaternion.identity, spawnParent.transform);
                enemy.GetComponent<Enemy>().enemySpawnManager = gameObject.GetComponent<EnemySpawnManager>();
                randomTimeModifier = Random.Range(enemyInfo.spawnRandomness[0], enemyInfo.spawnRandomness[1]);
            }
            yield return null;  
        }

    }
}
