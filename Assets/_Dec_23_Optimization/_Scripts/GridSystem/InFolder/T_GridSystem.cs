using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class T_GridSystem
{
    int _width, _height;
    float _cellSize;
    T_GridData[,] _gridDatas;
    T_Pathnode[,] _pathNodes;


    public T_GridSystem(int width, int height, float cellSize)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._gridDatas = new T_GridData[width, height];
        this._pathNodes = new T_Pathnode[width, height];


        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                var gridPosition = new T_GridPosition(i, j);
                _gridDatas[i, j] = new T_GridData(gridPosition);
                _pathNodes[i, j] = new T_Pathnode(gridPosition);










            }
        }
    }

    public Vector3 GridToWorldPosition(T_GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
    }

    // GridToWorldPosition with floor info
    public Vector3 GridToWorldPositionIncludesFloor(T_GridPosition gridPosition)
    {
        var girdData = GetGridData(gridPosition);
        int floorHeight = girdData.GetTerrainListCount();
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize + new Vector3(0, floorHeight, 0);
    }

    public T_GridPosition WorldToGridPosition(Vector3 worldPosition)
    {
        return new T_GridPosition(Mathf.RoundToInt(worldPosition.x / _cellSize), Mathf.RoundToInt(worldPosition.z / _cellSize));
    }

    public T_GridData GetGridData(T_GridPosition gridPosition)
    {
        return _gridDatas[gridPosition.x, gridPosition.z];
    }
    public T_Pathnode GetPathnode(T_GridPosition gridPosition)
    {
        return _pathNodes[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(T_GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
                gridPosition.z >= 0 &&
                gridPosition.x < _width &&
                gridPosition.z < _height;
    }

    public void CreateGridVisual(Transform objectVisualPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                var gridPosition = new T_GridPosition(x, z);

                Transform gridObjectPrefab = GameObject.Instantiate(objectVisualPrefab, GridToWorldPositionIncludesFloor(gridPosition), Quaternion.identity);

                SetGridData(gridObjectPrefab, gridPosition);
                SetPathNode(gridObjectPrefab, gridPosition);
            }
        }
    }

    void SetGridData(Transform gridObject, T_GridPosition gridPosition)
    {
        gridObject.GetComponent<T_GridDataVisual>().SetIndividualGridData(GetGridData(gridPosition));
    }
    void SetPathNode(Transform gridObject, T_GridPosition gridPosition)
    {
        gridObject.GetComponent<T_GridDataVisual>().SetPathNode(GetPathnode(gridPosition));
    }














}
