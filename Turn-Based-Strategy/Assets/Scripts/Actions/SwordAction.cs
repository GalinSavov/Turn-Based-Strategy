using Game.Grid;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    [SerializeField] private float beforeHitStateTime = 1f;
    [SerializeField] private float afterHitStateTime = 0.5f;
    [SerializeField] private int actionDamage = 75;
    [SerializeField] private float rotateSpeed = 5f;
    public Action onSwordActionStarted;
    public Action onSwordActionFinished;
    public static Action onAnySwordHit;

    private Coroutine beforeSwordHitCoroutine;
    private Unit targetUnit;
    private enum State
    {
        SwingingSwordBeforeHit,
        SwingSwordAfterHit,
    }
    private State state;
    private float stateTimer;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (!isActive)
            return;
    }
    private IEnumerator ActionStateFlow()
    {
        state = State.SwingingSwordBeforeHit;
        onSwordActionStarted?.Invoke();
        beforeSwordHitCoroutine = StartCoroutine(RotateAndAimAtTarget());
        yield return new WaitForSeconds(beforeHitStateTime);
        targetUnit.TakeDamage(actionDamage);
        onAnySwordHit?.Invoke();

        state = State.SwingSwordAfterHit;
        yield return new WaitForSeconds(afterHitStateTime);
        onSwordActionFinished?.Invoke();
        FinishAction();
    }
   
    private IEnumerator RotateAndAimAtTarget()
    {
        Vector3 direction = (targetUnit.transform.position - transform.position).normalized;
        while (Vector3.Angle(transform.forward, direction) > 0.1f)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * rotateSpeed);
            yield return null; // wait for next frame before looping again
        }
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        StartCoroutine(ActionStateFlow());
        base.TakeAction(gridPosition, onActionComplete);
    }
    
    public override string GetActionName()
    {
        return "Sword";
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { actionValue = 200, gridPosition = gridPosition };
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

               if(LevelGrid.Instance.GetUnitsAtGridPosition(validGridPosition).Count > 0)
               {
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(validGridPosition);
                    if (unit.IsEnemy() != targetUnit.IsEnemy())    
                         validGridPositions.Add(validGridPosition);
               }
            }
        }
        return validGridPositions;
    }
}
