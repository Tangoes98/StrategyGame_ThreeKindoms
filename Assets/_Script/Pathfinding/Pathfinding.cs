using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    public static Pathfinding Instance;
    GridSystem _gridSystem;
    [SerializeField] Transform _pathNodeVisual;

    int _gridWidth;
    int _gridHeight;
    float _gridCellSize;


    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
        _gridWidth = LevelGrid.Instance.GetWidth();
        _gridHeight = LevelGrid.Instance.GetHeight();
        _gridCellSize = LevelGrid.Instance.GetCellSize();

        _gridSystem = new GridSystem(_gridWidth, _gridHeight, _gridCellSize);

        _gridSystem.CreatePathNodeVisual(_pathNodeVisual);
    }
}
