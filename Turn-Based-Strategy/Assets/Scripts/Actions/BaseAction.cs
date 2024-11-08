using Game.Grid;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;
    protected int actionCost;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
    public abstract string GetActionName();
    public int GetActionCost() { return actionCost; }

    public virtual void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        unit.UnitActionPoints -= actionCost;
        isActive = true;
        this.onActionComplete = onActionComplete;
    }
    public abstract List<GridPosition> GetValidActionGridPositions();

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositions();
        return validGridPositions.Contains(gridPosition);
    }

}
