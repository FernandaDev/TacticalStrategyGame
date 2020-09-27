using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera _cam;
    Transform cameraTransform;
    Transform cameraArmTransform;
    [SerializeField] Vector3 mainCameraStartOffset;

    private void Awake()
    {
        if (_cam == null)
        {
            _cam = Camera.main;
            if (_cam == null)
                _cam = FindObjectOfType<Camera>();
        }

        cameraTransform = this.transform;
        cameraArmTransform = this.transform.parent;
    }

    private void Start()
    {
        transform.position += mainCameraStartOffset;
    }

    private void Update()
    {
        HandleCameraRotation();
        HandleCameraScroll();
    }

    [Space]
    [Header("Camera Rotation")]

    [SerializeField] float rotationSensitivity = 4f;
    [SerializeField] float orbitSpeed = 10f;
    [SerializeField] float minYRotation = 0;
    [SerializeField] float maxYRotation = 90f;
    Vector3 _mainCameraLocalRotation;
    bool canRotateCamera;

    public void HandleCameraRotation()
    {
        if (canRotateCamera)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _mainCameraLocalRotation.x += Input.GetAxis("Mouse X") * rotationSensitivity;
                _mainCameraLocalRotation.y -= Input.GetAxis("Mouse Y") * rotationSensitivity;

                _mainCameraLocalRotation.y = Mathf.Clamp(_mainCameraLocalRotation.y, minYRotation, maxYRotation);
            }
        }

        Quaternion newRotation = Quaternion.Euler(_mainCameraLocalRotation.y, _mainCameraLocalRotation.x, 0);

        cameraArmTransform.rotation = Quaternion.Lerp(cameraArmTransform.rotation, newRotation, Time.deltaTime * orbitSpeed);
    }

    [Space]
    [Header("Camera Scroll")]

    [SerializeField] float scrollSensitivity = 2f;
    [SerializeField] float scrollSpeedRelativeToDistance = 0.3f;
    [SerializeField] float scrollSpeed = 6f;
    [SerializeField] float minScrollDist = 2f;
    [SerializeField] float maxScrollDist = 15f;
    float _cameraDistance = 10f;
    bool canScrool;

    public void HandleCameraScroll()
    {
        if (canScrool)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

                scrollAmount *= (_cameraDistance * scrollSpeedRelativeToDistance);

                _cameraDistance += scrollAmount * -1f;

                _cameraDistance = Mathf.Clamp(_cameraDistance, minScrollDist, maxScrollDist);
            }

            if (cameraTransform.localPosition.z != _cameraDistance * -1f)
            {
                cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x,
                                                                  cameraTransform.localPosition.y,
                                                                  Mathf.Lerp(cameraTransform.localPosition.z, _cameraDistance * -1f, Time.deltaTime * scrollSpeed));
            }
        }
    }
}
