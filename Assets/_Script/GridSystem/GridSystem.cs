using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    int _width;
    int _height;
    float _cellSize;
    GridObject[,] _gridObjectArray;

    public GridSystem(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _gridObjectArray = new GridObject[width, height];

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                _gridObjectArray[x, z] = new GridObject(this, gridPosition);

                //Debug.DrawLine(GetWorldPosition(gridPosition), GetWorldPosition(gridPosition) + Vector3.up * 5f, Color.white, 100f);

            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
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

                Transform gridObjectPrefab = GameObject.Instantiate(objectVisualPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridObject gridObject = GetGridObject(gridPosition);
                gridObjectPrefab.GetComponent<GridObjectVisual>().SetGridObeject(gridObject);
            }
        }
    }
    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.x, gridPosition.z];
    }
}
