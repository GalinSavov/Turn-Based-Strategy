using Game.Actions;
using Game.Core;
using Game.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game.Units
{
    public class UnitActionSystem : MonoBehaviour
    {
        [SerializeField] private Unit selectedUnit = null;
        private BaseAction selectedAction;
        [SerializeField] private LayerMask unitLayerMask;
        public static UnitActionSystem Instance { get; private set; }

        public event Action OnSelectedUnitChanged;
        public event Action OnSelectedActionChanged;
        public event Action OnActionPointsSpent;
        public event Action<bool> OnCurrentlyInActionChanged;
        private bool isCurrentlyInAction;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        private void Start()
        {
            SetSelectedUnit(selectedUnit);
        }

        void Update()
        {
            if (isCurrentlyInAction) { return; }
            if (!TurnSystem.instance.GetIsPlayerTurn()) { return; }
            if(EventSystem.current.IsPointerOverGameObject()) { return; }
            if (TryHandleSelectedUnit()) { return; }
            HandleSelectedAction();  
        }

        private void HandleSelectedAction()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorldPosition.GetPosition());
                if (selectedAction.IsValidGridPosition(mouseGridPosition))
                {
                    if (selectedUnit.CanSpendActionPointsToTakeAction(selectedAction))
                    {
                        SetIsCurrentlyInAction();
                        selectedAction.TakeAction(mouseGridPosition, ClearIsCurrentlyInAction);
                        OnActionPointsSpent?.Invoke();
                    }
                }
            }
        }
        private bool TryHandleSelectedUnit()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitLayerMask))
                {
                    if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                    {
                        if (selectedUnit == unit) return false;

                        if (!unit.GetIsEnemy())
                        {
                            SetSelectedUnit(unit);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public void SetIsCurrentlyInAction()
        {
            isCurrentlyInAction = true;
            OnCurrentlyInActionChanged?.Invoke(isCurrentlyInAction);
        }
        public void ClearIsCurrentlyInAction()
        {
            isCurrentlyInAction = false;
            OnCurrentlyInActionChanged?.Invoke(isCurrentlyInAction);
        }
    
        private void SetSelectedUnit(Unit unit)
        {
            selectedUnit = unit;
            SetSelectedAction(selectedUnit.GetMoveAction());
            OnSelectedUnitChanged?.Invoke();

        }
        public void SetSelectedAction(BaseAction action)
        {
            selectedAction = action;
            OnSelectedActionChanged?.Invoke();
        }
        public Unit GetSelectedUnit()
        {
            return selectedUnit;
        }
        public BaseAction GetSelectedAction()
        {
            return selectedAction;
        }
        public bool CheckIsValidGridPosition(GridPosition gridPosition)
        {
            return selectedAction.IsValidGridPosition(gridPosition);
        }
    }

}