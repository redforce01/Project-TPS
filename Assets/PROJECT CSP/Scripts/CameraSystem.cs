using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

namespace CSP
{
    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem Instance { get; private set; } = null;


        public GameObject playerCamera;
        public GameObject topviewCamera;

        public Transform topviewCameraPivot;
        public float topviewCameraSpeed = 5f;

        [Range(0f, 1f)] public float cornerHorizontal = 0.1f;
        [Range(0f, 1f)] public float cornerVertical = 0.1f;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                playerCamera.SetActive(true);
                topviewCamera.SetActive(false);
            }

            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                playerCamera.SetActive(false);
                topviewCamera.SetActive(true);
            }

            Vector3 forwardDir = Camera.main.transform.forward.normalized;
            Vector3 rightDir = Camera.main.transform.right.normalized;
            forwardDir.y = 0;
            rightDir.y = 0;

            if (Input.GetKey(KeyCode.W))
            {
                topviewCameraPivot.localPosition += forwardDir * topviewCameraSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S))
            {
                topviewCameraPivot.localPosition -= forwardDir * topviewCameraSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A))
            {
                topviewCameraPivot.localPosition -= rightDir * topviewCameraSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D))
            {
                topviewCameraPivot.localPosition += rightDir * topviewCameraSpeed * Time.deltaTime;
            }

            Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if (viewportPosition.x < cornerHorizontal && viewportPosition.y > (1 - cornerVertical))
            {
                // Top Left
                topviewCameraPivot.localPosition += forwardDir * topviewCameraSpeed * Time.deltaTime;
                topviewCameraPivot.localPosition -= rightDir * topviewCameraSpeed * Time.deltaTime;
            }
        }
    }
}
