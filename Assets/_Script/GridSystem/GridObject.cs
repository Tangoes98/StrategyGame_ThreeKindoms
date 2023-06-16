using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    GridSystem _gridSystem;
    GridPosition _gridPosition;
    List<Unit> _unitList;
    Unit _unit;
    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
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

    public GridPosition GetGridPosition() => _gridPosition;
}
