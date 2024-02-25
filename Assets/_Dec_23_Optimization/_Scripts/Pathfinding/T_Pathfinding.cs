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
    [SerializeField] T_GirdPosition TestGridStartPos;
    [SerializeField] T_GirdPosition TestGridEndPos;
    [SerializeField] List<T_GirdPosition> testNeighbourList;
    [SerializeField] List<T_GirdPosition> PathList;

    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;
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

    public int G_CalculateHCost(T_GirdPosition current, T_GirdPosition end) => CalculateGridPositionDistance(current, end);
    public int G_CalculateGCost(T_GirdPosition current, T_GirdPosition start) => CalculateGridPositionDistance(current, start);

    public List<T_GirdPosition> G_FindPath(T_GirdPosition start, T_GirdPosition end, List<T_GirdPosition> gpRange) => FindPath(start, end, gpRange);


    #endregion ==================================



    T_Pathnode GetGridPathNode(T_GirdPosition gp) => T_LevelGridManager.Instance.G_GetGridPosPathNode(gp);

    int CalculateGridPositionDistance(T_GirdPosition a, T_GirdPosition b)
    {
        int deltaX = Mathf.Abs(a.x - b.x);
        int deltaZ = Mathf.Abs(a.z - b.z);
        return (deltaX + deltaZ) * PathfindingDistanceMultiplier;
    }

    int CalculatePathNodeDistance(T_Pathnode a, T_Pathnode b)
    {
        return CalculateGridPositionDistance(a.G_GetGridPosition(), b.G_GetGridPosition());
    }



    List<T_GirdPosition> FindPath(T_GirdPosition start, T_GirdPosition end, List<T_GirdPosition> gpRange)
    {
        ResetAllGridPathNodeCosts();

        List<T_GirdPosition> tempPathList = new();
        List<T_GirdPosition> finalPathList = new();

        T_GirdPosition currentGridPos = start;
        T_Pathnode targetGridPosPathnode = GetGridPathNode(end);

        CalculateStartGridPosFCost(start, end);
        int totalDistance = CalculateGridPositionDistance(start, end);

        tempPathList.Add(start);
        finalPathList.Add(start);

        //for (int i = 0; i < 50; i++)
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


            List<T_GirdPosition> neighbourList = GetNeighbourPathNodeGridList(currentGridPos, gpRange);

            if (neighbourList.Count < 1) break; // No valid neighbour grid

            List<T_Pathnode> neighbourNodeList = CalculateNeighbourGridFCost(neighbourList, currentGridPos, end);

            T_Pathnode nextNode;

            if (!neighbourNodeList.Contains(targetGridPosPathnode))
                nextNode = GetLowestFCostNode(neighbourNodeList, totalDistance, targetGridPosPathnode);
            else nextNode = targetGridPosPathnode;

            T_GirdPosition nextGridPos = nextNode.G_GetGridPosition();

            tempPathList.Add(nextGridPos);

            currentGridPos = nextGridPos;
        }

        Debug.Log("No path found");
        return null;


    }





    #region ========== Pathfinding Related Functions ==========


    List<T_GirdPosition> GetNeighbourPathNodeGridList(T_GirdPosition gp, List<T_GirdPosition> gpRange)
    {
        List<T_GirdPosition> neightbourList = new();
        int gridWidth = T_LevelGridManager.Instance.G_GetGridWidth();
        int gridHeight = T_LevelGridManager.Instance.G_GetGridHeight();

        T_GirdPosition leftNode = new T_GirdPosition(gp.x - 1, gp.z);
        T_GirdPosition rightNode = new T_GirdPosition(gp.x + 1, gp.z);
        T_GirdPosition upNode = new T_GirdPosition(gp.x, gp.z + 1);
        T_GirdPosition downNode = new T_GirdPosition(gp.x, gp.z - 1);


        if (leftNode.x >= 0) neightbourList.Add(leftNode);
        if (rightNode.x < gridWidth) neightbourList.Add(rightNode);

        if (upNode.z < gridHeight) neightbourList.Add(upNode);
        if (downNode.z >= 0) neightbourList.Add(downNode);

        // skip checked neighbour node
        return NeighbourGridPositionValidation(neightbourList, gpRange);
    }


    List<T_GirdPosition> NeighbourGridPositionValidation(List<T_GirdPosition> gridposList, List<T_GirdPosition> gpRange)
    {
        List<T_GirdPosition> ValidatedNeighbourGrids = new();
        foreach (T_GirdPosition gridpos in gridposList)
        {
            // Need to Update the condition later

            // Check if in the gridsystem
            if (!T_LevelGridManager.Instance.G_IsValidSystemGrid(gridpos)) continue;

            // Check if the gp is avaliable to reach
            if (!GetGridPathNode(gridpos).G_IsValidPathNode()) continue;

            // Check if the grid is already calculated
            if (GetGridPathNode(gridpos).G_GetFCost() != 0) continue;

            // Check if the grid is in provided grid range (When applying unit movement)
            if (!gpRange.Contains(gridpos)) continue;


            ValidatedNeighbourGrids.Add(gridpos);


        }
        return ValidatedNeighbourGrids;
    }

    void CalculateStartGridPosFCost(T_GirdPosition startGirdpos, T_GirdPosition endingGridpos)
    {
        T_Pathnode startNode = GetGridPathNode(startGirdpos);
        startNode.G_SetGCost(0);
        startNode.G_SetHCost(G_CalculateHCost(startGirdpos, endingGridpos));
        startNode.G_CalculateFCost();
    }

    List<T_Pathnode> CalculateNeighbourGridFCost(List<T_GirdPosition> gridposList, T_GirdPosition currentGridpos, T_GirdPosition endingGridpos)
    {
        T_Pathnode currentNode = GetGridPathNode(currentGridpos);
        int currentNodeGCost = currentNode.G_GetGCost();

        List<T_Pathnode> neighborNodeList = new();

        foreach (T_GirdPosition neighbourGridpos in gridposList)
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

    void CleanTempPathList(List<T_GirdPosition> tempPathList, List<T_GirdPosition> finalPathList)
    {
        foreach (T_GirdPosition item in tempPathList) finalPathList.Add(item);
    }

    void ResetAllGridPathNodeCosts()
    {
        int width = T_LevelGridManager.Instance.G_GetGridWidth();
        int height = T_LevelGridManager.Instance.G_GetGridHeight();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                T_GirdPosition gridPos = new(i, j);

                GetGridPathNode(gridPos).G_ResetCosts();
            }
        }
    }









    #endregion








}
