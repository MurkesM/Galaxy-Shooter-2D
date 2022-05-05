using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject[] _commonPowerups;
    [SerializeField] GameObject[] _rarePowerups;
    [SerializeField] GameObject[] _extraRarePowerups;
    [SerializeField] GameObject[] _wave1;
    [SerializeField] GameObject[] _wave2;
    [SerializeField] GameObject[] _wave3;
    [SerializeField] GameObject[] _enemyPrefabs;
    int _currentWave = 1;
    int _randomEnemyPrefab;
    bool _stopSpawning = false;
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
                    foreach (GameObject enemy in _wave1)
                    {
                        SpawnEnemy();
                    }
                    break;
                case 2:
                    _currentWave++;
                    foreach (GameObject enemy in _wave2)
                    {
                        SpawnEnemy();
                    }
                    break;
                case 3:
                    foreach (GameObject enemy in _wave3)
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
            int powerupChance = Random.Range(0, 11);
            int randomCommonPowerup = Random.Range(0, _commonPowerups.Length);
            int randomRarePowerup = Random.Range(0, _rarePowerups.Length);
            int randomExtraRarePowerup = Random.Range(0, _extraRarePowerups.Length);

            switch (powerupChance)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6: //Spawns with a 60% chance
                    Instantiate(_commonPowerups[randomCommonPowerup], spawnPosition, Quaternion.identity);
                    break;
                case 7:
                case 8:
                case 9: //Spawns with a 30% chance
                    Instantiate(_rarePowerups[randomRarePowerup], spawnPosition, Quaternion.identity);
                    break;
                case 10: //Spawns with a 10% chance
                    Instantiate(_extraRarePowerups[randomExtraRarePowerup], spawnPosition, Quaternion.identity);
                    break;
                default:
                    Debug.Log("Default powerup value");
                    break;
            }

            yield return new WaitForSeconds(randomSpawnTime);
        }
    }

    void SpawnEnemy()
    {
        _randomEnemyPrefab = Random.Range(0, _enemyPrefabs.Length);
        float randomXPosition = Random.Range(-10, 10);
        spawnPosition = new Vector3(randomXPosition, 8.5f, 0);
        GameObject newEnemy = Instantiate(_enemyPrefabs[_randomEnemyPrefab], spawnPosition, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}