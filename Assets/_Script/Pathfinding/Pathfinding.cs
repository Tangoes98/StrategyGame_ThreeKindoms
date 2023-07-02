using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance;

    GridSystem _gridSystem;
    [SerializeField] Transform _pathNodeVisual;

    int _gridWidth;
    int _gridHeight;
    float _gridCellSize;


    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
        _gridWidth = LevelGrid.Instance.GetWidth();
        _gridHeight = LevelGrid.Instance.GetHeight();
        _gridCellSize = LevelGrid.Instance.GetCellSize();

        _gridSystem = new GridSystem(_gridWidth, _gridHeight, _gridCellSize);

        _gridSystem.CreatePathNodeVisual(_pathNodeVisual);
    }


    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closeList = new List<PathNode>();

        PathNode startNode = _gridSystem.GetPathNode(startGridPosition);
        PathNode endNode = _gridSystem.GetPathNode(endGridPosition);

        openList.Add(startNode);

        // PathNode value initialization
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int z = 0; z < _gridHeight; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = _gridSystem.GetPathNode(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateGridPositionDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                //Reach the final
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourNodeList(currentNode))
            {
                if (closeList.Contains(neighbourNode)) continue;

                int tempGCost = currentNode.GetGCost() + CalculateGridPositionDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tempGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromNode(currentNode);
                    neighbourNode.SetGCost(tempGCost);
                    neighbourNode.SetHCost(CalculateGridPositionDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);

                }
            }
        }

        // No path fount
        return null;

    }

    public int CalculateGridPositionDistance(GridPosition a, GridPosition b)
    {
        GridPosition gridDistance = a - b;
        int distance = Mathf.Abs(gridDistance.x) + Mathf.Abs(gridDistance.z);
        return distance * MOVE_STRAIGHT_COST;

        // If diagonal movement is applied
        // 
        // int xDistance = Mathf.Abs(gridDistance.x);
        // int zDistance = Mathf.Abs(gridDistance.z);
        // int remainning = Mathf.Abs(xDistance - zDistance);
        // return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remainning;
        //

    }

    PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostNode.GetFCost())
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }


    List<PathNode> GetNeighbourNodeList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition currentNodeGridPosition = currentNode.GetGridPosition();

        GridPosition rightNode = new GridPosition(currentNodeGridPosition.x + 1, currentNodeGridPosition.z);
        GridPosition leftNode = new GridPosition(currentNodeGridPosition.x - 1, currentNodeGridPosition.z);
        GridPosition upNode = new GridPosition(currentNodeGridPosition.x, currentNodeGridPosition.z + 1);
        GridPosition downNode = new GridPosition(currentNodeGridPosition.x, currentNodeGridPosition.z - 1);

        if (IsValidGridPosition(rightNode)) neighbourList.Add(GetNode(rightNode));
        if (IsValidGridPosition(leftNode)) neighbourList.Add(GetNode(leftNode));
        if (IsValidGridPosition(upNode)) neighbourList.Add(GetNode(upNode));
        if (IsValidGridPosition(downNode)) neighbourList.Add(GetNode(downNode));


        return neighbourList;
    }

    PathNode GetNode(GridPosition gridPosition) => _gridSystem.GetPathNode(gridPosition);
    bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);

    List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromNode());
            currentNode = currentNode.GetCameFromNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;

    }

}
