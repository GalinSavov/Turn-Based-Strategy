using Game.Units;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    [SerializeField] private float bulletShakeIntensity = 0.5f;
    [SerializeField] private float grenadeShakeIntensity = 1f;
    [SerializeField] private float swordShakeIntensity = 1.25f;
    private void Start()
    {
        ShootAction.OnAnyShootBegin += HandleExplosion;
        GrenadeProjectile.onAnyGrenadeExploded += HandleGrenade;
        SwordAction.onAnySwordHit += HandleSwordHit;
    }
    private void HandleSwordHit()
    {
        ScreenShake.Instance.Shake(swordShakeIntensity);
    }
    private void HandleGrenade()
    {
        ScreenShake.Instance.Shake(grenadeShakeIntensity);
    }
    private void OnDisable()
    {
        ShootAction.OnAnyShootBegin -= HandleExplosion;
        GrenadeProjectile.onAnyGrenadeExploded -= HandleGrenade;
        SwordAction.onAnySwordHit -= HandleSwordHit;
    }
    private void HandleExplosion(Unit unit)
    {
        ScreenShake.Instance.Shake(bulletShakeIntensity);
    }
}
