using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;
    private List<Unit> allUnits = new List<Unit>();
    private List<Unit> friendlyUnits = new List<Unit>();
    private List<Unit> enemyUnits = new List<Unit>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        allUnits = new List<Unit>();
        friendlyUnits = new List<Unit>();
        enemyUnits = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += HandleOnAnyUnitSpawned;
        Unit.OnAnyUnitDead += HandleOnAnyUnitDead;
    }
    private void OnDestroy()
    {
        Unit.OnAnyUnitSpawned -= HandleOnAnyUnitSpawned;
        Unit.OnAnyUnitDead -= HandleOnAnyUnitDead;
    }
    private void HandleOnAnyUnitDead(Unit unit)
    {
        allUnits.Remove(unit);
        if(unit.IsEnemy())
            enemyUnits.Remove(unit);
        else
            friendlyUnits.Remove(unit);
    }

    private void HandleOnAnyUnitSpawned(Unit unit)
    {
        allUnits.Add(unit);
        if (unit.IsEnemy())
            enemyUnits.Add(unit);
        else
            friendlyUnits.Add(unit);
    }
    public List<Unit> GetAllUnitsList()
    {
        return allUnits;
    }
    public List<Unit> GetAllEnemyUnitsList()
    {
        return enemyUnits;
    }
    public List<Unit> GetAllFriendlyUnitsList()
    {
        return friendlyUnits;
    }
}
