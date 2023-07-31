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


    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);
    public GridObject GetGridObject(GridPosition gridPosition) => _gridSystem.GetGridObject(gridPosition);
    public Vector3 GetWorldPositionWithHeight(GridPosition gridPosition) => _gridSystem.GetGridObjectWorldPosition(gridPosition);
    public int GetGridObjectFloor(GridPosition gridPosition) => _gridSystem.GetGridFloorHeight(gridPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);
    public GridSystem GetGridSystem() => _gridSystem;

    public int GetWidth() => _gridWidth;
    public int GetHeight() => _gridHeight;
    public float GetCellSize() => _gridCellSize;

    public int GetGridFloorHeight(GridPosition gridPosition) => GetGridObject(gridPosition).GetFloorNumber();

    public void AddUnitToGridObject(GridPosition gridPosition, Unit unit) => GetGridObject(gridPosition).AddUnit(unit);
    public void RemoveUnitFromGridObject(GridPosition gridPosition, Unit unit) => GetGridObject(gridPosition).RemoveUnit(unit);
    public bool HasUnitOnGridPosition(GridPosition gridPosition) => GetGridObject(gridPosition).HasUnitOnGrid();
    public Unit GetUnitAtGridPosition(GridPosition gridPosition) => GetGridObject(gridPosition).GetUnit();
    public bool HasFriendlyUnitOnGridPosition(GridPosition gridPosition) => !GetUnitAtGridPosition(gridPosition).IsEnemyUnit();
    public bool HasEnemyUnitOnGridPosition(GridPosition gridPosition) => GetUnitAtGridPosition(gridPosition).IsEnemyUnit();

    public void AddConstructionToGrdObject(GridPosition gridPosition, Construction construction) => GetGridObject(gridPosition).AddConstruction(construction);
    public void RemoveConstructionFromGrdObject(GridPosition gridPosition, Construction construction) => GetGridObject(gridPosition).RemoveConstruction(construction);
    public bool HasConstructionOnGridPosition(GridPosition gridPosition) => GetGridObject(gridPosition).HasConstructionOnGrid();
    public Construction GetConstructionAtGridPosition(GridPosition gridPosition) => GetGridObject(gridPosition).GetConstructionObject();

}
