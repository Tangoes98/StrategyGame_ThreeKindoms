using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TerrainType : MonoBehaviour
{

    public enum TerrainTypeEnum
    {
        Default,
        Forest,
        ForestBurning,
        Grassland,
        GrasslandDry,
        GrasslandBurning,
        LandAshe,
        LandMud,
        LandSoil,
        SnowThin,
        SnowHeavy,
        StreamShallow,
        StreamDeep
    };

    [Serializable]
    public struct TerrainTypeMaterial
    {
        public TerrainTypeEnum terrainType;
        public Material terrainMaterial;
        public int terrainMoveCost;
        public bool isflammable;
        public bool isWalkable;
    }

    [SerializeField] List<TerrainTypeMaterial> _terrainTypeMaterialList;

    [Header("TerrainInfo")]
    [SerializeField] TerrainTypeEnum _terrainType;
    [SerializeField] bool _isBurning;

    void Start()
    {

        UpdateTerrainMaterial(_terrainType);

    }

    void UpdateTerrainMaterial(TerrainTypeEnum terrainType)
    {
        foreach (TerrainTypeMaterial terrainTypeMaterial in _terrainTypeMaterialList)
        {
            if (terrainTypeMaterial.terrainType == terrainType)
            {
                transform.GetComponent<Renderer>().material = terrainTypeMaterial.terrainMaterial;
            }
        }
    }




    public TerrainTypeEnum GetTerrainType() => _terrainType;
    public int GetTerrainMoveCost(TerrainTypeEnum terrainType)
    {
        foreach (TerrainTypeMaterial terrainTypeMaterial in _terrainTypeMaterialList)
        {
            if (terrainTypeMaterial.terrainType == terrainType) return terrainTypeMaterial.terrainMoveCost;
        }

        Debug.LogError("TerrainType not abaliable");
        return 0;
    }







}
