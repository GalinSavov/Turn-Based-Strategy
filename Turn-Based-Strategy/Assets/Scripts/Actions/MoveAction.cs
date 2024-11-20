using Game.Core;
using Game.Grid;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Actions
{
    public class MoveAction : BaseAction
    {
        public Action OnMoveStart;
        public Action OnMoveEnd;
        private Vector3 targetPosition;
        private float moveSpeed = 2f;
        private float rotateSpeed = 10f;
        private float stoppingDistance = 0.1f;

 
        protected override void Awake()
        {
            base.Awake();
            targetPosition = transform.position;
            actionCost = 1;
            maxDistanceForActionExecution = 2;
        }

        void Update()
        {
            if (!isActive) { return; }

            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
            else
            {
                OnMoveEnd?.Invoke();
                FinishAction();
            }
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

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

                    if (LevelGrid.Instance.GetUnitsAtGridPosition(validGridPosition).Count > 0)
                        continue;
 
                     validGridPositions.Add(validGridPosition);
                }
            }
            return validGridPositions;
        }

        public override string GetActionName()
        {
            return "Move";
        }
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            int targetCountAtGridPosition = unit.GetActionType<ShootAction>().GetTargetCountAtGridPosition(gridPosition);
            return new EnemyAIAction() { gridPosition = gridPosition, actionValue = (targetCountAtGridPosition * 10) + 1};
        }
        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
            OnMoveStart?.Invoke();
            base.TakeAction(gridPosition, onActionComplete);
        }
    }
}
