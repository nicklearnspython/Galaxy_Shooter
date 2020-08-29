using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;

    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        float enemySpawnDelay = 3.0f;
        bool isEnemySpawningFaster = true;
        
        yield return new WaitForSeconds(3.0f);
        
        while (_stopSpawning == false)
        {
            Vector3 positionToSpawn = new Vector3(Random.Range(-8f, 8f), 6, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, positionToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(enemySpawnDelay);
            switch (isEnemySpawningFaster)
            {
                case true:
                    enemySpawnDelay -= 0.1f;
                    if(enemySpawnDelay < 0.2f)
                    {
                        isEnemySpawningFaster = false;
                        Debug.Log("Slowing Spawn Rate.");
                    }
                    break;
                
                case false:
                    enemySpawnDelay += 0.4f;
                    if(enemySpawnDelay > 2.0f)
                    {
                        isEnemySpawningFaster = true;
                        Debug.Log("Increaasing Spawn Rate.");
                    }
                    break;
            }
            if (enemySpawnDelay > 0.3f)
            {
                enemySpawnDelay -= 0.1f;
            }
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(12.0f); //12 seconds is a good number
        
        while (_stopSpawning == false)
        {
            Vector3 positionToSpawn = new Vector3(Random.Range(-8f, 8f), 6, 0);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerup], positionToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5, 10));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
