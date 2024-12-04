using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float fallSpeed;
    [SerializeField] public int health;
    public EnemySpawnManager enemySpawnManager;

    void Update()
    {
        transform.position -= new Vector3(0, fallSpeed*Time.deltaTime, 0);
        if (health <= 0)
        {
            Debug.Log("Enemy down!");
            // TODO: Point up
            Destroy(gameObject);
            try
            {
                enemySpawnManager.enemiesKilled++;
            }
            catch
            {
                Debug.Log("Spawnmanager not assigned!");
            }
        }
    }

    IEnumerator turnRed(float fadeSpeed)
    {
        float elapsed = 0;
        float t = 0;

        Color startCol = gameObject.GetComponent<MeshRenderer>().material.color;
        while (elapsed < fadeSpeed)
        {
            elapsed += Time.deltaTime;
            t = elapsed / fadeSpeed;
            t = Mathf.Clamp01(t);   
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red * t;
            yield return null;
        }
        elapsed = fadeSpeed;
        while (elapsed >= 0.3f)
        {
            elapsed -= Time.deltaTime;
            t = elapsed / fadeSpeed;
            t = Mathf.Clamp01(t);
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red * t;
            yield return null;
        }
        gameObject.GetComponent<MeshRenderer>().material.color = startCol;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Hit ground
        if (other.gameObject.layer == 14)
        {
            Debug.Log("Lost health!");

            try
            {
                enemySpawnManager.health -= health;
                enemySpawnManager.enemiesKilled++;
                Destroy(gameObject);
            }
            catch
            {
                Debug.Log("Spawnmanager not assigned!");
            }
        }
        // Hit by projectile
        if (other.gameObject.layer == 9)
        {
            health--;
            StartCoroutine(turnRed(0.2f));
        }
    }
}
