using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    int _width;
    int _height;
    float _cellSize;
    GridObject[,] _gridObjectArray;
    PathNode[,] _pathNodeArray;

    public GridSystem(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _gridObjectArray = new GridObject[width, height];
        _pathNodeArray = new PathNode[width, height];

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                int raycastOffset = 2;
                RaycastHit[] raycastHits = Physics.RaycastAll(GetWorldPosition(gridPosition) + Vector3.down * raycastOffset,
                                                                Vector3.up, float.MaxValue,
                                                                LayerMask.GetMask("EnvironmentGameObject"));

                //Debug.DrawLine(GetWorldPosition(gridPosition) + Vector3.down * raycastOffset, GetWorldPosition(gridPosition) + Vector3.up * int.MaxValue, Color.white, 900f);

                int floor = raycastHits.Length;

                int lastRaycastHitObjectIndex = raycastHits.Length - 1;

                TerrainType terrainObject = raycastHits[lastRaycastHitObjectIndex].transform.GetComponent<TerrainType>();



                _gridObjectArray[x, z] = new GridObject(this, gridPosition, floor);

                _pathNodeArray[x, z] = new PathNode(gridPosition, terrainObject);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
    }

    public Vector3 GetGridObjectWorldPosition(GridPosition gridPosition)
    {
        int floorHeight = 1;
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize
        + new Vector3(0, GetGridFloorHeight(gridPosition) * floorHeight, 0);
    }

    public int GetGridFloorHeight(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.x, gridPosition.z].GetFloorNumber();
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(Mathf.RoundToInt(worldPosition.x / _cellSize), Mathf.RoundToInt(worldPosition.z / _cellSize));
    }


    public void CreateGridObjectVisual(Transform objectVisualPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform gridObjectPrefab = GameObject.Instantiate(objectVisualPrefab, GetGridObjectWorldPosition(gridPosition), Quaternion.identity);
                GridObject gridObject = GetGridObject(gridPosition);
                gridObjectPrefab.GetComponent<GridObjectVisual>().SetGridObeject(gridObject);
            }
        }
    }
    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPos)
    {
        return gridPos.x >= 0 &&
                gridPos.z >= 0 &&
                gridPos.x < _width &&
                gridPos.z < _height;
    }


    #region Pathfinding functions

    public void CreatePathNodeVisual(Transform visualPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform gridObjectPrefab = GameObject.Instantiate(visualPrefab, GetGridObjectWorldPosition(gridPosition), Quaternion.identity);
                PathNode pathNode = GetPathNode(gridPosition);
                gridObjectPrefab.GetComponent<PathNodeVisual>().SetPathNode(pathNode);
            }
        }
    }

    public PathNode GetPathNode(GridPosition gridPosition)
    {
        return _pathNodeArray[gridPosition.x, gridPosition.z];
    }

    #endregion

}
