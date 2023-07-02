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

    public int GetGCost() => _gCost;
    public int GetHCost() => _hCost;
    public int GetFCost() => _fCost;
}
