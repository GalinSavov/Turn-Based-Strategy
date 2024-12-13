using Game.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float degrees = 0;
    protected override void Awake()
    {
        base.Awake();
        actionCost = 1;
    }
    void Update()
    {
        if (!isActive) { return; }

        float spinAmount = 360 * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        degrees += spinAmount;
        if (degrees >= 360)
        {
            degrees = 0;
            FinishAction();
        }
    }
    public override string GetActionName()
    {
        return "Spin";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        base.TakeAction(gridPosition, onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return  new List<GridPosition> { unitGridPosition };
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction() { gridPosition = gridPosition, actionValue = 0 };
    }
}
