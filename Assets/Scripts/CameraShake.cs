using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Animator _cameraAnim;

    void Start()
    {
        _cameraAnim = GetComponent<Animator>();
    }

    public void CamShake()
    {
        _cameraAnim.SetTrigger("Shake");
    }
}
