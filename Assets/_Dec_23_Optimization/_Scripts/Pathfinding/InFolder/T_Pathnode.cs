using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Pathnode
{
    int _FCost;
    int _GCost;
    int _HCost;
    T_GridPosition _gridPosition;
    int _terrainMoveCost;
    // int _gridFloorHeight;

    bool _isValid;



    public T_Pathnode(T_GridPosition gp)
    {
        this._gridPosition = gp;
        this._isValid = true;

    }


    #region Publice Properties

    public int G_GetFCost() => _FCost;
    public int G_GetGCost() => _GCost;
    public int G_GetHCost() => _HCost;
    public int G_GetTerrainMoveCost() => _terrainMoveCost;
    // public int G_GetFloorHeight() => _gridFloorHeight;

    public void G_CalculateFCost() => _FCost = _GCost + _HCost;
    public void G_SetGCost(int g) => _GCost = g;
    public void G_SetHCost(int h) => _HCost = h;
    public void G_SetTerrainMoveCost(int t) => _terrainMoveCost = t;
    // public void G_SetNodeFloorHeight(int value) => _gridFloorHeight = value;

    public T_GridPosition G_GetGridPosition() => _gridPosition;
    public void G_ResetCosts() => ResetCosts();

    public bool G_IsValidPathNode() => _isValid;
    public void G_SetIsValidPathNode(bool a) => _isValid = a;


    #endregion

    void ResetCosts()
    {
        _GCost = 0;
        _HCost = 0;
        _FCost = _GCost + _HCost;
    }





}
