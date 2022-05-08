using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    [SerializeField] float _speed = 8;

    void Update()
    {
        //Moves Away from parent object using negative speed
        transform.position = Vector2.MoveTowards(transform.position,transform.parent.position, -_speed * Time.deltaTime);

        if (transform.position.y <= -6.5f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player _player = other.GetComponent<Player>();

            if (_player != null)
            {
                _player.Damage();
            }

            Destroy(gameObject);
        }
    }
}
