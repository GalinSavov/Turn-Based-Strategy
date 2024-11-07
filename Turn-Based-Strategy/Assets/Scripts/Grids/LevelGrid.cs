using Game.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class LevelGrid : MonoBehaviour
    {
        [SerializeField] Transform cellText;
        private GridSystem gridSystem;

        public static LevelGrid Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        void Start()
        {
            gridSystem = new GridSystem(10, 10, 2f);
            gridSystem.TestGrid(cellText);
        }
        public void AddUnitAtGridPosition(Unit unit, GridPosition gridPosition)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.AddUnit(unit);
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
        public void RemoveUnitAtGridPosition(Unit unit, GridPosition gridPosition)
        {
            gridSystem.GetGridObject(gridPosition).RemoveUnitFromList(unit);
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
            return gridSystem.GetGridSystemWidth();
        }
        public int GetGridSystemHeight()
        {
            return gridSystem.GetGridSystemHeight();
        }
    }

}