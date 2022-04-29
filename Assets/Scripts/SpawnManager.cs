using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject[] _powerups;
    [SerializeField] GameObject _rarePowerup; //the rare power up is the heat seeking missiles

    [SerializeField] Enemy[] _wave1;
    [SerializeField] Enemy[] _wave2;
    [SerializeField] Enemy[] _wave3;

    bool _stopSpawning = false;
    int _currentWave = 1;
    Vector3 spawnPosition;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerup());
    }

    IEnumerator SpawnEnemies()
    {
        while (_stopSpawning == false)
        {
            switch (_currentWave)
            {
                case 1:
                    _currentWave++;
                    foreach (Enemy enemy in _wave1)
                    {
                        SpawnEnemy();
                    }
                    break;
                case 2:
                    _currentWave++;
                    foreach (Enemy enemy in _wave2)
                    {
                        SpawnEnemy();
                    }
                    break;
                case 3:
                    foreach (Enemy enemy in _wave3)
                    {
                        SpawnEnemy();
                    }
                    break;
                default:
                    Debug.Log("All out of Waves!"); //Might add more waves and a boss wave
                    break;
            }
            yield return new WaitForSeconds(10);
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

    void SpawnEnemy()
    {
        float randomXPosition = Random.Range(-10, 10);
        spawnPosition = new Vector3(randomXPosition, 8.5f, 0);
        GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}