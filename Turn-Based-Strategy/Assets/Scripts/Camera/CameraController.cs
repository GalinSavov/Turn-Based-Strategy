using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 followOffset;
    private TurnBasedStrategy inputActions;

    private const float MIN_FOLLOW_OFFSET = 4f;
    private const float MAX_FOLLOW_OFFSET = 12f;

    [SerializeField] private float cameraMoveSpeed = 2f;
    [SerializeField] private float cameraRotateSpeed = 2f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera = null;
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Awake()
    {
        inputActions = new TurnBasedStrategy();
        cinemachineTransposer = cinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
        followOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    void Update()
    {
        MoveCamera();
        RotateCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        Vector3 inputMoveDirection = Vector3.zero;
        inputMoveDirection += inputActions.Camera.CameraMove.ReadValue<Vector3>();

        Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveVector * cameraMoveSpeed * Time.deltaTime;
    }
    private void RotateCamera()
    {
        Vector3 moveRotation = Vector3.zero;
        if (inputActions.Camera.CameraRotateLeft.IsPressed())
            moveRotation.y += 1f;
        else if (inputActions.Camera.CameraRotateRight.IsPressed())
            moveRotation.y -= 1f;

        transform.rotation *= Quaternion.Euler(moveRotation * Time.deltaTime * cameraRotateSpeed);
        
    }
    private void ZoomCamera()
    {
        if (Mouse.current.scroll.value.y > 0)
            followOffset.y += zoomSpeed;

        else if(Mouse.current.scroll.value.y < 0)
            followOffset.y -= zoomSpeed;
 
        followOffset.y = Mathf.Clamp(followOffset.y, MIN_FOLLOW_OFFSET, MAX_FOLLOW_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
    }
}
