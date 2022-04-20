using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private int _playerLives = 3;
    [SerializeField] private float _playerSpeed = 7f;
    [SerializeField] private float _fireRate = 0.15f;
    [SerializeField] private bool _tripleShotActive = false;
    [SerializeField] private bool _shieldActive = false;
    [SerializeField] private GameObject _shieldVisualizer;

    [SerializeField] GameObject _rightFire;
    [SerializeField] GameObject _leftFire;

    private float _canFire = -1f;
    private SpawnManager _spawnManager;
    protected UIManager _uiManager;

    [SerializeField] private int _score;

    void Start()
    {
        transform.position = new Vector3(0, -3, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
            Debug.Log("SpawnManager is null");
        if (_uiManager == null)
            Debug.Log("UIManager is null");

        _rightFire.SetActive(false);
        _leftFire.SetActive(false);
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
        if (_tripleShotActive == false)
        {
            Vector3 laserOffset = new Vector3(0, 0.8f, 0);

            Instantiate(_laserPrefab, transform.position + laserOffset, Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
        else if (_tripleShotActive == true)
        {
            Vector3 laserOffset = new Vector3(0, 0.8f, 0);

            Instantiate(_tripleShotPrefab, transform.position + laserOffset, Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
    }

    public void Damage()
    {
        if (_shieldActive == true)
        {
            _shieldVisualizer.SetActive(false);
            _shieldActive = false;
        }
        else if(_shieldActive == false)
        {
            _playerLives--;

            HandlePlayerHurtAnimations();

            _uiManager.UpdateLiveSprites(_playerLives);

            if (_playerLives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(gameObject);
            }
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedActive()
    {
        _playerSpeed = 12;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        _shieldVisualizer.SetActive(true);
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
}