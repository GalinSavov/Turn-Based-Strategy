using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionPointsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text = null;
    private void Start()
    {
        UnitActionSystem.Instance.OnActionPointsSpent += HandleActionPointsSpent;
        UnitActionSystem.Instance.OnSelectedUnitChanged += HandleSelectedUnitChanged;
        TurnSystem.instance.OnTurnChanged += HandleTurnChanged;
        UpdateText();
    }
    private void OnDisable()
    {
        UnitActionSystem.Instance.OnActionPointsSpent -= HandleActionPointsSpent;
        UnitActionSystem.Instance.OnSelectedUnitChanged -= HandleSelectedUnitChanged;
        TurnSystem.instance.OnTurnChanged -= HandleTurnChanged;
    }
    private void HandleActionPointsSpent()
    {
        UpdateText();
    }
    private void HandleTurnChanged()
    {
        UpdateText();
    }
    private void HandleSelectedUnitChanged()
    {
        UpdateText();
    }
    private void UpdateText()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit != null) 
        text.text = "Action Points: " + selectedUnit.UnitActionPoints.ToString();
    }
}
