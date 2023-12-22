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


    #region Public Access

    public int GetGridWidth() => _gridWidth;
    public int GetGridHeight() => _gridHeight;

    public Vector3 GridToWorldPosition(T_GirdPosition gridPosition) => _gridSystem.GridToWorldPosition(gridPosition);
    public T_GirdPosition WorldToGridPosition(Vector3 worldPosition) => _gridSystem.WorldToGridPosition(worldPosition);
    public T_GridData GetGridPosData(T_GirdPosition gridPosition) => _gridSystem.GetGridData(gridPosition);
    public T_Pathnode GetGridPosPathNode(T_GirdPosition gridPosition) => _gridSystem.GetPathnode(gridPosition);
    





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
    }

    void Start()
    {


    }






}
