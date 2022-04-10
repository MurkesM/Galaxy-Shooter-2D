using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   [SerializeField] private float _playerSpeed = 3.5f;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _playerSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.96f, 0), 0);

        if (transform.position.x <= -11.4f)
            transform.position = new Vector3(11.4f, transform.position.y, 0);
        else if (transform.position.x >= 11.4f)
            transform.position = new Vector3(-11.4f, transform.position.y, 0);
    }
}
