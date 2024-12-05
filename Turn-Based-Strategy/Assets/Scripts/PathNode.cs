using Game.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : IHeapItem<PathNode>
{
    private GridPosition gridPosition;
    private bool walkable = true;
    private int heapIndex;

    public int GCost { get; set; } // distance from the starting node
    public int HCost { get; set; } // distance to the end node
    public bool Walkable { get { return walkable; } set { walkable = value; } }
    public int FCost => GCost + HCost;
    public PathNode ParentNode { get; set; }
    public int HeapIndex { get => heapIndex; set => heapIndex = value; }

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }
    public override string ToString()
    {
        return gridPosition.ToString();
    }
    public GridPosition GetGridPosition() { return gridPosition; }

    public int CompareTo(PathNode nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(nodeToCompare.HCost);
        }
        return -compare; // return - because if this < other I want to return 1, not -1
    }
}
