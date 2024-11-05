using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turnNumber = 1;
    public static TurnSystem instance;
    public Action OnTurnChanged;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void NextTurn()
    {
        turnNumber++;
        OnTurnChanged?.Invoke();
    }
    public int GetTurnNumber() { return turnNumber; }
}
