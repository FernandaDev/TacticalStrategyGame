using UnityEngine;

namespace FernandaDev
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera _cam;
        [SerializeField] Vector3 startRotationOffset;
        [SerializeField] float startCameraDistance = 15f;
        [SerializeField] float cameraFOV = 50f;
        Transform cameraTransform;
        Transform cameraArmTransform;

        private void Awake()
        {
            if (_cam == null)
            {
                _cam = Camera.main;
                if (_cam == null)
                    _cam = FindObjectOfType<Camera>();
            }

            cameraTransform = transform;
            cameraArmTransform = transform.parent;
        }

        private void Start()
        {
            cameraLocalRotation = startRotationOffset;
            cameraDistance = startCameraDistance;
            _cam.fieldOfView = cameraFOV;
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(1))
                canRotateCamera = true;
            if (Input.GetMouseButtonUp(1))
                canRotateCamera = false;

            HandleCameraRotation();
            HandleCameraScroll();
        }

        [Space]
        [Header("Camera Rotation")]

        [SerializeField] float rotationSensitivity = 4f;
        [SerializeField] float orbitSpeed = 10f;
        [SerializeField] float minYRotation = 0;
        [SerializeField] float maxYRotation = 90f;
        Vector3 cameraLocalRotation;
        bool canRotateCamera;

        public void HandleCameraRotation()
        {
            if (canRotateCamera)
            {
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    cameraLocalRotation.x += Input.GetAxis("Mouse X") * rotationSensitivity;
                    cameraLocalRotation.y -= Input.GetAxis("Mouse Y") * rotationSensitivity;

                    cameraLocalRotation.y = Mathf.Clamp(cameraLocalRotation.y, minYRotation, maxYRotation);
                }
            }

            Quaternion newRotation = Quaternion.Euler(cameraLocalRotation.y, cameraLocalRotation.x, 0);

            cameraArmTransform.rotation = Quaternion.Lerp(cameraArmTransform.rotation, newRotation, Time.deltaTime * orbitSpeed);
        }

        [Space]
        [Header("Camera Scroll")]

        [SerializeField] float scrollSensitivity = 2f;
        [SerializeField] float scrollSpeedRelativeToDistance = 0.3f;
        [SerializeField] float scrollSpeed = 6f;
        [SerializeField] float minScrollDist = 10f;
        [SerializeField] float maxScrollDist = 20f;
        float cameraDistance = 10f;
        bool canScrool = true;

        public void HandleCameraScroll()
        {
            if (canScrool)
            {
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

                    scrollAmount *= cameraDistance * scrollSpeedRelativeToDistance;

                    cameraDistance += scrollAmount * -1f;

                    cameraDistance = Mathf.Clamp(cameraDistance, minScrollDist, maxScrollDist);
                }
            }

            if (cameraTransform.localPosition.z != cameraDistance * -1f)
            {
                cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x,
                                                            cameraTransform.localPosition.y,
                                                            Mathf.Lerp(cameraTransform.localPosition.z,
                                                                       cameraDistance * -1f, Time.deltaTime * scrollSpeed));
            }

        }
    }
}