using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class T_Pathfinding : MonoBehaviour
{
    #region =========== Instance ==============
    public static T_Pathfinding Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }
    #endregion

    #region ========== Variables =============

    [Header("DEBUG_TEST_DELETE_LATER")]
    [SerializeField] T_GridPosition TestGridStartPos;
    [SerializeField] T_GridPosition TestGridEndPos;
    [SerializeField] List<T_GridPosition> testNeighbourList;
    [SerializeField] List<T_GridPosition> PathList;

    // const int MOVE_STRAIGHT_COST = 10;
    // const int MOVE_DIAGONAL_COST = 14;
    const int PathfindingDistanceMultiplier = 10;
    #endregion

    #region ============ Monobehavior ==========

    void Start()
    {
    }

    void Update()
    {

    }
    #endregion


    #region ========== Public ==========

    public int G_CalculateHCost(T_GridPosition current, T_GridPosition end) => CalculateGridPositionDistance(current, end);
    public int G_CalculateGCost(T_GridPosition current, T_GridPosition start) => CalculateGridPositionDistance(current, start);

    public List<T_GridPosition> G_FindPath(T_GridPosition start, T_GridPosition end, List<T_GridPosition> gpRange) => FindPath(start, end, gpRange);
    public List<T_GridPosition> G_ValidMoveableGPList(List<T_GridPosition> gpList, T_GridPosition start, int maxDistance)
        => PotentialMoveabelGridPositionListValidation(gpList, start, maxDistance);


    #endregion ==================================



    T_Pathnode GetGridPathNode(T_GridPosition gp) => T_LevelGridManager.Instance.G_GetGridPosPathNode(gp);

    int CalculateGridPositionDistance(T_GridPosition a, T_GridPosition b)
    {
        int deltaX = Mathf.Abs(a.x - b.x);
        int deltaZ = Mathf.Abs(a.z - b.z);
        return (deltaX + deltaZ) * PathfindingDistanceMultiplier;
    }

    int CalculatePathNodeDistance(T_Pathnode a, T_Pathnode b)
    {
        return CalculateGridPositionDistance(a.G_GetGridPosition(), b.G_GetGridPosition());
    }



    List<T_GridPosition> FindPath(T_GridPosition start, T_GridPosition end, List<T_GridPosition> gpRange)
    {
        ResetAllGridPathNodeCosts();

        List<T_GridPosition> tempPathList = new();
        List<T_GridPosition> finalPathList = new();

        T_GridPosition currentGridPos = start;
        T_Pathnode targetGridPosPathnode = GetGridPathNode(end);

        CalculateStartGridPosFCost(start, end);
        int totalDistance = CalculateGridPositionDistance(start, end);

        tempPathList.Add(start);
        finalPathList.Add(start);

        while (tempPathList.Count > 0)
        {
            //Debug.Log(currentGridPos);

            if (currentGridPos == end)
            {
                Debug.Log("FindPath");

                CleanTempPathList(tempPathList, finalPathList);

                tempPathList.Clear();

                return finalPathList;
            }


            List<T_GridPosition> neighbourList = GetNeighbourPathNodeGridList(currentGridPos, gpRange, start);

            if (neighbourList.Count < 1) break; // No valid neighbour grid

            List<T_Pathnode> neighbourNodeList = CalculateNeighbourGridFCost(neighbourList, currentGridPos, end);

            T_Pathnode nextNode;

            if (!neighbourNodeList.Contains(targetGridPosPathnode))
                nextNode = GetLowestFCostNode(neighbourNodeList, totalDistance, targetGridPosPathnode);
            else nextNode = targetGridPosPathnode;

            T_GridPosition nextGridPos = nextNode.G_GetGridPosition();

            tempPathList.Add(nextGridPos);

            currentGridPos = nextGridPos;
        }

        Debug.Log("No path found");
        return null;


    }

    // Based on the unit max moveable distace to get the valid moveable gridpositions list
    List<T_GridPosition> PotentialMoveabelGridPositionListValidation(List<T_GridPosition> gpList, T_GridPosition start, int maxDistance)
    {
        List<T_GridPosition> validList = new();
        foreach (T_GridPosition gp in gpList)
        {
            List<T_GridPosition> pathList = FindPath(start, gp, gpList);
            if (pathList == null) continue;

            int fCost = GetGridPathNode(pathList[pathList.Count() - 1]).G_GetFCost();
            if (fCost > maxDistance * PathfindingDistanceMultiplier) continue;

            validList.Add(gp);
        }
        return validList;
    }





    #region ========== Pathfinding Related Functions ==========


    List<T_GridPosition> GetNeighbourPathNodeGridList(T_GridPosition gp, List<T_GridPosition> gpRange, T_GridPosition startGP)
    {
        List<T_GridPosition> neightbourList = new();
        int gridWidth = T_LevelGridManager.Instance.G_GetGridWidth();
        int gridHeight = T_LevelGridManager.Instance.G_GetGridHeight();

        T_GridPosition leftNode = new T_GridPosition(gp.x - 1, gp.z);
        T_GridPosition rightNode = new T_GridPosition(gp.x + 1, gp.z);
        T_GridPosition upNode = new T_GridPosition(gp.x, gp.z + 1);
        T_GridPosition downNode = new T_GridPosition(gp.x, gp.z - 1);


        if (leftNode.x >= 0) neightbourList.Add(leftNode);
        if (rightNode.x < gridWidth) neightbourList.Add(rightNode);

        if (upNode.z < gridHeight) neightbourList.Add(upNode);
        if (downNode.z >= 0) neightbourList.Add(downNode);

        // skip checked neighbour node
        return NeighbourGridPositionValidation(neightbourList, gpRange, startGP);
    }


    List<T_GridPosition> NeighbourGridPositionValidation(List<T_GridPosition> gridposList, List<T_GridPosition> gpRange, T_GridPosition startGP)
    {
        List<T_GridPosition> ValidatedNeighbourGrids = new();
        foreach (T_GridPosition gridpos in gridposList)
        {
            //TODO Need to Update the condition later

            // Check if in the gridsystem
            if (!T_LevelGridManager.Instance.G_IsValidSystemGrid(gridpos)) continue;

            // Check if the gp is avaliable to reach
            if (!GetGridPathNode(gridpos).G_IsValidPathNode()) continue;

            // Check if the neighbour grid is too high
            if (!T_LevelGridManager.Instance.G_IsAbleToClimb(gridpos, startGP)) continue;

            // Check if the grid is already calculated
            if (GetGridPathNode(gridpos).G_GetFCost() != 0) continue;

            // Check if the grid is in provided grid range (When applying unit movement)
            if (!gpRange.Contains(gridpos)) continue;


            ValidatedNeighbourGrids.Add(gridpos);


        }
        return ValidatedNeighbourGrids;
    }

    void CalculateStartGridPosFCost(T_GridPosition startGirdpos, T_GridPosition endingGridpos)
    {
        T_Pathnode startNode = GetGridPathNode(startGirdpos);
        startNode.G_SetGCost(0);
        startNode.G_SetHCost(G_CalculateHCost(startGirdpos, endingGridpos));
        startNode.G_CalculateFCost();
    }

    List<T_Pathnode> CalculateNeighbourGridFCost(List<T_GridPosition> gridposList, T_GridPosition currentGridpos, T_GridPosition endingGridpos)
    {
        T_Pathnode currentNode = GetGridPathNode(currentGridpos);
        int currentNodeGCost = currentNode.G_GetGCost();

        List<T_Pathnode> neighborNodeList = new();

        foreach (T_GridPosition neighbourGridpos in gridposList)
        {
            T_Pathnode neighbourNode = GetGridPathNode(neighbourGridpos);
            neighbourNode.G_SetHCost(G_CalculateHCost(neighbourGridpos, endingGridpos));
            int tempGCost = G_CalculateGCost(neighbourGridpos, currentGridpos);
            neighbourNode.G_SetGCost(tempGCost + currentNodeGCost + neighbourNode.G_GetTerrainMoveCost());
            neighbourNode.G_CalculateFCost();

            neighborNodeList.Add(neighbourNode);

            //Debug.Log("Validated neighbour girds: " + neighbourNode.G_GetGridPosition());
        }

        return neighborNodeList;

    }

    T_Pathnode GetLowestFCostNode(List<T_Pathnode> pathNodeList, int totalDistance, T_Pathnode targetNode)
    {
        T_Pathnode tempLowestFCostNode = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].G_GetFCost() > tempLowestFCostNode.G_GetFCost()) continue;

            if (CalculatePathNodeDistance(pathNodeList[i], targetNode) > totalDistance) continue;

            tempLowestFCostNode = pathNodeList[i];
        }

        return tempLowestFCostNode;
    }

    void CleanTempPathList(List<T_GridPosition> tempPathList, List<T_GridPosition> finalPathList)
    {
        foreach (T_GridPosition item in tempPathList) finalPathList.Add(item);
    }

    void ResetAllGridPathNodeCosts()
    {
        int width = T_LevelGridManager.Instance.G_GetGridWidth();
        int height = T_LevelGridManager.Instance.G_GetGridHeight();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                T_GridPosition gridPos = new(i, j);

                GetGridPathNode(gridPos).G_ResetCosts();
            }
        }
    }
    #endregion








}
