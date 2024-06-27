using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    
    private TurnBasedStrategy inputActions;
    [SerializeField] private float cameraMoveSpeed = 2f;
    [SerializeField] private float cameraRotateSpeed = 2f;
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Awake()
    {
        inputActions = new TurnBasedStrategy();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    void Update()
    {
        Vector3 inputMoveDirection = Vector3.zero;
        inputMoveDirection += inputActions.Camera.CameraMove.ReadValue<Vector3>();

        Vector3 moveVector = Vector3.forward * inputMoveDirection.z + Vector3.right * inputMoveDirection.x;
        transform.position += moveVector * cameraMoveSpeed * Time.deltaTime;

        Vector3 moveRotation = Vector3.zero;
        if (inputActions.Camera.CameraRotateLeft.IsPressed())
            moveRotation.y += 1f;
        else if(inputActions.Camera.CameraRotateRight.IsPressed()) 
            moveRotation.y -= 1f;

        transform.rotation *= Quaternion.Euler(moveRotation * Time.deltaTime * cameraRotateSpeed);
    }
}
