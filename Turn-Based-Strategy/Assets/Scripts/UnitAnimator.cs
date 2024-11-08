using Game.Actions;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator = null;
    [SerializeField] private GameObject bulletProjectile = null;
    [SerializeField] private Transform projectileShootingPoint = null;
    private MoveAction moveAction;
    private ShootAction shootAction;

    private void Awake()
    {
        if(TryGetComponent<MoveAction>(out moveAction))
        {
            moveAction.OnMoveStart += HandleOnMoveStart;
            moveAction.OnMoveEnd += HandleOnMoveEnd;
        }
        if(TryGetComponent<ShootAction>(out shootAction))
        {
            shootAction.OnShootBegin += HandleOnShootBegin;
        }
    }
    private void OnDisable()
    {
        moveAction.OnMoveStart -= HandleOnMoveEnd;
        moveAction.OnMoveEnd -= HandleOnMoveEnd;
        shootAction.OnShootBegin -= HandleOnShootBegin;
    }

    private void HandleOnShootBegin(Unit targetUnit)
    {
        unitAnimator.SetTrigger("Shoot");
        GameObject bulletProjectileGameObject = Instantiate(this.bulletProjectile, projectileShootingPoint.position,Quaternion.identity);
        Projectile projectile = bulletProjectileGameObject.GetComponent<Projectile>();
        Vector3 targetShootAtPosition = targetUnit.transform.position;
        targetShootAtPosition.y = projectileShootingPoint.position.y;
        projectile.Setup(targetShootAtPosition);
    }

    private void HandleOnMoveStart()
    {
        unitAnimator.SetBool("isRunning", true);
    }

    private void HandleOnMoveEnd()
    {
        unitAnimator.SetBool("isRunning", false);
    }

}
