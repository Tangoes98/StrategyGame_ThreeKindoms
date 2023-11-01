using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNodeTerrain : MonoBehaviour
{
    const int PathfindingDistanceMultiplier = 10;
    int _width;
    int _height;

    void Awake()
    {

        _width = LevelGrid.Instance.GetWidth();
        _height = LevelGrid.Instance.GetHeight();

        UpdateGridNodeTerrain();

    }

    void UpdateGridNodeTerrain()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                PathNode pathNode = Pathfinding.Instance.GetNode(gridPosition);

                TerrainType terrainType = RayCastTerrainObject(gridPosition);

                if (!terrainType) Debug.Log("no terrain found");

                pathNode.SetTerrain(terrainType);

                int moveCost = terrainType.GetTerrainMoveCost(terrainType.GetTerrainType());

                if (moveCost == 0) pathNode.SetIsWalkable(false);

                pathNode.SetOriginalMoveCost(moveCost * PathfindingDistanceMultiplier);

                pathNode.SetMoveCost(moveCost * PathfindingDistanceMultiplier);


            }
        }
    }


    TerrainType RayCastTerrainObject(GridPosition gridPosition)
    {
        int raycastOffset = 2;
        RaycastHit[] raycastHits = Physics.RaycastAll(LevelGrid.Instance.GetWorldPosition(gridPosition) + Vector3.down * raycastOffset,
                                                        Vector3.up, float.MaxValue,
                                                        LayerMask.GetMask("EnvironmentGameObject"));

        //Debug.DrawLine(GetWorldPosition(gridPosition) + Vector3.down * raycastOffset, GetWorldPosition(gridPosition) + Vector3.up * int.MaxValue, Color.white, 900f);

        if (raycastHits.Length == 0) return null;

        int lastRaycastHitObjectIndex = raycastHits.Length - 1;
        return raycastHits[lastRaycastHitObjectIndex].transform.GetComponent<TerrainType>();
    }
}
