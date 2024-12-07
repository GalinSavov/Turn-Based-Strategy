using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 followOffset;
    
    private const float MIN_FOLLOW_OFFSET = 4f;
    private const float MAX_FOLLOW_OFFSET = 12f;

    [SerializeField] private float cameraMoveSpeed = 2f;
    [SerializeField] private float cameraRotateSpeed = 2f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera = null;
   
    private void Awake()
    {  
        cinemachineTransposer = cinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
        followOffset = cinemachineTransposer.m_FollowOffset;
    }
    void Update()
    {
        MoveCamera();
        RotateCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        Vector3 moveVector = transform.forward * InputManager.Instance.GetCameraMovementInput().z + 
            transform.right * InputManager.Instance.GetCameraMovementInput().x;

        transform.position += moveVector * cameraMoveSpeed * Time.deltaTime;
    }
    private void RotateCamera()
    {
        transform.rotation *= Quaternion.Euler(InputManager.Instance.GetCameraRotationInput() * Time.deltaTime * cameraRotateSpeed); 
    }
    private void ZoomCamera()
    {
        followOffset.y += InputManager.Instance.GetCameraZoomInput();
        followOffset.y = Mathf.Clamp(followOffset.y, MIN_FOLLOW_OFFSET, MAX_FOLLOW_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
    }
}
