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




    #region ========== Public Methods =================

    public int GetGridWidth() => _gridWidth;
    public int GetGridHeight() => _gridHeight;

    public Vector3 G_GridToWorldPosition(T_GirdPosition gridPosition) => _gridSystem.GridToWorldPosition(gridPosition);
    public T_GirdPosition G_WorldToGridPosition(Vector3 worldPosition) => _gridSystem.WorldToGridPosition(worldPosition);
    public T_GridData G_GetGridPosData(T_GirdPosition gridPosition) => _gridSystem.GetGridData(gridPosition);
    public T_Pathnode G_GetGridPosPathNode(T_GirdPosition gridPosition) => _gridSystem.GetPathnode(gridPosition);

    public T_GridValidationVisual G_GetGridValidationVisual(T_GirdPosition gp) => GetGridValidationVisual(gp);
    public void G_ShowGridValidationVisual_Move(List<T_GirdPosition> gpList) => ShowGridValidationVisual_Move(gpList);
    public void G_ShowGridValidationVisual_Target(List<T_GirdPosition> gpList) => ShowGridValidationVisual_Target(gpList);




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

        _gridSystem.CreateGridVisual(_gridObejctVisual);

        InitializeGridValidationVisuals(_gridValidationVisualObject);
    }

    void Start()
    {

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
                Transform obj = GameObject.Instantiate(visualObject, G_GridToWorldPosition(gridPosition), Quaternion.identity);

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

    void ShowGridValidationVisual_Move(List<T_GirdPosition> gpList)
    {
        ClearAllGridValidationVisuals();
        foreach (var gp in gpList)
        {
            GetGridValidationVisual(gp).G_SetMoveGridVisual(true);
        }
    }
    void ShowGridValidationVisual_Target(List<T_GirdPosition> gpList)
    {
        ClearAllGridValidationVisuals();
        foreach (var gp in gpList)
        {
            GetGridValidationVisual(gp).G_SetTargetGridVisual(true);
        }
    }

    #endregion




}
