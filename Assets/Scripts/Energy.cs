using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField] float _speed = 8;

    void Update()
    {
        //Moves Backwards using negative speed
        transform.position = Vector2.MoveTowards(transform.position,transform.parent.position, -_speed * Time.deltaTime);
    }
}
