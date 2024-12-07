using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class LevelGrid : MonoBehaviour
    {
        [SerializeField] Transform cellText;
        private GridSystem<GridObject> gridSystem;
        public Action OnUnitGridPositionChanged;

        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float cellSize;

        public static LevelGrid Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        }

        void Start()
        {
            Pathfinding.Instance.Setup(gridSystem.GetWidth(),gridSystem.GetHeight(),gridSystem.GetCellSize());
            gridSystem.TestGrid(cellText);
        }
        public void AddUnitAtGridPosition(Unit unit, GridPosition gridPosition)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.AddUnit(unit);
            OnUnitGridPositionChanged?.Invoke();
        }
        public void RemoveUnitAtGridPosition(Unit unit, GridPosition gridPosition)
        {
            gridSystem.GetGridObject(gridPosition).RemoveUnitFromList(unit);
            OnUnitGridPositionChanged?.Invoke();
        }

        public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition,GridPosition toGridPosition)
        {
            RemoveUnitAtGridPosition(unit, fromGridPosition);
            AddUnitAtGridPosition(unit,toGridPosition);
        }
        public List<Unit> GetUnitsAtGridPosition(GridPosition gridPosition)
        {
            List<Unit> units = gridSystem.GetGridObject(gridPosition).GetUnitList();
            return units;
        }
        public Unit GetUnitAtGridPosition(GridPosition gridPosition)
        {
            return GetUnitsAtGridPosition(gridPosition)[0];
        }
        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return gridSystem.GetGridPositionFromWorld(worldPosition);
        }
        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return gridSystem.GetWorldPositionFromGrid(gridPosition) ;
        }

        public bool IsGridPositionWithinBounds(GridPosition gridPosition)
        {
            return gridSystem.IsGridPositionWithinBounds(gridPosition);
        }
        public int GetGridSystemWidth()
        {
            return gridSystem.GetWidth();
        }
        public int GetGridSystemHeight()
        {
            return gridSystem.GetHeight();
        }
        public Door GetDoorAtGridPosition(GridPosition gridPosition)
        {
            return gridSystem.GetGridObject(gridPosition).Door;
        }
        public void SetDoorAtGridPosition(GridPosition gridPosition,Door door)
        {
            gridSystem.GetGridObject(gridPosition).Door = door;
        }
    }

}