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

        private List<UnitActionButton> unitActionButtonList;
        private void OnEnable()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += HandleSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged += HandleUpdateSelectedVisual;
        }
        private void Awake()
        {
            unitActionButtonList = new List<UnitActionButton>();
        }

        void Start()
        {
            HandleSelectedUnitChanged();
            HandleUpdateSelectedVisual();
        }
        private void OnDisable()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= HandleSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged -= HandleUpdateSelectedVisual;
        }
        private void HandleSelectedUnitChanged()
        {
 
            foreach (Transform child in unitActionButtonsContainer)
            {
                Destroy(child.gameObject);
            }
            unitActionButtonList.Clear();
            
            Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
            foreach (BaseAction action in unit.GetBaseActions())
            {
                Transform actionButtonTransform = Instantiate(unitActionButtonPrefab, unitActionButtonsContainer);
                UnitActionButton actionButton = actionButtonTransform.GetComponent<UnitActionButton>();
                actionButton.SetAction(action);
                unitActionButtonList.Add(actionButton);
            }

            HandleUpdateSelectedVisual();

        }
        private void HandleUpdateSelectedVisual()
        {
            foreach(UnitActionButton actionButton in unitActionButtonList)
            {
                actionButton.UpdateSelectedVisual();
            }
        }

    }
}
