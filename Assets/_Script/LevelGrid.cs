using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance;
    GridSystem _gridSystem;
    [SerializeField] Transform _gridObjectVisual;


    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;

        _gridSystem = new GridSystem(10, 10, 2);
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


    public GridPosition GetGridPosition(Vector3 worldposition) => _gridSystem.GetGridPosition(worldposition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);
    public GridObject GetGridObject(GridPosition gridPosition) => _gridSystem.GetGridObject(gridPosition);

}
