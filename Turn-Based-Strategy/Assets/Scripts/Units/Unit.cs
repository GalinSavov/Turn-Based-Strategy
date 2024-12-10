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
        private const int ACTION_POINTS_MAX = 9;
        private BaseAction[] baseActions; 
        private GridPosition lastGridPosition;
        private int unitActionPoints = ACTION_POINTS_MAX;
        private Health unitHealth;
        [SerializeField] private bool isEnemy = false;
        public static Action<Unit> OnAnyUnitSpawned;
        public static Action<Unit> OnAnyUnitDead;
        public int UnitActionPoints { get { return unitActionPoints; } set { unitActionPoints = value; }}
        private void Awake()
        {
            baseActions = GetComponents<BaseAction>();
            unitHealth = GetComponent<Health>();
        }
        private void Start()
        {
            TurnSystem.instance.OnTurnChanged += HandleOnTurnChanged;
            unitHealth.OnDead += HandleUnitOnDie;
            OnAnyUnitSpawned?.Invoke(this);
            lastGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            transform.position = LevelGrid.Instance.GetWorldPosition(lastGridPosition);
            LevelGrid.Instance.AddUnitAtGridPosition(this, lastGridPosition);
        }
        private void Update()
        {
            UpdateUnitGridPosition();
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
        private void HandleUnitOnDie()
        {
            LevelGrid.Instance.RemoveUnitAtGridPosition(this, lastGridPosition);
            OnAnyUnitDead?.Invoke(this);
            Destroy(gameObject);
        }
       
        public void UpdateUnitGridPosition()
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition != lastGridPosition)
            {
                LevelGrid.Instance.UnitMovedGridPosition(this, lastGridPosition, newGridPosition);
                lastGridPosition = newGridPosition;
                LevelGrid.Instance.OnUnitGridPositionChanged?.Invoke();
            }
        }
        public void TakeDamage(int amount)
        {

            unitHealth.TakeDamage(amount);
        }
        public bool CanSpendActionPointsToTakeAction(BaseAction action)
        {
            return unitActionPoints >= action.GetActionCost();
        }
        public bool IsEnemy()
        {
            return isEnemy;
        }
        public GridPosition GetGridPosition()
        {
            return lastGridPosition;
        }
        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }
        public BaseAction[] GetBaseActions()
        {
            return baseActions;
        }
        public T GetActionType<T>() where T : BaseAction 
        {
            foreach (BaseAction action in baseActions)
            {
                if (action is T)
                    return (T)action;
            }
            return null;
        }
        public int GetUnitHealth()
        {
            return unitHealth.GetCurrentHealth();
        }
    }
}
