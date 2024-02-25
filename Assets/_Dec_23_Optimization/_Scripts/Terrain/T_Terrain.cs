using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class T_Terrain : MonoBehaviour
{
    protected T_GridData _gridData;
    protected T_Pathnode _gridPathNode;
    [SerializeField] protected int _terrainMoveCost;


    protected virtual void Start()
    {
        AddTerrainToGridData();
    }
    protected virtual void Update()
    {
    }

    #region ========= Generics =============

    public T G_GetChildTerrainType<T>() where T : T_Terrain => (T)this;

    #endregion

    #region ========= GridData =========

    //TODO: Get grid position and grid data info
    //TODO: Update grid data terrainList 

    //GridDataInitialization
    void AddTerrainToGridData()
    {
        T_LevelGridManager levelGrid = T_LevelGridManager.Instance;
        T_GirdPosition girPos = levelGrid.G_WorldToGridPosition(this.transform.position);
        _gridData = levelGrid.G_GetGridPosData(girPos);
        _gridPathNode = levelGrid.G_GetGridPosPathNode(girPos);
        _gridData.AddTerrain(this);
    }

    #endregion 




}
