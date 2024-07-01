using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Units;
using UnityEngine.UI;
using System;
using TMPro;
namespace Game.UI
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField] Transform unitActionButtonPrefab = null;
        [SerializeField] Transform unitActionButtonsContainer = null;
 

        private void OnEnable()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += HandleSelectedUnitChanged;
        }

        void Start()
        {
            CreateUnitActionButtons();
        }
        private void OnDisable()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= CreateUnitActionButtons;
        }
        private void CreateUnitActionButtons()
        {
            HandleSelectedUnitChanged();
        }
        private void HandleSelectedUnitChanged()
        {
 
            foreach (Transform child in unitActionButtonsContainer)
            {
                Destroy(child.gameObject);
            }

            Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
            foreach (BaseAction action in unit.GetBaseActions())
            {
                Transform actionButtonTransform = Instantiate(unitActionButtonPrefab, unitActionButtonsContainer);
                actionButtonTransform.GetComponent<UnitActionButton>().SetAction(action);
            }
        }

    }
}
