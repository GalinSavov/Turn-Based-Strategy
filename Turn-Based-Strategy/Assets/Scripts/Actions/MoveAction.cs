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
        private List<Vector3> targetPositions;
        private int currentTargetPositionIndex;
        private float moveSpeed = 2f;
        private float rotateSpeed = 10f;
        private float stoppingDistance = 0.1f;
        private List<GridPosition> path;
        protected override void Awake()
        {
            base.Awake();
            actionCost = 1;
            //maxDistanceForActionExecution = 3;
        }

        void Update()
        {
            if (!isActive) { return; }

            Vector3 targetPosition = targetPositions[currentTargetPositionIndex];
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
            else
            {
                currentTargetPositionIndex++;
                if (currentTargetPositionIndex >= targetPositions.Count) 
                {
                    OnMoveEnd?.Invoke();
                    FinishAction();
                }             
            }
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

                    if (!Pathfinding.Instance.IsNodeWalkable(validGridPosition))
                        continue;

                    if(!Pathfinding.Instance.IsPathPossible(unitGridPosition,validGridPosition))
                        continue;

                    if (LevelGrid.Instance.GetUnitsAtGridPosition(validGridPosition).Count > 0)
                        continue;

                    if(Pathfinding.Instance.GetPathLength(unitGridPosition, validGridPosition) > maxDistanceForActionExecution * 10)
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
            currentTargetPositionIndex = 0;
            targetPositions = new List<Vector3>();
            path = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition,out int pathLength);
            foreach (GridPosition gridPos in path)
            {
                targetPositions.Add(LevelGrid.Instance.GetWorldPosition(gridPos));
            }
            OnMoveStart?.Invoke();
            base.TakeAction(gridPosition, onActionComplete);
        }
    }
}
