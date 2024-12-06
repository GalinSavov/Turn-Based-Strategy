using Game.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    void Start()
    {
        DestructableCrate.onAnyCrateDestroyed += HandleOnAnyCrateDestroyed;
    }
    private void OnDisable()
    {
        DestructableCrate.onAnyCrateDestroyed -= HandleOnAnyCrateDestroyed;
    }
    private void HandleOnAnyCrateDestroyed(GridPosition crateGridPosition)
    {
        Pathfinding.Instance.GetNode(crateGridPosition.x, crateGridPosition.z).Walkable = true;
    }
}
