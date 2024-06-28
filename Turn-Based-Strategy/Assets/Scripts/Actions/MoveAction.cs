using Game.Core;
using Game.Grid;
using Game.Units;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Actions
{
    public class MoveAction : MonoBehaviour
    {
        [SerializeField] private Animator unitAnimator = null;
        [SerializeField] private int maxDistance = 2;
        private Vector3 targetPosition;
        private float moveSpeed = 2f;
        private float rotateSpeed = 10f;
        private float stoppingDistance = 0.1f;

        private Unit unit;

        private void Awake()
        {
            targetPosition = transform.position;
            unit = GetComponent<Unit>();
        }

        void Update()
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
            {
                unitAnimator.SetBool("isRunning", true);
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            }
            else
            {
                unitAnimator.SetBool("isRunning", false);
            }
        }
        public void Move(GridPosition gridPosition)
        {
            targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        }

        /// <summary>
        /// Set a int maximumDistance and return a new List<GridPosition> after looping through the list
        /// </summary>
        /// <returns></returns>
        public List<GridPosition> GetValidActionGridPositions()
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

        public bool IsValidActionGridPosition(GridPosition gridPosition)
        {
            List<GridPosition> validGridPositions = GetValidActionGridPositions();
            foreach (var grid in validGridPositions)
            {
                Debug.Log(grid);
            }
            return validGridPositions.Contains(gridPosition);
        }
    }
}