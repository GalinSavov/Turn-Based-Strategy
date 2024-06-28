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
    public class UnitActionSystem : MonoBehaviour
    {
        [SerializeField] private Unit selectedUnit = null;
        [SerializeField] private LayerMask unitLayerMask;

        public static UnitActionSystem Instance { get; private set; }

        public event Action OnSelectedUnitChanged;

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

        void Update()
        {
            if (isCurrentlyInAction) return;

            if (Mouse.current.leftButton.isPressed)
            {
                if (TryHandleSelectedUnit()) return;
                ValidMoveActionGridPosition();
            }
            if (Mouse.current.rightButton.isPressed)
            {
                SetIsCurrentlyInAction();
                selectedUnit.GetSpinAction().Spin(ClearIsCurrentlyInAction);
            }
        }
        public void SetIsCurrentlyInAction()
        {
            isCurrentlyInAction = true;
        }
        public void ClearIsCurrentlyInAction()
        {
            isCurrentlyInAction = false;
        }

        private bool TryHandleSelectedUnit()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitLayerMask))
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    SetSelectedUnit(unit);
                    return true;
                }
            }
            return false;
        }

        private bool ValidMoveActionGridPosition()
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorldPosition.GetPosition());
            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                selectedUnit.GetMoveAction().Move(mouseGridPosition,ClearIsCurrentlyInAction);
                SetIsCurrentlyInAction();
                return true;
            }
            return false;
        }
        private void SetSelectedUnit(Unit unit)
        {
            selectedUnit = unit;
            OnSelectedUnitChanged?.Invoke();

        }
        public Unit GetSelectedUnit()
        {
            return selectedUnit;
        }
        public bool CheckIsValidGridPosition(GridPosition gridPosition)
        {
            return selectedUnit.GetMoveAction().IsValidActionGridPosition(gridPosition);
        }
    }

}