using System.Collections;
using System.Collections.Generic;
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
    public bool G_IsValidSystemGrid(T_GirdPosition gp) => _gridSystem.IsValidGridPosition(gp);

    //* -------- Grid/World position --------
    public Vector3 G_GridToWorldPosition(T_GirdPosition gridPosition) => _gridSystem.GridToWorldPosition(gridPosition);
    public Vector3 G_GridToWorldPositionWithFloor(T_GirdPosition gridPosition) => _gridSystem.GridToWorldPositionIncludesFloor(gridPosition);
    public T_GirdPosition G_WorldToGridPosition(Vector3 worldPosition) => _gridSystem.WorldToGridPosition(worldPosition);
    public List<T_GirdPosition> G_ConvertListWorldToGridPosition(List<Vector3> worldPositions) => ConvertListWorldToGridPosition(worldPositions);
    public List<Vector3> G_ConvertListGridToWorldPosition(List<T_GirdPosition> gridPositions) => ConvertListGridToWorldPosition(gridPositions);
    public bool G_IsGridPositionsOnSameFloorHeight(T_GirdPosition a, T_GirdPosition b, out Vector3 gridOffsetWorldPos) => IsGridPositionsOnSameFloorHeight(a, b, out gridOffsetWorldPos);

    //* -------- Grid Data/ Grid Path node --------
    public T_GridData G_GetGridPosData(T_GirdPosition gridPosition) => _gridSystem.GetGridData(gridPosition);
    public T_Pathnode G_GetGridPosPathNode(T_GirdPosition gridPosition) => _gridSystem.GetPathnode(gridPosition);

    //* -------- Grid Validation Visual --------
    public T_GridValidationVisual G_GetGridValidationVisual(T_GirdPosition gp) => GetGridValidationVisual(gp);

    public void G_ClearAllGridValidationVisuals() => ClearAllGridValidationVisuals();

    /// <summary>
    /// String name: MOVE_GRID, ATTACK_RANGE, VALID_ATTACK
    /// </summary>
    /// <param name="visualName">MOVE_GRID, ATTACK_RANGE, VALID_ATTACK</param>
    /// <param name="gpList"></param>
    public void G_ShowGridValidationVisuals(string visualName, List<T_GirdPosition> gpList) => ShowGridValidationVisuals(visualName, gpList);








    #endregion ===================================================

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
        _gridSystem.CreateGridVisual(_gridObejctVisual);
        InitializeGridValidationVisuals(_gridValidationVisualObject);

    }







    #region ========== GRID VALIDATION VISUAL FUNCTIONS =================

    void InitializeGridValidationVisuals(Transform visualObject)
    {
        _gridValidationVisuals = new T_GridValidationVisual[_gridWidth, _gridHeight];
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                var gridPosition = new T_GirdPosition(i, j);

                Transform obj = Instantiate(visualObject, G_GridToWorldPositionWithFloor(gridPosition), Quaternion.identity);

                T_GridValidationVisual validationVisual = obj.GetComponent<T_GridValidationVisual>();

                _gridValidationVisuals[i, j] = validationVisual;
            }
        }
    }

    T_GridValidationVisual GetGridValidationVisual(T_GirdPosition gp)
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

    void ShowGridValidationVisuals(string visualName, List<T_GirdPosition> gpList)
    {
        foreach (var gp in gpList)
        {
            GetGridValidationVisual(gp).G_SetGridVisual(visualName, true);
        }
    }

    #endregion =========================================================


    #region ========== Convert List GridPosition and WorldPosition ==========
    List<T_GirdPosition> ConvertListWorldToGridPosition(List<Vector3> worldPositions)
    {
        List<T_GirdPosition> girdPositions = new();
        foreach (var worldPos in worldPositions)
        {
            girdPositions.Add(G_WorldToGridPosition(worldPos));
        }
        return girdPositions;
    }
    List<Vector3> ConvertListGridToWorldPosition(List<T_GirdPosition> gridPositions)
    {
        List<Vector3> worldPositions = new();
        foreach (var gridPos in gridPositions)
        {
            worldPositions.Add(G_GridToWorldPosition(gridPos));
        }
        return worldPositions;
    }
    #endregion =========================================================

    #region ================ Compare GridPosition Floor ================

    //* Compare 2 grid positions and output world position with lower grid position (X,Z) and upper grid position (Y)
    bool IsGridPositionsOnSameFloorHeight(T_GirdPosition a, T_GirdPosition b, out Vector3 gridOffsetWorldPos)
    {
        int floorA = G_GetGridPosData(a).GetTerrainListCount();
        int floorB = G_GetGridPosData(b).GetTerrainListCount();

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

    #endregion =========================================================



}
