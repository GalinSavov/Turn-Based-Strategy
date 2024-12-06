using Game.Grid;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private GrenadeProjectile grenadeProjectile = null;
    protected override void Awake()
    { 
        base.Awake();
        actionCost = 1;
    }
    public override string GetActionName()
    {
        return "Grenade";
    }
    private void Update()
    {
        if (!isActive)
            return;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { actionValue = 0, gridPosition = gridPosition };
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

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxDistanceForActionExecution)
                    continue;

                if(LevelGrid.Instance.GetUnitsAtGridPosition(validGridPosition).Count > 0)
                {
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(validGridPosition);
                    if (!targetUnit.IsEnemy())
                        continue;
                } 
                validGridPositions.Add(validGridPosition);  
            }
        }
        return validGridPositions;
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        GrenadeProjectile grenade = Instantiate(grenadeProjectile, unit.GetWorldPosition(), Quaternion.identity);
        grenade.Setup(gridPosition, OnGrenadeBehaviourComplete);
        base.TakeAction(gridPosition,onActionComplete);
    }
    private void OnGrenadeBehaviourComplete()
    {
        FinishAction();
    }
}
