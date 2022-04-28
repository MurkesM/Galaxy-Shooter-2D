using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Animator _cameraAnim;

    void Start()
    {
        _cameraAnim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void CamShake()
    {
        _cameraAnim.SetTrigger("Shake");
    }
}