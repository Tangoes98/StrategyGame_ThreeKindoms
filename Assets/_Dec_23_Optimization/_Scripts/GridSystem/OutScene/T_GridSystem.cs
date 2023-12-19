using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class T_GridSystem
{
    int _width, _height;
    float _cellSize;
    T_GridData[,] _gridDatas;


    public T_GridSystem(int width, int height, float cellSize)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._gridDatas = new T_GridData[width, height];


        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                var gridPosition = new T_GirdPosition(i, j);
                _gridDatas[i, j] = new T_GridData(gridPosition);










            }
        }
    }

    public Vector3 GridToWorldPosition(T_GirdPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
    }

    public T_GirdPosition WorldToGridPosition(Vector3 worldPosition)
    {
        return new T_GirdPosition(Mathf.RoundToInt(worldPosition.x / _cellSize), Mathf.RoundToInt(worldPosition.z / _cellSize));
    }

    public T_GridData GetGridData(T_GirdPosition gridPosition)
    {
        return _gridDatas[gridPosition.x, gridPosition.z];
    }



    public void CreateGridVisual(Transform objectVisualPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                var gridPosition = new T_GirdPosition(x, z);

                Transform gridObjectPrefab = GameObject.Instantiate(objectVisualPrefab, GridToWorldPosition(gridPosition), Quaternion.identity);

                SetGridData(gridObjectPrefab, gridPosition);
            }
        }
    }

    void SetGridData(Transform gridObject, T_GirdPosition gridPosition)
    {
        gridObject.GetComponentInChildren<T_GridDataVisual>().SetIndividualGridData(GetGridData(gridPosition));
    }














}
