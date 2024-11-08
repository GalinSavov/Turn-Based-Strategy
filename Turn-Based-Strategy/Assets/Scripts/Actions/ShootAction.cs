using Game.Grid;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    private enum State { Aiming,Shooting,Cooloff }
    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;
    private Coroutine rotateAndAimCoroutine;
    public Action OnShootBegin;

    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float aimingStateTime = 1.5f;
    [SerializeField] private float shootingStateTime = 0.2f;
    [SerializeField] private float coolingStateTime = 0.5f;
    protected override void Awake()
    {
        base.Awake();
        actionCost = 1;
        maxDistanceForActionExecution = 3;
    }
    void Update()
    {
        if (!isActive) { return; }
    }
    private IEnumerator ActionStateFlow()
    {
        state = State.Aiming;
        rotateAndAimCoroutine = StartCoroutine(RotateAndAimAtTarget());
        yield return new WaitForSeconds(aimingStateTime);

        state = State.Shooting;
        if(canShootBullet)
            canShootBullet = false;
        Shoot();
        yield return new WaitForSeconds(shootingStateTime);

        state = State.Cooloff;
        yield return new WaitForSeconds(coolingStateTime);
        FinishAction();
    }
    private IEnumerator RotateAndAimAtTarget()
    {
        Vector3 direction = (targetUnit.transform.position - transform.position).normalized;
        while(Vector3.Angle(transform.forward,direction) > 0.1f)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * rotateSpeed);
            yield return null; // wait for next frame before looping again
        }
    }
    private void Shoot()
    {
        OnShootBegin?.Invoke();
        targetUnit.Damage();
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        base.TakeAction(gridPosition, onActionComplete);
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        canShootBullet = true;
        StartCoroutine(ActionStateFlow());
    }
    public override string GetActionName()
    {
        return "Shoot";
    }
    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxDistanceForActionExecution; x <= maxDistanceForActionExecution; x++)
        {
            for (int z = -maxDistanceForActionExecution; z <= maxDistanceForActionExecution; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition validGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsGridPositionWithinBounds(validGridPosition))
                    continue;

                if (validGridPosition == unitGridPosition)
                    continue;

                if(LevelGrid.Instance.GetUnitsAtGridPosition(validGridPosition).Count > 0)
                {
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(validGridPosition);
                    if (this.unit.IsEnemy() != targetUnit.IsEnemy())
                        validGridPositions.Add(validGridPosition);
                }
            }
        }
        return validGridPositions;
    }
}
