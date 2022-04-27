using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private GameObject _rarePowerup; //the rare power up is the heat seeking missiles

    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerup());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(3);

        while (_stopSpawning == false)
        {
            float randomXPosition = Random.Range(-10, 10);
            Vector3 spawnPosition = new Vector3(randomXPosition, 8.5f, 0);

            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(3);

        while (_stopSpawning == false)
        {
            float randomXPosition = Random.Range(-10, 10);
            Vector3 spawnPosition = new Vector3(randomXPosition, 8.5f, 0);
            float randomSpawnTime = Random.Range(3f, 7f);
            int randomPowerup = Random.Range(0, _powerups.Length);
            int rarePowerupChance = Random.Range(0, 101);

            if (rarePowerupChance <= 10)
            {
                Instantiate(_rarePowerup, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(randomSpawnTime);
            }
            else if (rarePowerupChance >= 10)
            {
                Instantiate(_powerups[randomPowerup], spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(randomSpawnTime);
            }
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}