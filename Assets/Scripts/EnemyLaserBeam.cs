using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserBeam : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2);
    }

    void Update()
    {
        if (transform.parent == null)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player _player = other.GetComponent<Player>();

            if (_player != null)
            {
                _player.Damage();
            }
        }
    }
}
