using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCamera = null;

    private void Awake()
    {
        BaseAction.OnAnyActionStarted += HandleActionStarted;
        BaseAction.OnAnyActionFinished += HandleActionFinished;
    }
    private void OnDestroy()
    {
        BaseAction.OnAnyActionStarted -= HandleActionStarted;
        BaseAction.OnAnyActionFinished -= HandleActionFinished;
    }
    private void HandleActionStarted(BaseAction action)
    {
        switch (action)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeigth = Vector3.up * 1.7f;
                Vector3 shoulderOffset = new Vector3(0, 0, -0.5f);
                Vector3 shootDirection = (targetUnit.transform.position - shooterUnit.transform.position).normalized;

                actionCamera.transform.position = shooterUnit.transform.position + cameraCharacterHeigth + shoulderOffset +  shootDirection * -1;
                actionCamera.transform.LookAt(targetUnit.transform.position);
                ShowActionCamera();
                break;
            default:
                break;
        }
    }
    private void HandleActionFinished(BaseAction action)
    {
        switch (action)
        {
            case ShootAction _:
                HideActionCamera();
                break;
            default:
                break;
        }
    }
    private void ShowActionCamera()
    {
        actionCamera.SetActive(true);
    }
    private void HideActionCamera()
    {
        actionCamera.SetActive(false);
    }
}
