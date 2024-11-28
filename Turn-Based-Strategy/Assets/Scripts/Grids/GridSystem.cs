using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class GridSystem<TGridObject>
    {
        private int width;
        private int height;
        private float cellSize; //used to convert each cell to world space
        private TGridObject[,] gridObjects; //2D array 
        
        public GridSystem(int width, int height,float cellSize,Func<GridSystem<TGridObject>,GridPosition,TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            gridObjects = new TGridObject[width, height];
            for (int x = 0; x < width; x++)
            {
                for(int z = 0; z < height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x,z);
                    gridObjects[x, z] =createGridObject(this,gridPosition);
                }
            }
        }
        public Vector3 GetWorldPositionFromGrid(GridPosition gridPosition)
        {
            return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
        }

        public GridPosition GetGridPositionFromWorld(Vector3 worldPosition)
        {
            Vector3 gridPositionVector = worldPosition / cellSize;
            return new GridPosition(Mathf.RoundToInt(gridPositionVector.x),Mathf.RoundToInt(gridPositionVector.z));
        }

        public void TestGrid(Transform prefab)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x,z);
                    Transform debugPrefab = GameObject.Instantiate(prefab, GetWorldPositionFromGrid(gridPosition),Quaternion.identity);
                    debugPrefab.GetComponent<DebugCellText>().SetGridObject(GetGridObject(gridPosition));
                }
            }
        }
        public TGridObject GetGridObject(GridPosition gridPosition)
        {
            return gridObjects[gridPosition.x, gridPosition.z];
        }

        public bool IsGridPositionWithinBounds(GridPosition gridPosition)
        {
            if(gridPosition.x <= width && gridPosition.x >=0 && gridPosition.z <= height && gridPosition.z >=0)
                return true;

            return false;
        }
        public int GetWidth()
        {
            return width;
        }
        public int GetHeight()
        {
            return height;
        }
        public float GetCellSize()
        {
            return cellSize;
        }

    }
}