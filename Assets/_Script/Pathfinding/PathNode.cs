using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    GridPosition _gridPosition;
    int _gCost;
    int _hCost;
    int _fCost;
    PathNode _cameFromNode;


    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;

    }


    public override string ToString()
    {
        return
            _gridPosition.ToString();
    }

    public GridPosition GetGridPosition() => _gridPosition;
    public int GetGCost() => _gCost;
    public int GetHCost() => _hCost;
    public int GetFCost() => _fCost;

    public void SetGCost(int gCost) => _gCost = gCost;
    public void SetHCost(int hCost) => _hCost = hCost;

    public void CalculateFCost() => _fCost = _gCost + _hCost;

    public PathNode GetCameFromNode() => _cameFromNode;
    public void SetCameFromNode(PathNode cameFromNode) => _cameFromNode = cameFromNode;
    public void ResetCameFromNode() => _cameFromNode = null;
}
