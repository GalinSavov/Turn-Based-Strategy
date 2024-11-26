using Game.Core;
using Game.Grid;
using Game.Units;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] GridSystemVisual gridSystemVisual;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {   
            GridPosition startGridPosition = new GridPosition(0, 0);
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorldPosition.GetPosition());
            List<GridPosition> gridPositions = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);
            for (int i = 0; i < gridPositions.Count - 1; i++)
            {
               Debug.DrawLine(LevelGrid.Instance.GetWorldPosition(gridPositions[i]), LevelGrid.Instance.GetWorldPosition(gridPositions[i + 1]),Color.green,10f);
            }
            
        }
    }
}
