using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed = 4;
    Player _player;
    Animator _animator;
    Collider2D _collider2D;

    private AudioSource _audioSource;
    [SerializeField] AudioClip _explosionClip;
    private void Start()
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
    }

    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -6.5f)
        {
            float randomXPosition = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomXPosition, 8.5f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _collider2D.enabled = false;
            _enemySpeed = 1.5f;
            _audioSource.PlayOneShot(_explosionClip);
            Destroy(gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            _player.AddScore(10);
            _animator.SetTrigger("OnEnemyDeath");
            _collider2D.enabled = false;
            _enemySpeed = 1.5f;
            _audioSource.PlayOneShot(_explosionClip);
            Destroy(other.gameObject);
            Destroy(gameObject, 2.8f);
        }
    }
}
