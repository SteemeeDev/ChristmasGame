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
        }
    }
}
