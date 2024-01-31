using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_GridData
{
    T_GirdPosition _gridPosition;
    List<T_Unit> _unitList;
    public List<T_Unit> G_GetUnitList() => _unitList;


    public T_GirdPosition GridPosition { get { return _gridPosition; } }




    public T_GridData(T_GirdPosition gp)
    {
        this._gridPosition = gp;
        this._unitList = new();



    }

    public override string ToString()
    {
        string unitString = "";
        foreach (T_Unit item in _unitList)
        {
            unitString += item + "\n";
        }

        return _gridPosition.ToString()
            + "\n"
            + unitString;
    }


    public void AddUnit(T_Unit unit)
    {
        _unitList.Add(unit);
    }
    public void RemoveUnit(T_Unit unit)
    {
        _unitList.Remove(unit);
    }


}
