using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] float _bossSpeed = 2;
    [SerializeField] GameObject _fireBallPrefab;
    [SerializeField] GameObject _energyBallsPrefab;
    [SerializeField] GameObject _enemyMinionsPrefab;
    [SerializeField] Vector3 _fireBallOffset;
    [SerializeField] Vector3 _minionsOffset;
    [SerializeField] Vector3 _explosionOffset;
    [SerializeField] int _maxHealth = 20;
    [SerializeField] int _currentHealth;
    [SerializeField] GameObject _explosionPrefab;

    [SerializeField] AudioClip _explosionSFX;
    [SerializeField] AudioClip _fireBallSFX;
    [SerializeField] AudioClip _energyBallSFX;

    GameObject _currentMinion;
    bool _canMove = true;
    bool _bossDead = false;
    bool _changeColor = false;
    bool _defaultFire = true;
    bool _canTakeDmg = false;
    Color _defaultColor = new Color(255, 255, 255, 255);
    Player _player;
    AudioSource _audioSource;
    SpriteRenderer _spriteRenderer;
    SpawnManager _spawnManager;
    UIManager _UIManager;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _currentHealth = _maxHealth;

        if (_player == null)
            Debug.Log("Player is null");
        if (_audioSource == null)
            Debug.Log("AudioSource is null");
        if (_spriteRenderer == null)
            Debug.Log("SpriteRenderer is null");
        if (_spawnManager == null)
            Debug.Log("SpawnManager is null");
        if (_UIManager == null)
            Debug.Log("UIManager is null");
    }

    void Update()
    {
        if (_canMove == true)
            transform.Translate(Vector2.down * _bossSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BossStopPoint")
        {
            _canMove = false;
            _canTakeDmg = true;
            _UIManager.BossHealthUI();
            StartCoroutine(StandardFireRoutine());
            SpawnMinions();
        }

        if (_canTakeDmg == true)
        {
            if (other.tag == "Laser")
            {
                Destroy(other.gameObject);
                TakeDamage();
                HandleShootingPatterns();
            }

            if (other.tag == "HSMissile")
            {
                Destroy(other.gameObject);
                TakeDamage();
                HandleShootingPatterns();
            }
        }
    }

    void HandleShootingPatterns()
    {
        switch (_currentHealth)
        {
            case 0:
                KillBoss();
                break;
            case 10:
                Debug.Log("Boss Health is 10");
                StartCoroutine(EnergyShotFireRoutine());
                break;
            default:
                Debug.Log("Default Value");
                break;
        }
    }

    void TakeDamage()
    {
        _currentHealth--;
        _UIManager.UpdateBossHealth();

        _changeColor = true;
        if (_changeColor == true)
            StartCoroutine(ChangeColor());
    }

    void KillBoss()
    {
        if (_player != null)
            _player.AddScore(2000);
        _bossDead = true;
        _spawnManager.OnBossDeath();
        _UIManager.BossIsDeadUI();
        Instantiate(_explosionPrefab, transform.position + _explosionOffset, Quaternion.identity);
        _audioSource.PlayOneShot(_explosionSFX);
        Destroy(_currentMinion);
        Destroy(gameObject, 2.5f);
    }

    IEnumerator StandardFireRoutine()
    {
        while (_defaultFire == true)
        {
            int randomFireTime = Random.Range(0, 5);
            GameObject newEnemyShot = Instantiate(_fireBallPrefab, transform.position + _fireBallOffset, Quaternion.identity);
            newEnemyShot.transform.parent = this.transform;
            _audioSource.PlayOneShot(_fireBallSFX);
            yield return new WaitForSeconds(randomFireTime);
        }
    }

    IEnumerator EnergyShotFireRoutine()
    {
        while (_bossDead == false)
        {
            _defaultFire = false;
            GameObject newEnemyShot = Instantiate(_energyBallsPrefab, transform.position + _fireBallOffset, Quaternion.identity);
            newEnemyShot.transform.parent = this.transform;
            _audioSource.PlayOneShot(_energyBallSFX);
            yield return new WaitForSeconds(0.8f);
        }
    }

    void SpawnMinions()
    {
         _currentMinion = Instantiate(_enemyMinionsPrefab, transform.position + _minionsOffset, Quaternion.identity);
    }

    IEnumerator ChangeColor()
    {
        while (_changeColor == true)
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            _changeColor = false;
            _spriteRenderer.color = _defaultColor;
        }
    }
}