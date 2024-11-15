using Game.Actions;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        public enum GridVisualColor
        {
            White,
            Red,
            Blue,
            Yellow,
            RedSoft
        }
        [Serializable]
        public struct GridVisualType
        {
            public GridVisualColor typeColor;
            public Material material;
        }

        [SerializeField] private Transform gridVisual = null;
        [SerializeField] private List<GridVisualType> gridVisualTypes;
        private GridSystemVisualSingle[,] gridSystemVisualSinglesArray;
        private void Start()
        {
            SetupGridVisual();
            UpdateGrid();
            UnitActionSystem.Instance.OnSelectedActionChanged += HandleSelectedActionChanged;
            LevelGrid.Instance.OnUnitGridPositionChanged += HandleUnitGridPositionChanged;
        }
        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedActionChanged -= HandleSelectedActionChanged;
            LevelGrid.Instance.OnUnitGridPositionChanged -= HandleUnitGridPositionChanged;
        }
        private void SetupGridVisual()
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
        private void HandleSelectedActionChanged()
        {
            UpdateGrid();
        }
        private void HandleUnitGridPositionChanged()
        {
            UpdateGrid();
        }
        private void UpdateGrid()
        {
            HideAllGridPositions();

            BaseAction action = UnitActionSystem.Instance.GetSelectedAction();
            GridVisualColor gridVisualColor;
            switch (action)
            {
                case MoveAction:
                    gridVisualColor = GridVisualColor.Yellow;
                    break;
                case SpinAction:
                    gridVisualColor = GridVisualColor.Blue;
                    break;
                case ShootAction:
                    gridVisualColor = GridVisualColor.Red;
                    GridVisualHelper(UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition(), 3, GridVisualColor.RedSoft);
                    break;
                default:
                    gridVisualColor = GridVisualColor.White;
                    break;
            }
            ShowGridPositionsList(UnitActionSystem.Instance.GetSelectedAction().GetValidActionGridPositions(),gridVisualColor);
        }
        private void GridVisualHelper(GridPosition gridPosition, int range,GridVisualColor gridVisualColor)
        {
            List<GridPosition> gridPositions = new List<GridPosition>();
            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                {

                    GridPosition currentGridPosition = gridPosition + new GridPosition(x, z);
                    if (x + z > range)
                        continue;
                    if (!LevelGrid.Instance.IsGridPositionWithinBounds(currentGridPosition))
                        continue;

                    gridPositions.Add(currentGridPosition);
                }
            }
            ShowGridPositionsList(gridPositions,gridVisualColor);
        }
        public void HideAllGridPositions()
        {
            foreach (GridSystemVisualSingle visualSingle in gridSystemVisualSinglesArray)
            {
                visualSingle.Hide();
            }
        }
        public void ShowGridPositionsList(List<GridPosition> gridPositions, GridVisualColor gridVisualColor)
        {
            foreach (GridPosition gridPosition in gridPositions)
            {
                gridSystemVisualSinglesArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualColor));
                
            }
        }
                
            
        
        public Material GetGridVisualTypeMaterial(GridVisualColor gridVisualColor)
        {
            foreach (GridVisualType gridVisualType in gridVisualTypes)
            {
                if (gridVisualType.typeColor == gridVisualColor)
                {
                    return gridVisualType.material;
                }
            }
            Debug.LogError("MATERIAL NOT FOUND!!");
            return null;
        }
    }
}