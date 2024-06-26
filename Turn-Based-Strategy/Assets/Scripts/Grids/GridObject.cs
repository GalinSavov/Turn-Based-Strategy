using Game.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class GridObject
    {
        private GridSystem gridSystem;
        private GridPosition gridPosition;
        private List<Unit> units;
        public GridObject(GridSystem gridSystem, GridPosition gridPosition)
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
                return "x: " + gridPosition.x + ", z: " + gridPosition.z + "\n" + unitStr;

            return "x: " + gridPosition.x + ", z: " + gridPosition.z;
        }
    }

}