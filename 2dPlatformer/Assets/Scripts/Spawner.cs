using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public Transform enemySpawnPoint;
    public float spawnInterval = 3f;
    void Start()
    {
        spawnInterval = 5f;

        enemySpawnPoint = GameObject.Find("SpawnPoint").GetComponent<Transform>();
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void FixedUpdate()
    {
        
    }


    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
    }

    
}
