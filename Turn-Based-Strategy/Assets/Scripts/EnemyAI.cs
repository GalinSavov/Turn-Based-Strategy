using Game.Core;
using Game.Grid;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer = 2f;
    private enum State
    {
        WaitingForPlayerTurn,
        TakingTurn,
        Busy
    }
    private State currentEnemyState;
    public static event Action OnActionPointsSpent;

    private void Awake()
    {
        currentEnemyState = State.WaitingForPlayerTurn;
    }
    private void Start()
    {
        TurnSystem.instance.OnTurnChanged += HandleTurnChanged;
    }
    private void OnDisable()
    {
        TurnSystem.instance.OnTurnChanged -= HandleTurnChanged;
    }
    private void Update()
    {
        if (TurnSystem.instance.GetIsPlayerTurn()) return;
       
        switch (currentEnemyState)
        {
            case State.WaitingForPlayerTurn:
                break;
            case State.TakingTurn:
                TakeEnemyTurn();
                break;
            case State.Busy:
                break;
            default:
                break;
        }
    }
    private void HandleTurnChanged()
    {
        if (!TurnSystem.instance.GetIsPlayerTurn())
        {
            currentEnemyState = State.TakingTurn;
        }
    }
    private void TakeEnemyTurn()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (TryTakeEnemyAIAction(SetStateTakingTurn))
            {
                currentEnemyState = State.Busy;
            }
            else
            {
                timer = 2f;
                TurnSystem.instance.NextTurn();
            }
        }
    }
    private bool TryTakeEnemyAIAction(Action onActionComplete)
    {
        foreach (Game.Units.Unit enemyUnit in UnitManager.instance.GetAllEnemyUnitsList()) 
        {
            if(TryTakeEnemyAIAction(enemyUnit, onActionComplete))
            return true;
        }
        return false;
    }
    private bool TryTakeEnemyAIAction(Game.Units.Unit enemyUnit,Action onActionComplete)
    {

        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach (BaseAction baseAction in enemyUnit.GetBaseActions())
        {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                continue;
            }
            if(bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if (bestEnemyAIAction != null && enemyUnit.CanSpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onActionComplete);
            OnActionPointsSpent?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }
    private void SetStateTakingTurn()
    {
        timer = 0.75f;
        currentEnemyState = State.TakingTurn;
    }
}
