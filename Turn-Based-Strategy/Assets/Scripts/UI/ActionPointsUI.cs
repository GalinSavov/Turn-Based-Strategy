using Game.Units;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionPointsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text = null;
    private void Awake()
    {
        UnitActionSystem.Instance.OnActionPointsSpent += UpdateText;
    }
    private void OnDisable()
    {
        UnitActionSystem.Instance.OnActionPointsSpent -= UpdateText;
    }
    void Start()
    {
        UpdateText();
    }
    private void UpdateText()
    {
        text.text = "Action Points: " + UnitActionSystem.Instance.GetSelectedUnit().UnitActionPoints.ToString();
    }
}
