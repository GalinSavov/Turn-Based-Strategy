using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private void Start()
    {
        TurnSystem.instance.OnTurnChanged += HandleTurnChanged;
    }
    private void OnDisable()
    {
        TurnSystem.instance.OnTurnChanged -= HandleTurnChanged;
    }

    private void HandleTurnChanged()
    {
        if (TurnSystem.instance.GetIsPlayerTurn()) { return; }
        StartCoroutine(PlayEnemyTurn());
    }
    private IEnumerator PlayEnemyTurn()
    {
        yield return new WaitForSeconds(3f);
        TurnSystem.instance.NextTurn();
    }
}
