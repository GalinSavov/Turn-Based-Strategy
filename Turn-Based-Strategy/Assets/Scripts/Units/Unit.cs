using Game.Actions;
using Game.Core;
using Game.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Units
{
    public class Unit : MonoBehaviour
    {
        private const int ACTION_POINTS_MAX = 3;
        private BaseAction[] baseActions; 
        private MoveAction moveAction;
        private SpinAction spinAction;
        private GridPosition lastGridPosition;
        private int unitActionPoints = ACTION_POINTS_MAX;
        [SerializeField] private bool isEnemy = false;

        public int UnitActionPoints { get { return unitActionPoints; } set { unitActionPoints = value; }}
        private void Awake()
        {
            moveAction = GetComponent<MoveAction>();
            spinAction = GetComponent<SpinAction>();
            baseActions = GetComponents<BaseAction>();
        }
        private void Start()
        {
            TurnSystem.instance.OnTurnChanged += HandleOnTurnChanged;
            lastGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(this, lastGridPosition);
        }
        private void OnDisable()
        {
            TurnSystem.instance.OnTurnChanged -= HandleOnTurnChanged;
        }
        private void HandleOnTurnChanged()
        {
            if(isEnemy && !TurnSystem.instance.GetIsPlayerTurn() ||
               !isEnemy && TurnSystem.instance.GetIsPlayerTurn()) 
            unitActionPoints = ACTION_POINTS_MAX;
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
        public void Damage()
        {
            Debug.Log(gameObject.name + " was damaged!");
        }
        public bool CanSpendActionPointsToTakeAction(BaseAction action)
        {
            return unitActionPoints >= action.GetActionCost();
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
        public bool IsEnemy()
        {
            return isEnemy;
        }
    }
}
