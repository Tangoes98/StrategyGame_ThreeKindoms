using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_GridData
{
    T_GridPosition _gridPosition;
    List<T_Unit> _unitList;
    List<T_Terrain> _terrainList = new();
    T_Terrain _surfaceTerrain;



    // public T_GirdPosition GridPosition { get { return _gridPosition; } }




    public T_GridData(T_GridPosition gp)
    {
        this._gridPosition = gp;
        this._unitList = new();


    }


    public override string ToString()
    {
        string unitString = "";
        foreach (T_Unit item in _unitList)
        {
            unitString += item + "\n";
        }

        return _gridPosition.ToString()
            + "\n"
            + unitString
            + "Floor: " + _terrainList.Count;
    }


    public void AddUnit(T_Unit unit) => _unitList.Add(unit);

    public void RemoveUnit(T_Unit unit) => _unitList.Remove(unit);
    public List<T_Unit> G_GetUnitList() => _unitList;

    public void AddTerrain(T_Terrain terrain) => _terrainList.Add(terrain);
    public void RemoveTerrain(T_Terrain terrain) => _terrainList.Remove(terrain);

    public T_Terrain GetSurfaceTerrain()
    {
        SurfaceTerrain();
        return _surfaceTerrain;
    }
    public void SurfaceTerrain()
    {
        foreach (var terrain in _terrainList)
        {
            if (terrain is not T_TerrainBase)
                _surfaceTerrain = terrain;
        }
    }


    /// <summary>
    /// Grid position floor height == Terrain list count
    /// </summary>
    /// <returns></returns>
    public int GetTerrainListCount() => _terrainList.Count;





}
