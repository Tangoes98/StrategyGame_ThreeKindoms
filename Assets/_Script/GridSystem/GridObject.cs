using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    GridSystem _gridSystem;
    GridPosition _gridPosition;
    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
    }

    public GridPosition GetGridPosition() => _gridPosition;
}
