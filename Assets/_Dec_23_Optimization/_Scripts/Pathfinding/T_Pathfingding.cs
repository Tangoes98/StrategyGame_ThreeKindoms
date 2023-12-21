using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class T_Pathfingding : MonoBehaviour
{
    public static T_Pathfingding Instance;





    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }





    [Header("DEBUG_TEST_DELETE_LATER")]
    [SerializeField] T_GirdPosition TestGridStartPos;
    [SerializeField] T_GirdPosition TestGridEndPos;
    [SerializeField] List<T_GirdPosition> testNeighbourList;
    [SerializeField] List<T_GirdPosition> Panthfinding;

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            // CalculateStartGridPosFCost(TestGridStartPos, TestGridEndPos);
            // testNeighbourList = GetNeighbourPathNodeGridList(TestGridStartPos);
            // CalculateNeighbourGridFCost(testNeighbourList, TestGridStartPos, TestGridEndPos);

            Panthfinding = FindPath(TestGridStartPos, TestGridEndPos);

        }
    }






    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;
    const int PathfindingDistanceMultiplier = 10;


    T_GirdPosition _startGridPosition;
    T_GirdPosition _targetGridPosition;

    [SerializeField] List<T_GirdPosition> _neighbourGrids;
    [SerializeField] List<T_GirdPosition> _checkedGrids;



    #region Public Properties
    public int CalculateHCost(T_GirdPosition current, T_GirdPosition end) => CalculateGridPositionDistance(current, end);
    public int CalculateGCost(T_GirdPosition current, T_GirdPosition start) => CalculateGridPositionDistance(current, start);



    #endregion



    int CalculateGridPositionDistance(T_GirdPosition a, T_GirdPosition b)
    {
        int deltaX = Mathf.Abs(a.x - b.x);
        int deltaZ = Mathf.Abs(a.z - b.z);
        return (deltaX + deltaZ) * PathfindingDistanceMultiplier;
    }


    T_Pathnode GetGridPathNode(T_GirdPosition gp) => T_LevelGridManager.Instance.GetGridPosPathNode(gp);

    List<T_GirdPosition> FindPath(T_GirdPosition start, T_GirdPosition end)
    {
        List<T_GirdPosition> pathList = new();

        T_GirdPosition currentGridPos = start;

        CalculateStartGridPosFCost(start, end);

        pathList.Add(start);



        // while (currentGridPos != end)
        // {
        for (int i = 0; i < 50; i++)
        {
            Debug.Log(currentGridPos);
            if (currentGridPos == end)
            {
                Debug.Log("FindPath");
                return pathList;
            }

            List<T_GirdPosition> neighbourList = GetNeighbourPathNodeGridList(currentGridPos);

            List<T_Pathnode> neighbourNodeList = CalculateNeighbourGridFCost(neighbourList, currentGridPos, end);

            T_Pathnode nextNode = GetLowestFCostNode(neighbourNodeList);

            T_GirdPosition nextGridPos = nextNode.GetGridPosition();

            pathList.Add(nextGridPos);

            currentGridPos = nextGridPos;

            // if (Input.GetKey(KeyCode.Backspace))
            // {
            //     Debug.Log("BREAK");
            //     break;
            // }
        }



        // }
        Debug.Log("No path found");
        return null;


        // return null;




    }







    List<T_GirdPosition> GetNeighbourPathNodeGridList(T_GirdPosition gp)
    {
        List<T_GirdPosition> neightbourList = new();
        int gridWidth = T_LevelGridManager.Instance.GetGridWidth();
        int gridHeight = T_LevelGridManager.Instance.GetGridHeight();

        T_GirdPosition leftNode = new T_GirdPosition(gp.x - 1, gp.z);
        T_GirdPosition rightNode = new T_GirdPosition(gp.x + 1, gp.z);
        T_GirdPosition upNode = new T_GirdPosition(gp.x, gp.z + 1);
        T_GirdPosition downNode = new T_GirdPosition(gp.x, gp.z - 1);


        if (leftNode.x >= 0) neightbourList.Add(leftNode);
        if (rightNode.x <= gridWidth) neightbourList.Add(rightNode);
        if (upNode.z <= gridHeight) neightbourList.Add(upNode);
        if (downNode.z >= 0) neightbourList.Add(downNode);

        // skip checked neighbour node
        return RemoveCheckedGirdPos(neightbourList);
    }


    List<T_GirdPosition> RemoveCheckedGirdPos(List<T_GirdPosition> gridposList)
    {
        List<T_GirdPosition> nonDuplicatedNeighbourGrids = new();
        foreach (T_GirdPosition gridpos in gridposList)
        {
            if (GetGridPathNode(gridpos).GetFCost() == 0) nonDuplicatedNeighbourGrids.Add(gridpos);
        }
        return nonDuplicatedNeighbourGrids;
    }

    void CalculateStartGridPosFCost(T_GirdPosition startGirdpos, T_GirdPosition endingGridpos)
    {
        T_Pathnode startNode = GetGridPathNode(startGirdpos);
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateHCost(startGirdpos, endingGridpos));
        startNode.CalculateFCost();
    }

    List<T_Pathnode> CalculateNeighbourGridFCost(List<T_GirdPosition> gridposList, T_GirdPosition currentGridpos, T_GirdPosition endingGridpos)
    {
        T_Pathnode currentNode = GetGridPathNode(currentGridpos);
        int currentNodeGCost = currentNode.GetGCost();

        List<T_Pathnode> neighborNodeList = new();

        foreach (T_GirdPosition neighbourGridpos in gridposList)
        {
            T_Pathnode neighbourNode = GetGridPathNode(neighbourGridpos);
            neighbourNode.SetHCost(CalculateHCost(neighbourGridpos, endingGridpos));
            int tempGCost = CalculateGCost(neighbourGridpos, currentGridpos);
            neighbourNode.SetGCost(tempGCost + currentNodeGCost);
            neighbourNode.CalculateFCost();

            neighborNodeList.Add(neighbourNode);
        }

        return neighborNodeList;

    }

    T_Pathnode GetLowestFCostNode(List<T_Pathnode> pathNodeList)
    {
        T_Pathnode tempLowestFCostNode = pathNodeList[0];
        int tempLowestFCost = tempLowestFCostNode.GetFCost();

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() > tempLowestFCost) continue;
            else tempLowestFCostNode = pathNodeList[i];
        }

        return tempLowestFCostNode;
    }









}
