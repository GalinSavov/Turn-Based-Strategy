using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerInputActions playerInputActions;

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerInputActions = new PlayerInputActions();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }
    public Vector2 GetMouseScreenPosition()
    {
        return Mouse.current.position.ReadValue();
    }
    public bool IsLeftMouseButtonPressed()
    {
        return Mouse.current.leftButton.isPressed;
    }
    public float GetMouseScrollYValue()
    {
        return Mouse.current.scroll.value.y;
    }
    public Vector3 GetCameraRotationInput()
    {
        Vector3 moveRotation = Vector3.zero;
        if (playerInputActions.Camera.CameraRotateLeft.IsPressed())
            moveRotation.y += 1f;
        else if (playerInputActions.Camera.CameraRotateRight.IsPressed())
            moveRotation.y -= 1f;

        return moveRotation;
    }
    public Vector3 GetCameraMovementInput()
    {
        Vector3 inputMoveDirection = Vector3.zero;
        inputMoveDirection += playerInputActions.Camera.CameraMove.ReadValue<Vector3>();

        return inputMoveDirection;
    }
    public float GetCameraZoomInput()
    {
        float zoom = 0f;
        if (GetMouseScrollYValue() > 0)
            zoom = 2f;

        else if (InputManager.Instance.GetMouseScrollYValue() < 0)
            zoom = -2f;

        return zoom;
    }
}
