using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    [SerializeField] private float bulletShakeIntensity = 0.5f;
    [SerializeField] private float grenadeShakeIntensity = 1f;
    private void Start()
    {
        ShootAction.OnAnyShootBegin += HandleExplosion;
        GrenadeProjectile.onAnyGrenadeExploded += HandleGrenade; 
    }

    private void HandleGrenade()
    {
        ScreenShake.Instance.Shake(grenadeShakeIntensity);
    }

    private void OnDisable()
    {
        ShootAction.OnAnyShootBegin -= HandleExplosion;
    }
    private void HandleExplosion(Unit unit)
    {
        ScreenShake.Instance.Shake(bulletShakeIntensity);
    }
}
