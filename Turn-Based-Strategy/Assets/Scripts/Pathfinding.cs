using Game.Grid;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10; // distance between 2 nodes is 1
    private const int MOVE_DIAGONAL_COST = 14; // distance between 2 nodes diagonally is sqrt of 2 = 1.4
    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathNode> nodeGridSystem;
    private List<PathNode> openList;
    [SerializeField] LayerMask unwalkableLayerMask;
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
    }
    public void Setup(int width,int height,float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        nodeGridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        nodeGridSystem.TestGrid(debugPrefab);

        CheckForUnwalkables();
    }

    private void CheckForUnwalkables()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(new GridPosition(x, z));
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, unwalkableLayerMask))
                {
                    GetNode(x, z).Walkable = false;
                }
            }
        }
    }
    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        openList = new List<PathNode>(); //nodes to search
        closeList = new List<PathNode>(); //nodes already searched

        PathNode startNode = nodeGridSystem.GetGridObject(startGridPosition);
        PathNode endNode = nodeGridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode); //add the first node to the open list

        for (int x = 0; x < nodeGridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < nodeGridSystem.GetHeight(); z++)
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

                if (!neighbourNode.Walkable)
                {
                    closeList.Add(neighbourNode);
                    continue;
                }

                int gCost = currentNode.GCost + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
                if (gCost < neighbourNode.GCost || !openList.Contains(neighbourNode))
                {
                    neighbourNode.ParentNode = currentNode;
                    neighbourNode.GCost = gCost;
                    neighbourNode.HCost = CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition);
                    openList.Add(neighbourNode);

                    if(!openList.Contains(neighbourNode))
                        openList.Add(neighbourNode);
                }
            }
        }
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

    /// <summary>
    /// Used to calculate the G cost and H cost of a neighbour node
    /// </summary>
    /// <param name="startGridPosition"></param>
    /// <param name="endGridPosition"></param>
    /// <returns></returns>
    public int CalculateDistance(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        GridPosition gridPosition = (startGridPosition - endGridPosition);
        int xDistance = Mathf.Abs(gridPosition.x);
        int zDistance = Mathf.Abs(gridPosition.z);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * Mathf.Abs(xDistance - zDistance);
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
            if (gridPosition.z + 1 < nodeGridSystem.GetHeight())
            {
                {
                    // Left Up
                    neighbourNodes.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
                }
            }
        }
        if (gridPosition.x + 1 < nodeGridSystem.GetWidth())
        {
            // Right
            neighbourNodes.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                // Right Down
                neighbourNodes.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < nodeGridSystem.GetHeight())
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
        if (gridPosition.z + 1 < nodeGridSystem.GetHeight())
        {
            // Up
            neighbourNodes.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }
        return neighbourNodes;
    }
}
