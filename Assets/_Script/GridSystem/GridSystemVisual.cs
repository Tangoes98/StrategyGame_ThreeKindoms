using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance;

    [SerializeField] Transform _gridSystemVisualPrefab;

    //List<Transform> _singleGridVisualList = new List<Transform>();
    Transform[,] _singleGridVisualArray;

    int _gridWidth;
    int _gridHeight;

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
        _gridWidth = LevelGrid.Instance.GetWidth();
        _gridHeight = LevelGrid.Instance.GetHeight();

        _singleGridVisualArray = new Transform[_gridWidth, _gridHeight];

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int z = 0; z < _gridHeight; z++)
            {
                GridPosition gridPos = new GridPosition(x, z);

                Transform singleVisual = Instantiate(_gridSystemVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPos), Quaternion.identity);

                _singleGridVisualArray[x, z] = singleVisual;

                //_singleGridVisualList.Add(singleVisual);
            }
        }

        HideAllGridPositionVisuals();
    }

    void Update()
    {
        UpdateGridSystemVisual();
    }


    public void HideAllGridPositionVisuals()
    {
        // foreach (Transform singleGridVisual in _singleGridVisualList)
        // {
        //     MeshRenderer meshRed = singleGridVisual.GetComponentInChildren<MeshRenderer>();
        //     meshRed.enabled = false;
        // }


        for (int x = 0; x < _gridWidth; x++)
        {
            for (int z = 0; z < _gridHeight; z++)
            {
                MeshRenderer meshRend = _singleGridVisualArray[x, z].GetComponentInChildren<MeshRenderer>();
                meshRend.enabled = false;
            }
        }

    }

    public void ShowGridPositionVisuals(List<GridPosition> gridPosList)
    {
        foreach (GridPosition gridPos in gridPosList)
        {
            Transform singleVisualObejct = GetSingelGridVisualObject(gridPos);
            MeshRenderer meshRend = singleVisualObejct.GetComponentInChildren<MeshRenderer>();
            meshRend.enabled = true;
        }
    }
    Transform GetSingelGridVisualObject(GridPosition gridPos) => _singleGridVisualArray[gridPos.x, gridPos.z];

    void UpdateGridSystemVisual()
    {
        HideAllGridPositionVisuals();

        if (UnitSelection.Instance.UnitIsSelected())
        {
            ShowGridPositionVisuals(UnitSelection.Instance.GetSelectedUnit().GetComponent<UnitMovementAction>().GetValidGridPositionList());
        }
    }



}
