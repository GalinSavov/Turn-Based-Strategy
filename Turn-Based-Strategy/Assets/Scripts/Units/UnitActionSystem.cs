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
            if (Mouse.current.leftButton.isPressed)
            {
                if (TryHandleSelectedUnit()) return;
                selectedUnit.Move();
            }
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
        private void SetSelectedUnit(Unit unit)
        {
            selectedUnit = unit;
            OnSelectedUnitChanged?.Invoke();

        }
        public Unit GetSelectedUnit()
        {
            return selectedUnit;
        }
    }

}