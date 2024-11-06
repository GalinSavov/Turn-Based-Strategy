using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI turnNumberText = null;
    [SerializeField]private Button endTurnButton = null;
    [SerializeField] private GameObject enemyTurnVisual = null; 

    void Start()
    {
        TurnSystem.instance.OnTurnChanged += HandleOnTurnChanged;
        UpdateText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
        AddButtonEventListener();
    }
    private void HandleOnTurnChanged()
    {
        UpdateText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void OnDisable()
    {
        TurnSystem.instance.OnTurnChanged -= HandleOnTurnChanged;
    }
    private void UpdateText()
    {
        turnNumberText.text = "Turn " + TurnSystem.instance.GetTurnNumber().ToString();
    }
    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisual.gameObject.SetActive(!TurnSystem.instance.GetIsPlayerTurn());
    }
    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.instance.GetIsPlayerTurn());
    }
    private void AddButtonEventListener()
    {
        endTurnButton.onClick.AddListener(TurnSystem.instance.NextTurn);
    }
}
