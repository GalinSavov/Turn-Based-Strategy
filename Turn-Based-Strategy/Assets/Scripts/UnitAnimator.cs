using Game.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator = null;
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

    private void HandleOnShootBegin()
    {
        unitAnimator.SetTrigger("Shoot");
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
