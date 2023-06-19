using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitSelection : MonoBehaviour
{
    public static UnitSelection Instance;
    public event EventHandler OnUnitSelecedChanged;

    [SerializeField] LayerMask _unitLayerMask;
    [SerializeField] Transform _selectedUnit;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }


    void Update()
    {
        // Left mouse click to select and deselect unit
        if (Input.GetMouseButtonDown(0))
        {
            GetUnitSelection();
            ShowReachableGridPosition();
        }

        // Right mouse click to move
        if (Input.GetMouseButtonDown(1))
        {
            if (_selectedUnit != null) SelectedUnitMovement();
        }
    }

    void GetUnitSelection()
    {
        // cast ray to Unitlayer to see if his a unit
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _unitLayerMask))
        {
            SetSelectedUnit(hit.transform);
        }
        else
        {
            if (_selectedUnit != null)
            {

                _selectedUnit.GetComponentInChildren<UnitSelectedVisual>().OnSelected(false);
            }
            _selectedUnit = null;
        }
    }

    void SetSelectedUnit(Transform unit)
    {
        _selectedUnit = unit;
        OnUnitSelecedChanged?.Invoke(this, EventArgs.Empty);
    }

    public Transform GetSelectedUnit()
    {
        if (_selectedUnit == null) return null;
        return _selectedUnit;
    }


    void SelectedUnitMovement()
    {
        // try to get Unitmovement class from selected unit
        if (_selectedUnit.TryGetComponent<UnitMovementAction>(out UnitMovementAction unitMovement))
        {
            unitMovement.SetTargetPosition(TargetMovePosition());
        }
        else return;
    }

    // Set target MovePosition to gridPosition
    Vector3 TargetMovePosition()
    {
        Vector3 mousePosition = MouseToWorld.Instance.GetMouseWorldPosition();

        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(mousePosition);

        List<GridPosition> validGridPositionList = _selectedUnit.GetComponent<UnitMovementAction>().GetValidGridPositionList();

        if (!validGridPositionList.Contains(gridPosition)) return _selectedUnit.position;

        return LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    void ShowReachableGridPosition()
    {
        if (_selectedUnit == null) return;

        List<GridPosition> gridPositionList = _selectedUnit.GetComponent<UnitMovementAction>().GetValidGridPositionList();

        GridSystemVisual.Instance.ShowGridPositionVisuals(gridPositionList);


        // foreach (GridPosition gridPos in gridPositionList)
        // {
        //     Debug.Log(gridPos);
        // }
    }

    public bool UnitIsSelected()
    {
        if (_selectedUnit == null) return false;
        return true;
    }
}

