using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject _laserPrefab;
    [SerializeField] GameObject _tripleShotPrefab;
    [SerializeField] GameObject _shieldVisualizer;
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] GameObject _rightFire;
    [SerializeField] GameObject _leftFire;
    [SerializeField] GameObject _thruster;

    [SerializeField] int _playerLives = 3;
    [SerializeField] int _ammo = 15;
    [SerializeField] float _playerSpeed = 7f;
    [SerializeField] bool _tripleShotActive = false;
    [SerializeField] bool _speedPowerupActive = false;
    [SerializeField] bool _shieldActive = false;
    [SerializeField] int _shieldHitCount = 3;
    [SerializeField] bool _canMove = true;

    float _fireRate = 0.15f;
    float _canFire = -1f;
    int _score;

    SpawnManager _spawnManager;
    UIManager _uiManager;
    Collider2D _collider2D;
    SpriteRenderer _shieldSpriteRenderer;

    AudioSource _audioSource;
    [SerializeField] AudioClip _laserClip;
    [SerializeField] AudioClip _explosionClip;
    [SerializeField] AudioClip _powerupClip;
    [SerializeField] AudioClip _noAmmo; 

    void Start()
    {
        transform.position = new Vector3(0, -3, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _collider2D = GetComponent<Collider2D>();
        _shieldSpriteRenderer = _shieldVisualizer.GetComponent<SpriteRenderer>();

        if (_spawnManager == null)
            Debug.Log("SpawnManager is null");
        if (_uiManager == null)
            Debug.Log("UIManager is null");
        if (_audioSource == null)
            Debug.Log("Audio Source is null");
        if (_shieldSpriteRenderer == null)
            Debug.Log("Shield SpriteRenderer is null");

        _rightFire.SetActive(false);
        _leftFire.SetActive(false);
        _thruster.SetActive(false);
        _shieldVisualizer.SetActive(false);
    }

    void Update()
    {
        if (_canMove == true)
            HandleMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _canFire && _ammo > 0)
            ShootLaser();
       
        else if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _canFire && _ammo <= 0)
            _audioSource.PlayOneShot(_noAmmo);
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(_playerSpeed * Time.deltaTime * direction);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.96f, 0), 0);

        if (_speedPowerupActive == true && Input.GetKey(KeyCode.LeftShift))
        {
            _playerSpeed = 12;
            _thruster.SetActive(true);
        }
        else
        {
            _playerSpeed = 7;
            _thruster.SetActive(false);
        }

        if (transform.position.x <= -11.4f)
            transform.position = new Vector3(11.4f, transform.position.y, 0);
        
        else if (transform.position.x >= 11.4f)
            transform.position = new Vector3(-11.4f, transform.position.y, 0);
    }

    void ShootLaser()
    {
        if (_tripleShotActive == false)
        {
            Vector3 laserOffset = new Vector3(0, 0.8f, 0);

            Instantiate(_laserPrefab, transform.position + laserOffset, Quaternion.identity);
            _canFire = Time.time + _fireRate;
            _ammo--;
            _uiManager.UpdateAmmo(_ammo);
        }
        else if (_tripleShotActive == true)
        {
            Vector3 laserOffset = new Vector3(0, 0.8f, 0);

            Instantiate(_tripleShotPrefab, transform.position + laserOffset, Quaternion.identity);
            _canFire = Time.time + _fireRate;
            _ammo--;
            _uiManager.UpdateAmmo(_ammo);
        }

        _audioSource.PlayOneShot(_laserClip);
    }

    public void Damage()
    {
        if (_shieldActive == true)
        {
            _shieldHitCount--;
            DamageShield();
        }
        else if(_shieldActive == false)
        {
            _playerLives--;

            HandlePlayerHurtAnimations();

            _uiManager.UpdateLiveSprites(_playerLives);

            if (_playerLives <= 0)
            {
                KillPlayer();
            }
        }
    }

    void KillPlayer()
    {
        _canMove = false;
        _collider2D.enabled = false;
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _spawnManager.OnPlayerDeath();
        _audioSource.PlayOneShot(_explosionClip);
        Destroy(gameObject, 1.75f);
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        _audioSource.PlayOneShot(_powerupClip);
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedActive()
    {
        _audioSource.PlayOneShot(_powerupClip);
        StartCoroutine(SpeedPowerDownRoutine());
        _speedPowerupActive = true;
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        _shieldHitCount = 3;
        _shieldSpriteRenderer.color = Color.cyan;
        _audioSource.PlayOneShot(_powerupClip);
        _shieldVisualizer.SetActive(true);
    }

    public void RefillAmmo()
    {
        _ammo = 15;
        _uiManager.UpdateAmmo(_ammo);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _tripleShotActive = false;
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _playerSpeed = 7;
        _speedPowerupActive = false;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    void HandlePlayerHurtAnimations()
    {
        switch (_playerLives)
        {
            case 2:
                _rightFire.SetActive(true);
                break;
            case 1:
                _leftFire.SetActive(true);
                break;
        }
    }
     void DamageShield()
    {
        switch (_shieldHitCount)
        {
            case 2:
                _shieldSpriteRenderer.color = Color.magenta;
                break;
            case 1:
                _shieldSpriteRenderer.color = Color.red;
                break;
            case 0:
                _shieldVisualizer.SetActive(false);
                _shieldActive = false;
                break;
        }
    }
}