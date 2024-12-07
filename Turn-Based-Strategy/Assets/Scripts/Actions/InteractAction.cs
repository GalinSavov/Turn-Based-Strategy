using Game.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    protected override void Awake()
    {
        base.Awake();
        actionCost = 1;
    }
    private void Update()
    {
        if (!isActive) return;
    }
    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { actionValue = 0 ,gridPosition = gridPosition};
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

                Door door = LevelGrid.Instance.GetDoorAtGridPosition(validGridPosition);

                if(door == null) 
                    continue;

                validGridPositions.Add(validGridPosition);
            }
        }
        return validGridPositions;
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
        door.Interact();
        base.TakeAction(gridPosition, onActionComplete);
        StartCoroutine(WaitForDoorAnimation());

    }
    IEnumerator WaitForDoorAnimation()
    {
        yield return new WaitForSeconds(1f);
        base.FinishAction();
    }
}
