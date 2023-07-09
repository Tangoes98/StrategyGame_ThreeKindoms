using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    const int PathfindingDistanceMultiplier = 10;
    GridPosition _gridPosition;
    int _gCost;
    int _hCost;
    int _fCost;
    PathNode _cameFromNode;
    bool _isWalkable = true;
    int _moveCost;
    int _accumulatedMoveDistance;

    TerrainType _terrainType;

    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;
        //_terrainType = terrainType;
        // _terrainType = GetRayCastTerrainObject(gridPosition);

        // var terrain = _terrainType.GetTerrainType();
        // _moveCost = _terrainType.GetTerrainMoveCost(terrain);

        // if (_moveCost == 0) _isWalkable = false;
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

    public int GetMoveCost() => _moveCost * PathfindingDistanceMultiplier;
    public void SetMoveCost(float moveCost) => _moveCost = Mathf.RoundToInt(moveCost);

    public int GetAccucmulatedMoveDistance() => _accumulatedMoveDistance;
    public void SetAccucmulatedMoveDistance(int a) => _accumulatedMoveDistance = a;

    public void SetGCost(int gCost) => _gCost = gCost;
    public void SetHCost(int hCost) => _hCost = hCost;

    public void CalculateFCost() => _fCost = _gCost + _hCost;

    public PathNode GetCameFromNode() => _cameFromNode;
    public void SetCameFromNode(PathNode cameFromNode) => _cameFromNode = cameFromNode;
    public void ResetCameFromNode() => _cameFromNode = null;

    public bool GetIsWalkable() => _isWalkable;
    public void SetIsWalkable(bool isWalkable) => _isWalkable = isWalkable;

    public TerrainType GetTerrain() => _terrainType;
    public void SetTerrain(TerrainType terrain) => _terrainType = terrain;

}
