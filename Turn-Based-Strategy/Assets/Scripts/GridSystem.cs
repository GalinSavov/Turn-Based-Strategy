using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class GridSystem
    {
        private int width;
        private int height;
        //used to convert the cell to world space
        private float cellSize;
        
        public GridSystem(int width, int height,float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            for (int x = 0; x < width; x++)
            {
                for(int z = 0; z < height; z++)
                {
                    Debug.DrawLine(GetWorldPositionFromGrid(x,z),GetWorldPositionFromGrid(x,z) +Vector3.right * .2f,Color.white,1000);

                }
            }
        }
        public Vector3 GetWorldPositionFromGrid(int x, int z)
        {
            return new Vector3(x, 0, z) * cellSize;
        }

        public GridPosition GetGridPositionFromWorld(Vector3 worldPosition)
        {
            Vector3 gridPositionVector = worldPosition / cellSize;
            return new GridPosition((int)gridPositionVector.x, (int)gridPositionVector.z);
        }
    }

    

}