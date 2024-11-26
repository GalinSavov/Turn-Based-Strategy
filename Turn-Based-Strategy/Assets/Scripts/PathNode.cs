using Game.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;

    private GridPosition gridPosition;
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
