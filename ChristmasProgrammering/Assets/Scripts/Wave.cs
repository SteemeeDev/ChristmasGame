using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave", order = 1)]
public class Wave : ScriptableObject
{
    [System.Serializable]
    public class EnemyInfo
    {
        public GameObject enemyPrefab;
        public float waitTime;
        public int count;
        public float timeBetweenSpawns;
        public float[] spawnRandomness;
    }
    public float totalTimeToBeat;
    public EnemyInfo[] enemies;
}
