using Game.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int GCost { get; set; } // distance from the starting node
    public int HCost { get; set; } // distance to the end node
    public bool Walkable { get { return walkable; } set { walkable = value; } }
    public int FCost => GCost + HCost;

    private GridPosition gridPosition;
    private bool walkable = true;
    public PathNode ParentNode { get; set; }

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }
    public override string ToString()
    {
        return gridPosition.ToString();
    }
    public GridPosition GetGridPosition() { return gridPosition; }
}
