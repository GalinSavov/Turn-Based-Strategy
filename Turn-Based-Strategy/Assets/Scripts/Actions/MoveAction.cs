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
        [SerializeField] private Animator unitAnimator = null;
        [SerializeField] private int maxDistance = 2;
        private Vector3 targetPosition;
        private float moveSpeed = 2f;
        private float rotateSpeed = 10f;
        private float stoppingDistance = 0.1f;

 
        protected override void Awake()
        {
            base.Awake();
            targetPosition = transform.position;
            actionCost = 1;
        }

        void Update()
        {
            if (!isActive) { return; }

            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
            {
                unitAnimator.SetBool("isRunning", true);
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
            else
            {
                unitAnimator.SetBool("isRunning", false);
                isActive = false;
                onActionComplete();
            }
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        }
        /// <summary>
        /// Set a int maximumDistance and return a new List<GridPosition> after looping through the list
        /// </summary>
        /// <returns></returns>
        public override List<GridPosition> GetValidActionGridPositions()
        {
            List<GridPosition> validGridPositions = new List<GridPosition>();
            GridPosition unitGridPosition = unit.GetGridPosition();

            for (int x = -maxDistance; x <= maxDistance; x++)
            {
                for (int z = -maxDistance; z <= maxDistance; z++)
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

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            base.TakeAction(gridPosition, onActionComplete);
            targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        }
    }
}
