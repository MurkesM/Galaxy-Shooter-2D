using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _enemySpeed = 4;
    [SerializeField] float _horizontalSpeed = 2;
    [SerializeField] bool _moveHorizontaly = false;
    [SerializeField] bool _moveRight = false;
    [SerializeField] GameObject _enemyShotPrefab;
    [SerializeField] AudioClip _explosionClip;
    [SerializeField] AudioClip _laserClip;
    [SerializeField] Vector3 _laserOffset;
    [SerializeField] bool _shieldActive = false;
    [SerializeField] GameObject _shieldVisualizer;
    AudioSource _audioSource;
    

    Player _player;
    Animator _animator;
    Collider2D _collider2D;
    bool _enemyDead = false;
    
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
            Debug.Log("Player is null");
        if (_animator == null)
            Debug.Log("Animator is null");
        if (_audioSource == null)
            Debug.Log("Audio Source is null");

        StartCoroutine(EnemyFireRoutine());

        if (_moveHorizontaly == true)
            StartCoroutine(SwitchDirectionRoutine());

        if (_shieldActive == true)
            _shieldVisualizer.SetActive(true);
        else if (_shieldActive == false)
            _shieldVisualizer.SetActive(false);
    }

    void Update()
    {
        if (_moveHorizontaly == true)
            MoveHorizontaly();

        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -6.5f)
        {
            float randomXPosition = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomXPosition, 8.5f, 0);
        }

        if (transform.position.x <= -10.23)
            transform.position = new Vector3(10.23f, transform.position.y, 0);

        else if (transform.position.x >= 10.23)
            transform.position = new Vector3(-10.23f, transform.position.y, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_shieldActive == true)
            {
                _shieldActive = false;
                _shieldVisualizer.SetActive(false);
            }
            else if (_shieldActive == false)
            {
                if (_player != null)
                {
                    _player.Damage();
                }
                _player.AddScore(10);

                KillEnemy();

                Destroy(gameObject, 2.8f);
            }
        }

        if (other.tag == "Laser")
        {
            if (_shieldActive == true)
            {
                _shieldActive = false;
                _shieldVisualizer.SetActive(false);
            }
            else if (_shieldActive == false)
            {
                _player.AddScore(10);

                KillEnemy();

                Destroy(other.gameObject);
                Destroy(gameObject, 2.8f);
            }
        }

        if (other.tag == "HSMissile")
        {
            if (_shieldActive == true)
            {
                _shieldActive = false;
                _shieldVisualizer.SetActive(false);
            }
            else if (_shieldActive == false)
            {
                _player.AddScore(10);

                KillEnemy();

                Destroy(other.gameObject);
                Destroy(gameObject, 1.75f);
            }
        }
    }

    void KillEnemy()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _enemyDead = true;
        _enemySpeed = 1.5f;
        _horizontalSpeed = .5f;
        _collider2D.enabled = false;
        _audioSource.PlayOneShot(_explosionClip);
    }

    IEnumerator EnemyFireRoutine()
    {
        while (_enemyDead == false)
        {
            int randomFireTime = Random.Range(3, 7);
            GameObject newEnemyShot = Instantiate(_enemyShotPrefab, transform.position + _laserOffset, Quaternion.identity);
            newEnemyShot.transform.parent = this.transform;
            _audioSource.PlayOneShot(_laserClip);
            yield return new WaitForSeconds(randomFireTime);
        }
    }

    void MoveHorizontaly()
    {
        if (_moveRight == true)
            transform.Translate(new Vector3(1, 0, 0) * _horizontalSpeed * Time.deltaTime);
        
        else if (_moveRight == false) //move left
            transform.Translate(new Vector3(-1, 0, 0) * _horizontalSpeed * Time.deltaTime);
    }

    IEnumerator SwitchDirectionRoutine()
    {
        while (_moveHorizontaly == true)
        {
            _moveRight = true;
            yield return new WaitForSeconds(3);
            _moveRight = false; //move left
            yield return new WaitForSeconds(3);
        }
    }
}
