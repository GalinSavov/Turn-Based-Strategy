using Game.Grid;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private int width;
    private int height;
    private int cellSize;
    private GridSystem<PathNode> nodeGridSystem;
    private List<PathNode> openList;
    public static Pathfinding Instance { get; private set; }

    private List<PathNode> closeList;
    [SerializeField] Transform debugPrefab = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        nodeGridSystem = new GridSystem<PathNode>(10, 10, 2f, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        nodeGridSystem.TestGrid(debugPrefab);
    }
    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {


        openList = new List<PathNode>(); //nodes to search
        closeList = new List<PathNode>(); //nodes already searched

        PathNode startNode = nodeGridSystem.GetGridObject(startGridPosition);
        PathNode endNode = nodeGridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode); //add the first node to the grid 

        for (int x = 0; x < nodeGridSystem.GetGridSystemWidth(); x++)
        {
            for (int z = 0; z < nodeGridSystem.GetGridSystemHeight(); z++)
            {
                //reset all the path nodes before doing any other logic
                PathNode pathNode = nodeGridSystem.GetGridObject(new GridPosition(x, z));
                pathNode.GCost = int.MaxValue;
                pathNode.HCost = 0;
                pathNode.ParentNode = null;
            }
        }
        startNode.GCost = 0; //0 because it is where the path starts
        startNode.HCost = CalculateDistance(startGridPosition, endGridPosition);

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestCostNode(openList);
            if (currentNode == endNode)
            {
                //reached final node
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);//node already searched
            closeList.Add(currentNode);


            foreach (PathNode neighbourNode in GetNodeNeighbours(currentNode))
            {
                if (closeList.Contains(neighbourNode)) // check if the neighbour node was already searched
                    continue;

                //calculate h and g costs for each neighbour node
                int gCost = currentNode.GCost + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
                if (gCost < neighbourNode.GCost)
                {
                    neighbourNode.ParentNode = currentNode;
                    neighbourNode.GCost = gCost;
                    neighbourNode.HCost = CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition);

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        Debug.Log("WTF");
        return null;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodes = new List<PathNode> ();
        pathNodes.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.ParentNode != null)
        {
            pathNodes.Add(currentNode.ParentNode);
            currentNode = currentNode.ParentNode;
        }
        pathNodes.Reverse();
        List<GridPosition> gridPositions = new List<GridPosition>();
        foreach (PathNode node in pathNodes)
        {
            gridPositions.Add(node.GetGridPosition());
        }
        return gridPositions;
    }

    public int CalculateDistance(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        GridPosition gridPosition = (startGridPosition - endGridPosition);
        int xDistance = Mathf.Abs(gridPosition.x);
        int zDistance = Mathf.Abs(gridPosition.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    private PathNode GetLowestCostNode(List<PathNode> pathNodes)
    {
        PathNode currentNode = pathNodes[0];
        foreach (PathNode node in pathNodes)
        {
            if (node.FCost < currentNode.FCost || node.FCost == currentNode.FCost && node.HCost < currentNode.HCost) //check for a more efficient node
            {
                currentNode = node;
            }
        }
        return currentNode;
    }
    private PathNode GetNode(int x, int z)
    {
        return nodeGridSystem.GetGridObject(new GridPosition(x, z));
    }
    private List<PathNode> GetNodeNeighbours(PathNode targetNode)
    {
        List<PathNode> neighbourNodes = new List<PathNode>();
        GridPosition gridPosition = targetNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            // Left
            neighbourNodes.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                // Left Down
                neighbourNodes.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < nodeGridSystem.GetGridSystemHeight())
            {
                {
                    // Left Up
                    neighbourNodes.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
                }
            }
        }
        if (gridPosition.x + 1 < nodeGridSystem.GetGridSystemWidth())
        {
            // Right
            neighbourNodes.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                // Right Down
                neighbourNodes.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < nodeGridSystem.GetGridSystemHeight())
            {
                // Right Up
                neighbourNodes.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }
        if (gridPosition.z - 1 >= 0)
        {
            // Down
            neighbourNodes.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }
        if (gridPosition.z + 1 < nodeGridSystem.GetGridSystemHeight())
        {
            // Up
            neighbourNodes.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }
        return neighbourNodes;
    }
}
