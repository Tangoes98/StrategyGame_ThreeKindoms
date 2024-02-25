using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

public class T_LevelGridManager : MonoBehaviour
{
    public static T_LevelGridManager Instance;




    [SerializeField] int _gridWidth;
    [SerializeField] int _gridHeight;
    [SerializeField] float _gridCellSize;
    [SerializeField] Transform _gridObejctVisual;


    T_GridSystem _gridSystem;

    [SerializeField] Transform _gridValidationVisualObject;
    T_GridValidationVisual[,] _gridValidationVisuals;




    #region ========== Public Property =================

    //* ------------ Grid System ------------
    public int G_GetGridWidth() => _gridWidth;
    public int G_GetGridHeight() => _gridHeight;
    public bool G_IsValidSystemGrid(T_GridPosition gp) => _gridSystem.IsValidGridPosition(gp);


    //* -------- Grid/World position --------
    public Vector3 G_GridToWorldPosition(T_GridPosition gridPosition) => _gridSystem.GridToWorldPosition(gridPosition);
    public Vector3 G_GridToWorldPositionWithFloor(T_GridPosition gridPosition) => _gridSystem.GridToWorldPositionIncludesFloor(gridPosition);
    public T_GridPosition G_WorldToGridPosition(Vector3 worldPosition) => _gridSystem.WorldToGridPosition(worldPosition);
    public List<T_GridPosition> G_ConvertListWorldToGridPosition(List<Vector3> worldPositions) => ConvertListWorldToGridPosition(worldPositions);
    public List<Vector3> G_ConvertListGridToWorldPosition(List<T_GridPosition> gridPositions) => ConvertListGridToWorldPosition(gridPositions);
    public bool G_IsGridPositionsOnSameFloorHeight(T_GridPosition a, T_GridPosition b, out Vector3 gridOffsetWorldPos)
        => IsGridPositionsOnSameFloorHeight(a, b, out gridOffsetWorldPos);
    public bool G_IsAbleToClimb(T_GridPosition target, T_GridPosition start) => IsAbleToClimbOver(target, start);


    //* -------- Grid Data/ Grid Path node --------
    public T_GridData G_GetGridPosData(T_GridPosition gridPosition) => _gridSystem.GetGridData(gridPosition);
    public T_Pathnode G_GetGridPosPathNode(T_GridPosition gridPosition) => _gridSystem.GetPathnode(gridPosition);


    //* -------- Grid Validation Visual --------
    public T_GridValidationVisual G_GetGridValidationVisual(T_GridPosition gp) => GetGridValidationVisual(gp);

    public void G_ClearAllGridValidationVisuals() => ClearAllGridValidationVisuals();

    /// <summary>
    /// String name: MOVE_GRID, ATTACK_RANGE, VALID_ATTACK
    /// </summary>
    /// <param name="visualName">MOVE_GRID, ATTACK_RANGE, VALID_ATTACK</param>
    /// <param name="gpList"></param>
    public void G_ShowGridValidationVisuals(string visualName, List<T_GridPosition> gpList) => ShowGridValidationVisuals(visualName, gpList);








    #endregion

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;


        _gridSystem = new(_gridWidth, _gridHeight, _gridCellSize);


    }

    void Start()
    {
        // Wait for 3 second then generate grid visual
        WaitForHeight(3, GridVisualInit);


    }

    #region =============== Waiting for GridPosition height information =================

    async void WaitForHeight(int waitTime, Action gridVisualInit)
    {
        await Task.Delay(waitTime);
        gridVisualInit();
    }

    void GridVisualInit()
    {
        _gridSystem.CreateGridVisual(_gridObejctVisual);
        InitializeGridValidationVisuals(_gridValidationVisualObject);
    }

    #endregion

    #region ========== GRID VALIDATION VISUAL FUNCTIONS =================

    void InitializeGridValidationVisuals(Transform visualObject)
    {
        _gridValidationVisuals = new T_GridValidationVisual[_gridWidth, _gridHeight];
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                var gridPosition = new T_GridPosition(i, j);

                Transform obj = Instantiate(visualObject, G_GridToWorldPositionWithFloor(gridPosition), Quaternion.identity);

                T_GridValidationVisual validationVisual = obj.GetComponent<T_GridValidationVisual>();

                _gridValidationVisuals[i, j] = validationVisual;
            }
        }
    }

    T_GridValidationVisual GetGridValidationVisual(T_GridPosition gp)
    {
        return _gridValidationVisuals[gp.x, gp.z];
    }

    void ClearAllGridValidationVisuals()
    {
        foreach (var visual in _gridValidationVisuals)
        {
            foreach (var item in visual.G_GetGridValidationVisualDictionary())
            {
                item.Value.enabled = false;
            }
        }
    }

    void ShowGridValidationVisuals(string visualName, List<T_GridPosition> gpList)
    {
        foreach (var gp in gpList)
        {
            GetGridValidationVisual(gp).G_SetGridVisual(visualName, true);
        }
    }

    #endregion 


    #region ========== Convert List GridPosition and WorldPosition ==========
    List<T_GridPosition> ConvertListWorldToGridPosition(List<Vector3> worldPositions)
    {
        List<T_GridPosition> girdPositions = new();
        foreach (var worldPos in worldPositions)
        {
            girdPositions.Add(G_WorldToGridPosition(worldPos));
        }
        return girdPositions;
    }
    List<Vector3> ConvertListGridToWorldPosition(List<T_GridPosition> gridPositions)
    {
        List<Vector3> worldPositions = new();
        foreach (var gridPos in gridPositions)
        {
            worldPositions.Add(G_GridToWorldPosition(gridPos));
        }
        return worldPositions;
    }
    #endregion 

    #region ================ Compare GridPosition Floor ================

    void GetGridPositionFloor(T_GridPosition a, T_GridPosition b, out int floorA, out int floorB)
    {
        floorA = G_GetGridPosData(a).GetTerrainListCount();
        floorB = G_GetGridPosData(b).GetTerrainListCount();
    }

    //* Compare 2 grid positions and output world position with lower grid position (X,Z) and upper grid position (Y)
    bool IsGridPositionsOnSameFloorHeight(T_GridPosition a, T_GridPosition b, out Vector3 gridOffsetWorldPos)
    {
        GetGridPositionFloor(a, b, out int floorA, out int floorB);

        if (floorA == floorB)
        {
            gridOffsetWorldPos = new Vector3(0, 0, 0);
            return true;
        }
        else if (floorA > floorB)
        {
            gridOffsetWorldPos = new Vector3(G_GridToWorldPosition(b).x, floorA, G_GridToWorldPosition(b).z);
            return false;
        }
        else
        {
            gridOffsetWorldPos = new Vector3(G_GridToWorldPosition(a).x, floorB, G_GridToWorldPosition(a).z);
            return false;
        }
    }

    bool IsAbleToClimbOver(T_GridPosition target, T_GridPosition start)
    {
        GetGridPositionFloor(target, start, out int floorA, out int floorB);

        if ((floorA - floorB) > 2) return false;
        else return true;
    }


    #endregion




}
