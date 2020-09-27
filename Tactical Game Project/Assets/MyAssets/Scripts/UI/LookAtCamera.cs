using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] Camera _camera;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(2 * transform.position - _camera.transform.position);
    }
}
