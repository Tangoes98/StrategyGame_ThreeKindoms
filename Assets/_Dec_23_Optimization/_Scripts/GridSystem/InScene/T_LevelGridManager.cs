using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_LevelGridManager : MonoBehaviour
{
    public static T_LevelGridManager Instance;




    [SerializeField] int _gridWidth;
    [SerializeField] int _gridHeight;
    [SerializeField] float _gridCellSize;
    [SerializeField] Transform _gridObejctVisual;


    T_GridSystem _gridSystem;


    #region Public Access Functions

    #endregion

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;

    }

    void Start()
    {
        _gridSystem = new(_gridWidth, _gridHeight, _gridCellSize);

        _gridSystem.CreateGridVisual(_gridObejctVisual);


    }






}
