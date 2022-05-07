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
    [SerializeField] int _bossHealth = 20;

    Player _player;
    bool _canMove = true;
    bool _bossDead = false;
    bool _changeColor = false;
    bool _defaultFire = true;
    bool _fireEnergyShot = false;
    Color _defaultColor = new Color(255, 255, 255, 255);
    AudioSource _audioSource;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
       
        if (_player == null)
            Debug.Log("Player is null");
        if (_audioSource == null)
            Debug.Log("AudioSource is null");
        if (_spriteRenderer == null)
            Debug.Log("SpriteRenderer is null");

        StartCoroutine(StandardFireRoutine());
        StartCoroutine(SpawnMinionsRoutine());
    }

    void Update()
    {
        if (_canMove == true)
            transform.Translate(Vector2.down * _bossSpeed * Time.deltaTime);

        if (transform.position.y <= 4.56)
            _canMove = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _bossHealth--;
            Destroy(other.gameObject);

            _changeColor = true;
            if (_changeColor == true)
              StartCoroutine(ChangeColor());

            switch (_bossHealth)
            {
                case 0:
                    if (_player != null)
                        _player.AddScore(2000);
                    Destroy(gameObject);
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    StartCoroutine(EnergyShotFireRoutine());
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                default:
                    Debug.Log("Default Value");
                    break;
            }

            if (_bossHealth <= 0)
            {
                
            }
        }

        if (other.tag == "HSMissile")
        {
            _bossHealth--;
            Destroy(other.gameObject);

            _changeColor = true;
            if (_changeColor == true)
                StartCoroutine(ChangeColor());

            if (_bossHealth <= 0)
            {
                if (_player != null)
                    _player.AddScore(2000);

                Destroy(gameObject);
            }
        }
    }

    IEnumerator StandardFireRoutine()
    {
        while (_bossDead == false)
        {
            while (_defaultFire == true)
            {
                int randomFireTime = Random.Range(0, 5);
                GameObject newEnemyShot = Instantiate(_fireBallPrefab, transform.position + _fireBallOffset, Quaternion.identity);
                newEnemyShot.transform.parent = this.transform;
                //_audioSource.PlayOneShot(_laserClip);
                yield return new WaitForSeconds(randomFireTime);
            }
        }
    }

    IEnumerator EnergyShotFireRoutine()
    {
        while (_bossDead == false)
        {
            while (_fireEnergyShot == true)
            {
                GameObject newEnemyShot = Instantiate(_energyBallsPrefab, transform.position + _fireBallOffset, Quaternion.identity);
                newEnemyShot.transform.parent = this.transform;
                //_audioSource.PlayOneShot(_laserClip);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator SpawnMinionsRoutine()
    {
        while (_bossDead == false)
        {
            yield return new WaitForSeconds(3);
            Instantiate(_enemyMinionsPrefab, transform.position + _minionsOffset, Quaternion.identity);
            yield return new WaitForSeconds(8);
        }
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
