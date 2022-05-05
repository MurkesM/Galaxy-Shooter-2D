using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _enemySpeed = 4;
    [SerializeField] GameObject _enemyShotPrefab;
    [SerializeField] AudioClip _explosionClip;
    [SerializeField] AudioClip _laserClip;
    [SerializeField] Vector3 _laserOffset;
    //[SerializeField] bool _moveSideToSide = false;
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

        //if (_moveSideToSide == true)
           // StartCoroutine(MoveSideToSide());
    }

    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        //if(_moveSideToSide == true)
        //{
        //    if (transform.position.x == transform.position.x + 4)
        //      transform.Translate(Vector3.left * 50 * Time.deltaTime);
        //    else if (transform.position.x == transform.position.x - 4)
        //    transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
        //}

        if (transform.position.y <= -6.5f)
        {
            float randomXPosition = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomXPosition, 8.5f, 0);
        }

        //if (transform.position.x <= -10.23)
        //    transform.position = new Vector3(10.23f, transform.position.y, 0);

        //else if (transform.position.x >= 10.23)
        //    transform.position = new Vector3(-10.23f, transform.position.y, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }
            _player.AddScore(10);

            KillEnemy();

            Destroy(gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            _player.AddScore(10);

            KillEnemy();

            Destroy(other.gameObject);
            Destroy(gameObject, 2.8f);
        }

        if (other.tag == "HSMissile")
        {
            _player.AddScore(10);

            KillEnemy();

            Destroy(other.gameObject);
            Destroy(gameObject, 1.75f);
        }
    }

    void KillEnemy()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _enemyDead = true;
        _enemySpeed = 1.5f;
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

    //IEnumerator MoveSideToSide()
    //{
    //    while(_moveSideToSide == true)
    //    {
    //        Debug.Log("Test");
    //        transform.Translate(Vector3.right * 50 * Time.deltaTime);
    //        new WaitForSeconds(2);
    //        transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime);
    //        yield return new WaitForSeconds(2);
    //    }
    //}
}