using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Pathnode
{
    int _FCost;
    int _GCost;
    int _HCost;
    T_GirdPosition _gridPosition;



    public T_Pathnode(T_GirdPosition gp)
    {
        this._gridPosition = gp;


    }


    #region Publice Properties

    public int GetFCost() => _FCost;
    public int GetGCost() => _GCost;
    public int GetHCost() => _HCost;

    public void CalculateFCost() => _FCost = _GCost + _HCost;
    public void SetGCost(int g) => _GCost = g;
    public void SetHCost(int h) => _HCost = h;

    public T_GirdPosition GetGridPosition() => _gridPosition;


    #endregion
}
