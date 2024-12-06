using Game.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    public static Action<GridPosition> onAnyCrateDestroyed;
   public void Damage()
    {
        onAnyCrateDestroyed?.Invoke(LevelGrid.Instance.GetGridPosition(transform.position));
        Destroy(gameObject);
    }
}
