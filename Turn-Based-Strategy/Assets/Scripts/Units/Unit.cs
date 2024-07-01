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
        private BaseAction[] baseActions;
        private MoveAction moveAction;
        private SpinAction spinAction;
        private GridPosition lastGridPosition;
        private void Awake()
        {
            moveAction = GetComponent<MoveAction>();
            spinAction = GetComponent<SpinAction>();
            baseActions = GetComponents<BaseAction>();
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
        public SpinAction GetSpinAction()
        {
            return spinAction;
        }
        public GridPosition GetGridPosition()
        {
            return lastGridPosition;
        }
        public BaseAction[] GetBaseActions()
        {
            return baseActions;
        }
    }
}
