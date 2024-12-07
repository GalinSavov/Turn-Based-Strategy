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

                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(validGridPosition);

                if(interactable == null) 
                    continue;

                validGridPositions.Add(validGridPosition);
            }
        }
        return validGridPositions;
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
        interactable.Interact();
        base.TakeAction(gridPosition, onActionComplete);
        StartCoroutine(WaitForInteractable());

    }
    IEnumerator WaitForInteractable()
    {
        yield return new WaitForSeconds(1f);
        base.FinishAction();
    }
}
