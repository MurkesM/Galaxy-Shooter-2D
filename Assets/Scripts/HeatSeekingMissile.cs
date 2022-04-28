using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeekingMissile : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5;
    [SerializeField] float _rotateSpeed = 100;

    Rigidbody2D _rigidbody2D;

    float _leftBounds = -11.5f;
    float _rightBounds = 11.5f;
    float _northBounds = 9;
    float _southBounds = -6.5f;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        FindClosestEnemy();

        if (transform.position.x <= _leftBounds || transform.position.x >= _rightBounds
            || transform.position.y >= _northBounds || transform.position.y <= _southBounds)
        {
            Destroy(gameObject);
        }
    }

    void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        //Calculates which enemy is the closest
        foreach (Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;

            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }
        //Moves missile to the closest enemy
        if (closestEnemy != null)
        {
             Vector2 targetDirection = (Vector2)closestEnemy.transform.position - _rigidbody2D.position;

             targetDirection.Normalize();

             float rotateAmount = Vector3.Cross(targetDirection, transform.up).z;

             _rigidbody2D.angularVelocity = -rotateAmount * _rotateSpeed;

             _rigidbody2D.velocity = transform.up * _moveSpeed;

            //Debug.DrawLine(this.transform.position, closestEnemy.transform.position);
        }
    }
}