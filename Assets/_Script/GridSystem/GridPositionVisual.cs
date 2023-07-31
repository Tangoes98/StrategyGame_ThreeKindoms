using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridPositionVisual : MonoBehaviour
{
    public enum GridPositionVisualEnum
    {
        ValidGridPosition,
        RangeGridPosition,
        MovePathGridPosition,
    }

    [Serializable]
    public struct GridPositionVisualMaterial
    {
        public GridPositionVisualEnum gridPositionVisualType;
        public Material gridPositionVisualMaterial;

    }

    [SerializeField] List<GridPositionVisualMaterial> _gridPositionVisualList;


    [Header("GridPositionVisual")]
    [SerializeField] GridPositionVisualEnum _gridPositionVisualType;




    void Update()
    {
        UpdateGridPositionVisualMaterial(_gridPositionVisualType);
    }



    void UpdateGridPositionVisualMaterial(GridPositionVisualEnum gridVisualtype)
    {
        foreach (GridPositionVisualMaterial gridMaterial in _gridPositionVisualList)
        {
            if (gridMaterial.gridPositionVisualType == gridVisualtype)
            {
                transform.GetComponent<MeshRenderer>().material = gridMaterial.gridPositionVisualMaterial;
            }
        }
    }




    public GridPositionVisualEnum GetGridPositionVisualType() => _gridPositionVisualType;
    public void SetGridPositionVisualType(GridPositionVisualEnum gridVisualType) => _gridPositionVisualType = gridVisualType;










}
