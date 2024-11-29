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
    public Action<Unit> OnShootBegin;

    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float aimingStateTime = 1.5f;
    [SerializeField] private float shootingStateTime = 0.2f;
    [SerializeField] private float coolingStateTime = 0.5f;
    [SerializeField] private int shootDamageAmount = 20;
    [SerializeField] private LayerMask unwalkableLayerMask;
    protected override void Awake()
    {
        base.Awake();
        actionCost = 1;
        //maxDistanceForActionExecution = 4;
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
        OnShootBegin?.Invoke(targetUnit);
        targetUnit.TakeDamage(shootDamageAmount);
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        canShootBullet = true;
        StartCoroutine(ActionStateFlow());
        base.TakeAction(gridPosition, onActionComplete);
    }
    public override string GetActionName()
    {
        return "Shoot";
    }
    //used for friendly units
    public override List<GridPosition> GetValidActionGridPositions()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositions(unitGridPosition);
    }
    //used for enemy units
    public List<GridPosition> GetValidActionGridPositions(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
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
                    {
                       
                        Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                        Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                        float unitShoulderOffset = 1.7f;
                        if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderOffset,
                            shootDir,
                            Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()), unwalkableLayerMask))
                        {
                            continue;
                        }
                        validGridPositions.Add(validGridPosition);
                    }
                }
            }
        }
        return validGridPositions;
    }
    public Unit GetTargetUnit()
    {
        return targetUnit;
    }
    public int GetActionRange()
    {
        return maxDistanceForActionExecution;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        int targetUnitHealth = targetUnit.GetUnitHealth();
        return new EnemyAIAction() { gridPosition = gridPosition, actionValue = 100 + (100 - targetUnitHealth)};
    }
    public int GetTargetCountAtGridPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositions(gridPosition).Count;
    }

}
