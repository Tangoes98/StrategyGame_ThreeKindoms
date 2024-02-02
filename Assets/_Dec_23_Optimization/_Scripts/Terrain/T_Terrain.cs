using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Terrain : MonoBehaviour
{
    T_GridData _gridData;


    void Start()
    {
        AddTerrainToGridData();
    }

    #region ========= GridData =========

    //TODO: Get grid position and grid data info
    //TODO: Update grid data terrainList 

    //GridDataInitialization
    void AddTerrainToGridData()
    {
        T_LevelGridManager levelGrid = T_LevelGridManager.Instance;
        T_GirdPosition girPos = levelGrid.G_WorldToGridPosition(this.transform.position);
        _gridData = levelGrid.G_GetGridPosData(girPos);
        _gridData.AddTerrain(this);
    }

    #endregion ====================================



}
