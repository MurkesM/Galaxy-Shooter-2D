using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 40;
    [SerializeField] GameObject _explosionPrefab;
    SpawnManager _spawnManager;
    Collider2D _collider2D;

    private AudioSource _audioSource;
    [SerializeField] AudioClip _explosionClip;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _collider2D = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
            Debug.Log("SpawnManager is null");
        if (_collider2D == null)
            Debug.Log("Collider2D is null");
        if (_audioSource == null)
            Debug.Log("Audio Source is null");
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _collider2D.enabled = false;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_explosionClip);
            _spawnManager.StartSpawning();
            Destroy(other.gameObject);
            Destroy(gameObject, 1.5f);
        }
    }
}