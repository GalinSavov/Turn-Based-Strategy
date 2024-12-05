using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    [SerializeField] private float screenShakeIntensity = 0.5f;
    private void Start()
    {
        ShootAction.OnAnyShootBegin += HandleShootBegin;
    }
    private void OnDisable()
    {
        ShootAction.OnAnyShootBegin -= HandleShootBegin;
    }
    private void HandleShootBegin(Unit unit)
    {
        ScreenShake.Instance.Shake(screenShakeIntensity);
    }
}
