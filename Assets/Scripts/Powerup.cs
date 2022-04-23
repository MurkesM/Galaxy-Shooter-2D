using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private int powerupID;//0 = TripleShot, 1 = Speed, 2 = Shield, 3 = Ammo
    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6.5f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        _player.TripleShotActive();
                        break;
                    case 1:
                        _player.SpeedActive();
                         break;
                    case 2:
                        _player.ShieldActive();
                         break;
                    case 3:
                        _player.RefillAmmo();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
