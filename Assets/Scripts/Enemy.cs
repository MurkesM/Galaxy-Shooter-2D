using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed = 4;
   
    Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -5.80f)
        {
            float randomXPosition = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomXPosition, 8, 0);
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

            Destroy(gameObject);
        }

        if (other.tag == "Laser")
        {
            _player.AddScore(10);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
