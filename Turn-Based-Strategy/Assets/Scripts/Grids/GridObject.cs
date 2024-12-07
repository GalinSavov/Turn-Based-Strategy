using Game.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class GridObject
    {
        private GridSystem<GridObject> gridSystem;
        private GridPosition gridPosition;
        private List<Unit> units;
        private Door door;
        public Door Door { get => door; set => door = value; }
        public GridObject(GridSystem<GridObject> gridSystem,GridPosition gridPosition)
        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
            units = new List<Unit>(); 
        }
        public void AddUnit(Unit unit)
        {
            units.Add(unit);       
        }
        public List<Unit> GetUnitList() 
        {
            return units;
        }
        public void RemoveUnitFromList(Unit unit)
        {
            units.Remove(unit);
        }
        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }
        public override string ToString()
        {
            string unitStr = "";
            foreach (Unit unit in units)
            {
                unitStr += unit.name;
            }
            if(unitStr != null)
                return gridPosition.ToString() + "\n" + unitStr;

            return gridPosition.ToString();
        }
    }

}