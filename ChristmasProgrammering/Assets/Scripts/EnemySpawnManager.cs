using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Wave;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    [SerializeField] GameObject spawnParent;
    [SerializeField] GameObject slingshot;

    [SerializeField] GameObject deathMenu;
    [SerializeField] TMP_Text deathScore;
    [SerializeField] GameObject mainMenu;

    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text waveText;
    Wave currentWave;
    [SerializeField] int waveIndex = 0;
    [SerializeField] float waveDifficulty = 1;

    int totalEnemies = 0;
    public int enemiesKilled = 0;
    public int health = 10;
    public int score = 0;

    
    private void Awake()
    {
        currentWave = waves[waveIndex];
        foreach (EnemyInfo enemy in currentWave.enemies)
        {
            totalEnemies += enemy.count;
        }
        Debug.Log(totalEnemies);
        StartCoroutine(spawnNewWave(currentWave));
    }
    float GlobalElapsedTime = 0;

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }
    private void Update()
    {
        if (health <= 0)
        {
            slingshot.SetActive(false);
            deathMenu.SetActive(true);
            mainMenu.SetActive(false);
            deathScore.text = score.ToString();
        }


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
        waveText.text = (waveIndex+1).ToString();
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
        float elapsedTime = 9999;

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
                Vector3 spawnPos = new Vector3(Random.Range(-16f, 16f), 16.5f, 12.5f);
                GameObject enemy = Instantiate(enemyInfo.enemyPrefab, spawnPos, Quaternion.identity, spawnParent.transform);
                enemy.GetComponent<Enemy>().enemySpawnManager = gameObject.GetComponent<EnemySpawnManager>();
                randomTimeModifier = Random.Range(enemyInfo.spawnRandomness[0], enemyInfo.spawnRandomness[1]);
            }
            yield return null;  
        }

    }

    public void UpdateScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();
    }
}
