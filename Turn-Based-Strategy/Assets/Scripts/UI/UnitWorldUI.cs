using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText = null;
    [SerializeField] private Unit unit = null;

    private void Awake()
    {
        UnitActionSystem.Instance.OnActionPointsSpent += HandleActionPointsSpent;
        EnemyAI.OnActionPointsSpent += HandleEnemyActionPointsSpent;
    }

    private void HandleEnemyActionPointsSpent()
    {
        UpdateActionPointsText();
    }

    private void Start()
    {
        UpdateActionPointsText();
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnActionPointsSpent -= HandleActionPointsSpent;
    }
    private void HandleActionPointsSpent()
    {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.UnitActionPoints.ToString();
    }
}
