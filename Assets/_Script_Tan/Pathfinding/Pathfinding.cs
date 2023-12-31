using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;
    const int PathfindingDistanceMultiplier = 10;

    public static Pathfinding Instance;

    GridSystem _gridSystem;
    [SerializeField] Transform _pathNodeVisual;

    protected int _gridWidth;
    protected int _gridHeight;
    float _gridCellSize;

    List<GridPosition> _validMoveGridPoisitionList = new List<GridPosition>();


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

        // _gridSystem = LevelGrid.Instance.GetGridSystem();

        _gridSystem.CreatePathNodeVisual(_pathNodeVisual);

    }

    void Start()
    {

    }
    void Update()
    {


    }











    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
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

                pathNode.SetGCost(5000); // 5000 is greater than any Unit max move distance
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
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (PathNode neighbourNode in GetValidMoveableNeighbourNodeList(currentNode))
            {
                if (closeList.Contains(neighbourNode)) continue;

                if (!neighbourNode.GetIsWalkable())
                {
                    closeList.Add(neighbourNode);
                    continue;
                }


                int tempGCost = currentNode.GetGCost() + CalculateNeighbourGridPositionDistance(neighbourNode.GetGridPosition(), currentNode.GetGridPosition());

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

        // No path fount.
        pathLength = 0;
        return null;

    }

    #region //PathFindingCodeOriginal//

    // public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    // {
    //     List<PathNode> openList = new List<PathNode>();
    //     List<PathNode> closeList = new List<PathNode>();

    //     PathNode startNode = _gridSystem.GetPathNode(startGridPosition);
    //     PathNode endNode = _gridSystem.GetPathNode(endGridPosition);

    //     openList.Add(startNode);

    //     // PathNode value initialization
    //     for (int x = 0; x < _gridWidth; x++)
    //     {
    //         for (int z = 0; z < _gridHeight; z++)
    //         {
    //             GridPosition gridPosition = new GridPosition(x, z);
    //             PathNode pathNode = _gridSystem.GetPathNode(gridPosition);

    //             pathNode.SetGCost(int.MaxValue);
    //             pathNode.SetHCost(0);
    //             pathNode.CalculateFCost();
    //             pathNode.ResetCameFromNode();
    //         }
    //     }

    //     startNode.SetGCost(0);
    //     startNode.SetHCost(CalculateGridPositionDistance(startGridPosition, endGridPosition));
    //     startNode.CalculateFCost();

    //     while (openList.Count > 0)
    //     {
    //         PathNode currentNode = GetLowestFCostPathNode(openList);

    //         if (currentNode == endNode)
    //         {
    //             //Reach the final
    //             pathLength = endNode.GetFCost();
    //             return CalculatePath(endNode);
    //         }

    //         openList.Remove(currentNode);
    //         closeList.Add(currentNode);

    //         foreach (PathNode neighbourNode in GetNeighbourNodeList(currentNode))
    //         {
    //             if (closeList.Contains(neighbourNode)) continue;

    //             if (!neighbourNode.GetIsWalkable())
    //             {
    //                 closeList.Add(neighbourNode);
    //                 continue;
    //             }


    //             int tempGCost = currentNode.GetGCost() + CalculateGridPositionDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

    //             if (tempGCost < neighbourNode.GetGCost())
    //             {
    //                 neighbourNode.SetCameFromNode(currentNode);
    //                 neighbourNode.SetGCost(tempGCost);
    //                 neighbourNode.SetHCost(CalculateGridPositionDistance(neighbourNode.GetGridPosition(), endGridPosition));
    //                 neighbourNode.CalculateFCost();

    //                 if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);

    //             }
    //         }
    //     }

    //     // No path fount.
    //     pathLength = 0;
    //     return null;

    // }

    #endregion











    public List<GridPosition> GetValidMoveGridPoisitionList(GridPosition startGridPosition, float moveDistance)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closeList = new List<PathNode>();

        PathNode startNode = _gridSystem.GetPathNode(startGridPosition);
        startNode.SetAccucmulatedMoveDistance(0);

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            PathNode currentNode = openList[0];

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourNodeList(currentNode))
            {
                if (closeList.Contains(neighbourNode)) continue;
                if (validGridPositionList.Contains(neighbourNode.GetGridPosition())) continue;
                if (!neighbourNode.GetIsWalkable())
                {
                    closeList.Add(neighbourNode);
                    continue;
                }

                // calculate distance between neighbourNode and currentNode
                int distance = currentNode.GetAccucmulatedMoveDistance() + CalculateNeighbourGridPositionDistance(neighbourNode.GetGridPosition(), currentNode.GetGridPosition());

                if (distance <= moveDistance * PathfindingDistanceMultiplier)
                {
                    neighbourNode.SetAccucmulatedMoveDistance(distance);
                    if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                    if (!validGridPositionList.Contains(neighbourNode.GetGridPosition())) validGridPositionList.Add(neighbourNode.GetGridPosition());
                }
            }
        }
        _validMoveGridPoisitionList = validGridPositionList;
        return validGridPositionList;
    }








    public bool CanClimbNeighbourGrid(GridPosition currentPosition, GridPosition neighbourPosition)
    {
        int currentGridHeight = LevelGrid.Instance.GetGridFloorHeight(currentPosition);
        int neighbourGridHeight = LevelGrid.Instance.GetGridFloorHeight(neighbourPosition);

        int maxFloorMoveDifference = 2;



        if ((neighbourGridHeight - currentGridHeight) < maxFloorMoveDifference) return true;

        if (!LevelGrid.Instance.HasConstructionOnGridPosition(currentPosition)) return false;

        Construction construction = LevelGrid.Instance.GetConstructionAtGridPosition(currentPosition);
        return construction is ConstructionRopeLadder;

    }











    public int CalculateGridPositionDistance(GridPosition a, GridPosition b)
    {
        GridPosition gridDistance = a - b;
        int distance = Mathf.Abs(gridDistance.x) + Mathf.Abs(gridDistance.z);
        return distance * MOVE_STRAIGHT_COST;

        #region // If diagonal movement is applied
        // 
        // int xDistance = Mathf.Abs(gridDistance.x);
        // int zDistance = Mathf.Abs(gridDistance.z);
        // int remainning = Mathf.Abs(xDistance - zDistance);
        // return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remainning;
        //
        #endregion

    }
    public int CalculateNeighbourGridPositionDistance(GridPosition neighbourGrid, GridPosition currentGrid)
    {
        GridPosition gridDistance = neighbourGrid - currentGrid;
        int distance = Mathf.Abs(gridDistance.x) + Mathf.Abs(gridDistance.z);
        int neighbourGridMoveCost = GetNode(neighbourGrid).GetMoveCost();
        return distance * neighbourGridMoveCost;
    }

    public int CalculateTotalMoveDistance(List<GridPosition> gridPositionList)
    {
        int totalDistance = 0;

        for (int i = 0; i < gridPositionList.Count - 1; i++)
        {
            int tempDistance = CalculateNeighbourGridPositionDistance(gridPositionList[i + 1], gridPositionList[i]);

            totalDistance += tempDistance;
        }
        return totalDistance;
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

        if (IsValidGridPosition(rightNode) && CanClimbNeighbourGrid(currentNodeGridPosition, rightNode)) neighbourList.Add(GetNode(rightNode));
        if (IsValidGridPosition(leftNode) && CanClimbNeighbourGrid(currentNodeGridPosition, leftNode)) neighbourList.Add(GetNode(leftNode));
        if (IsValidGridPosition(upNode) && CanClimbNeighbourGrid(currentNodeGridPosition, upNode)) neighbourList.Add(GetNode(upNode));
        if (IsValidGridPosition(downNode) && CanClimbNeighbourGrid(currentNodeGridPosition, downNode)) neighbourList.Add(GetNode(downNode));


        return neighbourList;
    }

    List<PathNode> GetValidMoveableNeighbourNodeList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition currentNodeGridPosition = currentNode.GetGridPosition();

        GridPosition rightNodeGridPosition = new GridPosition(currentNodeGridPosition.x + 1, currentNodeGridPosition.z);
        GridPosition leftNodeGridPosition = new GridPosition(currentNodeGridPosition.x - 1, currentNodeGridPosition.z);
        GridPosition upNodePosition = new GridPosition(currentNodeGridPosition.x, currentNodeGridPosition.z + 1);
        GridPosition downNodeGridPosition = new GridPosition(currentNodeGridPosition.x, currentNodeGridPosition.z - 1);


        if (ValidNeighbourGridCheck(currentNodeGridPosition, rightNodeGridPosition)) neighbourList.Add(GetNode(rightNodeGridPosition));
        if (ValidNeighbourGridCheck(currentNodeGridPosition, leftNodeGridPosition)) neighbourList.Add(GetNode(leftNodeGridPosition));
        if (ValidNeighbourGridCheck(currentNodeGridPosition, upNodePosition)) neighbourList.Add(GetNode(upNodePosition));
        if (ValidNeighbourGridCheck(currentNodeGridPosition, downNodeGridPosition)) neighbourList.Add(GetNode(downNodeGridPosition));

        return neighbourList;
    }

    bool ValidNeighbourGridCheck(GridPosition currentGrid, GridPosition nextGrid)
    {
        if (ValidMoveableNeighbourNodeCheck(nextGrid) &&
            NeighbourGridPositionClimbingCheck(currentGrid, nextGrid))
            return true;
        return false;
    }

    bool ValidMoveableNeighbourNodeCheck(GridPosition gridpos)
    {
        if (IsValidGridPosition(gridpos) &&
            _validMoveGridPoisitionList.Contains(gridpos) &&
            !HasUnitOnGridPosition(gridpos) &&
            !NeighbourGirdConstrucionCheck<ConstructionObstacle>(gridpos))
            return true;
        return false;
    }

    bool NeighbourGridPositionClimbingCheck(GridPosition currentGridpos, GridPosition nextGridpos)
    {
        if (CanClimbNeighbourGrid(currentGridpos, nextGridpos)) return true;
        return false;
    }

    bool NeighbourGirdConstrucionCheck<ConstructionType>(GridPosition gridpos)
    {
        if (!LevelGrid.Instance.HasConstructionOnGridPosition(gridpos)) return false;
        Construction construction = LevelGrid.Instance.GetConstructionAtGridPosition(gridpos);
        return construction is ConstructionType;
    }










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






    public void UpdatingGridMoveCost(GridPosition gridPosition)
    {
        PathNode pathNode = Pathfinding.Instance.GetNode(gridPosition);
        int moveCost = pathNode.GetMoveCost();
        pathNode.SetMoveCost(moveCost / 2);
    }











    public PathNode GetNode(GridPosition gridPosition) => _gridSystem.GetPathNode(gridPosition);
    bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);

    bool HasUnitOnGridPosition(GridPosition gridpos) => LevelGrid.Instance.HasUnitOnGridPosition(gridpos);


    public bool IsWalkableGridPosition(GridPosition gridPosition) => GetNode(gridPosition).GetIsWalkable();
    public bool HasPathToGridPosition(GridPosition start, GridPosition end) => FindPath(start, end, out int pathLength) != null;

    public int GetPathLength(GridPosition start, GridPosition end)
    {
        FindPath(start, end, out int pathLength);
        return pathLength;
    }



}
