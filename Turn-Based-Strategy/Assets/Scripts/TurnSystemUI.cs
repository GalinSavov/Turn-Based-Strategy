using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnNumberText = null;
    [SerializeField] Button endTurnButton = null;

    private void OnDisable()
    {
        TurnSystem.instance.OnTurnChanged -= UpdateText;
    }

    void Start()
    {
        TurnSystem.instance.OnTurnChanged += UpdateText;
        UpdateText();
        AddButtonEventListener();
    }
    private void UpdateText()
    {
        turnNumberText.text = "Turn " + TurnSystem.instance.GetTurnNumber().ToString();
    }
    private void AddButtonEventListener()
    {
        endTurnButton.onClick.AddListener(TurnSystem.instance.NextTurn);
    }
}
