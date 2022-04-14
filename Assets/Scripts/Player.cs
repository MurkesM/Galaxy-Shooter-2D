using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private int _playerLives = 3;
    [SerializeField] private float _playerSpeed = 3.5f;
    [SerializeField] private float _fireRate = 0.15f;
    private float _canFire = -1f;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _canFire)
            ShootLaser();
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(_playerSpeed * Time.deltaTime * direction);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.96f, 0), 0);

        if (transform.position.x <= -11.4f)
            transform.position = new Vector3(11.4f, transform.position.y, 0);
        
        else if (transform.position.x >= 11.4f)
            transform.position = new Vector3(-11.4f, transform.position.y, 0);
    }

    void ShootLaser()
    {
        Vector3 laserOffset = new Vector3(0, 0.8f, 0);
               
        Instantiate(_laserPrefab, transform.position + laserOffset, Quaternion.identity);
        _canFire = Time.time + _fireRate;
    }

    public void Damage()
    {
        _playerLives--;

        if (_playerLives < 1)
            Destroy(gameObject);
    }
}