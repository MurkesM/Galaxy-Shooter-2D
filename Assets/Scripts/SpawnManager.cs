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
    [SerializeField] GameObject[] _bossWave;
    [SerializeField] GameObject[] _enemyPrefabs;
    [SerializeField] GameObject _bossPrefab;
    
    int _currentWave = 1;
    int _randomEnemyPrefab;
    bool _stopSpawning = false;
    bool _spawnBoss = true;
    Vector3 spawnPosition;

    void Update()
    {
        
    }

    public void StartSpawning()
    {
        _stopSpawning = false;
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
                        yield return new WaitForSeconds(1);
                    }
                    break;
                case 2:
                    _currentWave++;
                    foreach (GameObject enemy in _wave2)
                    {
                        SpawnEnemy();
                        yield return new WaitForSeconds(1);
                    }
                    break;
                case 3:
                    _currentWave++;
                    foreach (GameObject enemy in _wave3)
                    {
                        SpawnEnemy();
                        yield return new WaitForSeconds(1);
                    }
                    break;
                case 4:
                    SpawnBoss();
                    _spawnBoss = false;
                    yield return new WaitForSeconds(2);
                    foreach (GameObject enemy in _bossWave)
                    {
                        SpawnEnemy();
                    }
                    //When the boss gets killed the wave will increment by 1 again
                    break;
                case 5:
                    foreach (GameObject enemy in _wave3)
                    {
                        SpawnEnemy();
                        yield return new WaitForSeconds(1);
                    }
                    break;
                default:
                    Debug.Log("All out of Waves!"); 
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
        if (_stopSpawning == false)
        {
            _randomEnemyPrefab = Random.Range(0, _enemyPrefabs.Length);
            float randomXPosition = Random.Range(-10, 10);
            spawnPosition = new Vector3(randomXPosition, 8.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefabs[_randomEnemyPrefab], spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
    }

    void SpawnBoss()
    {
        if (_spawnBoss == true && _stopSpawning == false)
        {
            spawnPosition = new Vector3(0, 11, 0);
            Instantiate(_bossPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void StopSpawning()
    {
        _stopSpawning = true;
    }

    public void OnBossDeath()
    {
        _currentWave++;
    }
}