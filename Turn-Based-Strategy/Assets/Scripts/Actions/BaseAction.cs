using Game.Grid;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static Action<BaseAction> OnAnyActionStarted;
    public static Action<BaseAction> OnAnyActionFinished;
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;
    protected int actionCost;
    [SerializeField] protected int maxDistanceForActionExecution; // exceeding this number of grids will make the action out of range

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
    public virtual void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        unit.UnitActionPoints -= actionCost;
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStarted?.Invoke(this);
    }
    protected void FinishAction()
    {
        isActive = false;
        onActionComplete();
        OnAnyActionFinished?.Invoke(this);
    }
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositions();
        return validGridPositions.Contains(gridPosition);
    }
    public Unit GetUnit() { return unit; }
    public abstract string GetActionName();
    public int GetActionCost() { return actionCost; }
    public abstract List<GridPosition> GetValidActionGridPositions();


}
