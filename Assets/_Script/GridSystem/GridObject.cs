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

    List<Construction> _constructionList;


    public GridObject(GridSystem gridSystem, GridPosition gridPosition, int floor)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _floorNumber = floor;
        _unitList = new List<Unit>();
        _constructionList = new List<Construction>();

    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in _unitList)
        {
            unitString += unit + "\n";
        }

        // string constructionString = "";
        // foreach (Construction construction in _constructionList)
        // {
        //     constructionString += construction + "\n";
        // }

        return
            _gridPosition.ToString()
            + "\n"
            + $"Floor: {_floorNumber}"
            + "\n"
            + unitString;
        // + "\n"
        // + constructionString;
    }

    public void AddUnit(Unit unit) => _unitList.Add(unit);
    public void RemoveUnit(Unit unit) => _unitList.Remove(unit);
    public bool HasUnitOnGrid() => _unitList.Count > 0;
    public Unit GetUnit() => _unitList[0];

    public int GetFloorNumber() => _floorNumber;

    public void AddConstruction(Construction construction) => _constructionList.Add(construction);
    public void RemoveConstruction(Construction construction) => _constructionList.Remove(construction);
    public bool HasConstructionOnGrid() => _constructionList.Count > 0;
    public Construction GetConstructionObject() => _constructionList[0];
}
