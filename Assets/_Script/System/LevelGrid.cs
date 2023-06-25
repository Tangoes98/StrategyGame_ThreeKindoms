using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance;
    GridSystem _gridSystem;
    [SerializeField] Transform _gridObjectVisual;

    [SerializeField] int _gridWidth;
    [SerializeField] int _gridHeight;
    [SerializeField] float _gridCellSize;


    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;

        _gridSystem = new GridSystem(_gridWidth, _gridHeight, _gridCellSize);
        _gridSystem.CreateGridObjectVisual(_gridObjectVisual);
    }
    void Start()
    {

    }

    public void AddUnitToGridObject(GridPosition gridPos, Unit unit)
    {
        GridObject gridObj = GetGridObject(gridPos);
        gridObj.AddUnit(unit);
    }
    public void RemoveUnitFromGridObject(GridPosition gridPos, Unit unit)
    {
        GridObject gridobj = GetGridObject(gridPos);
        gridobj.RemoveUnit(unit);
    }


    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);
    public GridObject GetGridObject(GridPosition gridPosition) => _gridSystem.GetGridObject(gridPosition);
    public Vector3 GetGridObjectWorldPosition(GridPosition gridPosition) => _gridSystem.GetGridObjectWorldPosition(gridPosition);
    public int GetGridObjectFloor(GridPosition gridPosition) => _gridSystem.GetGridFloorHeight(gridPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);
    public int GetWidth() => _gridWidth;
    public int GetHeight() => _gridHeight;
    public float GetCellSize() => _gridCellSize;

}
