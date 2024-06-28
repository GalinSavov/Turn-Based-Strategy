using Game.Actions;
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
        private MoveAction moveAction;
        private GridPosition lastGridPosition;
        private void Awake()
        {
            moveAction = GetComponent<MoveAction>();
        }
        private void Start()
        {
            lastGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(this, lastGridPosition);
        }
        private void Update()
        {
            UpdateUnitGridPosition();
        }
        public void UpdateUnitGridPosition()
        {

            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition != lastGridPosition)
            {
                LevelGrid.Instance.RemoveUnitAtGridPosition(this, lastGridPosition);
                LevelGrid.Instance.AddUnitAtGridPosition(this, newGridPosition);
                lastGridPosition = newGridPosition;
            }

        }
        public MoveAction GetMoveAction()
        {
            return moveAction;
        }
        public GridPosition GetGridPosition()
        {
            return lastGridPosition;
        }
    }
}
