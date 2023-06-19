using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    GridSystem _gridSystem;
    GridPosition _gridPosition;
    List<Unit> _unitList;
    Unit _unit;
    int _floorNumber;


    public GridObject(GridSystem gridSystem, GridPosition gridPosition, int floor)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _floorNumber = floor;
        _unitList = new List<Unit>();

    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in _unitList)
        {
            unitString += unit + "\n";
        }

        return
            _gridPosition.ToString()
            + "\n"
            + $"Floor: {_floorNumber}"
            + "\n"
            + unitString;
    }

    public void AddUnit(Unit unit)
    {
        _unitList.Add(unit);
    }
    public void RemoveUnit(Unit unit)
    {
        _unitList.Remove(unit);
    }


    public int GetFloorNumber() => _floorNumber;
}
