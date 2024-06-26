using Game.Core;
using Game.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Animator unitAnimator = null;
        private Vector3 targetPosition;
        private float moveSpeed = 2f;
        private float rotateSpeed = 10f;
        private float stoppingDistance = 0.1f;

        private GridPosition lastGridPosition;

        private void Awake()
        {
            targetPosition = transform.position;
        }
        private void Start()
        {
            lastGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(this, lastGridPosition);
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
        public void Move()
        {
            targetPosition = MouseWorldPosition.GetPosition();
            UpdateUnitGridPosition();
        }
        private void UpdateUnitGridPosition()
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorldPosition.GetPosition());
            LevelGrid.Instance.RemoveUnitAtGridPosition(this,lastGridPosition);
            LevelGrid.Instance.AddUnitAtGridPosition(this,newGridPosition);
            lastGridPosition = newGridPosition;
        }

    }
}
