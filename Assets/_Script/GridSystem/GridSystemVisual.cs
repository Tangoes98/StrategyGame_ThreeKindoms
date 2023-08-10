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

    // bool _isMoveActionSelected;




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

                Vector3 worldPosition = LevelGrid.Instance.GetWorldPositionWithHeight(gridPos);

                Transform singleVisual = Instantiate(_gridSystemVisualPrefab, worldPosition, Quaternion.identity);

                _singleGridVisualArray[x, z] = singleVisual;

                //_singleGridVisualList.Add(singleVisual);
            }
        }

        HideAllGridPositionVisuals();

        UnitSelection.Instance.OnUnitSelecedChanged += UnitSelection_OnUnitSelecedChanged;
        UnitSelection.Instance.OnSelectEmpty += UnitSelection_OnSelectEmpty;
    }

    void Update()
    {
        UpdateGridSystemVisual();
    }




    void UnitSelection_OnSelectEmpty() => HideAllGridPositionVisuals();
    void UnitSelection_OnUnitSelecedChanged() => HideAllGridPositionVisuals();
    Transform GetSingelGridVisualObject(GridPosition gridPos) => _singleGridVisualArray[gridPos.x, gridPos.z];


    void UpdateGridSystemVisual()
    {
        HideAllGridPositionVisuals();

        UnitBaseAction baseAction = UnitSelection.Instance.GetUnitCurrentAction();

        if (!baseAction) return;

        ShowValidGridPositionVisuals(baseAction.GetValidGridPositionList());

        if (baseAction is UnitMovementAction)
        {
            ShowUnitMovePathValidation(baseAction);
        }

    }

    void ShowUnitMovePathValidation(UnitBaseAction baseAction)
    {
        UnitMovementAction moveAction = (UnitMovementAction)baseAction;
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseToWorld.Instance.GetMouseWorldPosition());
        if (!moveAction.IsValidActionGridPosition(mouseGridPosition)) return;
        List<GridPosition> movePathList = moveAction.GetPredictedMovePathGridPositionList(mouseGridPosition);
        ShowPredictedUnitMovePath(movePathList);
    }







    public void HideAllGridPositionVisuals()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int z = 0; z < _gridHeight; z++)
            {
                // MeshRenderer[] meshRendArray = _singleGridVisualArray[x, z].GetComponentsInChildren<MeshRenderer>();
                // foreach (MeshRenderer meshRenderer in meshRendArray) meshRenderer.enabled = false;
                MeshRenderer meshRend = _singleGridVisualArray[x, z].GetComponentInChildren<MeshRenderer>();
                meshRend.enabled = false;
            }
        }

    }


    void ShowValidGridPositionVisuals(List<GridPosition> gridPosList)
    {
        foreach (GridPosition gridPos in gridPosList)
        {
            EnableGridVisualObjectMeshrenderer(gridPos, GridPositionVisual.GridPositionVisualEnum.ValidGridPosition);
        }
    }




    public void ShowGridpositionRangeVisual(List<GridPosition> gridPosList)
    {
        foreach (GridPosition gridPos in gridPosList)
        {
            EnableGridVisualObjectMeshrenderer(gridPos, GridPositionVisual.GridPositionVisualEnum.RangeGridPosition);
        }
    }


    void ShowPredictedUnitMovePath(List<GridPosition> gridPosList)
    {
        foreach (GridPosition gridPos in gridPosList)
        {
            EnableGridVisualObjectMeshrenderer(gridPos, GridPositionVisual.GridPositionVisualEnum.MovePathGridPosition);
        }
    }











    void EnableGridVisualObjectMeshrenderer(GridPosition gridPosition, GridPositionVisual.GridPositionVisualEnum gridVisualType)
    {
        Transform singleVisualObejct = GetSingelGridVisualObject(gridPosition);
        Transform sigleVisual = singleVisualObejct.GetChild(0);

        MeshRenderer meshRend = sigleVisual.GetComponent<MeshRenderer>();
        meshRend.enabled = true;

        GridPositionVisual gridPositionVisual = sigleVisual.GetComponent<GridPositionVisual>();
        gridPositionVisual.SetGridPositionVisualType(gridVisualType);
    }
}
