using Game.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        [SerializeField] private Transform gridVisual = null;
        private GridSystemVisualSingle[,] gridSystemVisualSinglesArray;

        private void Start()
        {
            gridSystemVisualSinglesArray = new GridSystemVisualSingle[LevelGrid.Instance.GetGridSystemWidth(),
                LevelGrid.Instance.GetGridSystemHeight()];

            for (int x = 0; x < LevelGrid.Instance.GetGridSystemWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetGridSystemHeight(); z++)
                {
                    Transform gridSystemVisualSingle = Instantiate(gridVisual, LevelGrid.Instance.GetWorldPosition(new GridPosition(x, z)), Quaternion.identity);
                    gridSystemVisualSinglesArray[x, z] = gridSystemVisualSingle.GetComponent<GridSystemVisualSingle>();

                }
            }
        }
        private void Update()
        {
           HideAllGridPositions();
           ShowGridPositionsList(UnitActionSystem.Instance.GetSelectedAction().GetValidActionGridPositions());
        }

        public void HideAllGridPositions()
        {
            foreach (GridSystemVisualSingle visualSingle in gridSystemVisualSinglesArray)
            {
                visualSingle.Hide();
            }
        }
        public void ShowGridPositionsList(List<GridPosition> gridPositions)
        {
            for (int x = 0; x < LevelGrid.Instance.GetGridSystemWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetGridSystemHeight(); z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    if (UnitActionSystem.Instance.CheckIsValidGridPosition(gridPosition))
                    {
                        gridSystemVisualSinglesArray[x, z].Show();
                    }
                }
            }


        }
    }

}