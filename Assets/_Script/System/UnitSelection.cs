using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UnitSelection : MonoBehaviour
{
    public static UnitSelection Instance;
    public event Action OnUnitSelecedChanged;
    public event Action OnSelectEmpty;
    [SerializeField] LayerMask _unitLayerMask;
    [SerializeField] Transform _selectedUnit;

    UnitBaseAction _selectedAction;

    [SerializeField] bool _isBusy;

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
        // If any actions on going, no inputs allowed
        if (_isBusy) return;

        // If mouse is over UI elements, return
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Left mouse click to select and deselect unit
        if (Input.GetMouseButtonDown(0))
        {
            GetUnitSelection();
        }

        HandleUnitActionSelection();

        // Right mouse click to move
        // if (Input.GetMouseButtonDown(1))
        // {
        //     if (_selectedUnit == null) return;

        //     GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseToWorld.Instance.GetMouseWorldPosition());

        //     if (_selectedAction.IsValidActionGridPosition(mouseGridPosition))
        //     {
        //         SetBusy();
        //         _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
        //     }

        // }

        Debug.Log(_selectedAction);
    }

    void GetUnitSelection()
    {
        // cast ray to Unitlayer to see if his a unit
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _unitLayerMask))
        {
            SetSelectedUnit(hit.transform);
            //GetUnitMovementAction(_selectedUnit).ShowReachableGridPosition();
        }
        else
        {
            //if (_selectedUnit != null) _selectedUnit.GetComponentInChildren<UnitSelectedVisual>().OnSelected(false);
            _selectedUnit = null;
            _selectedAction = null;
            OnSelectEmpty?.Invoke();
        }
    }



    void SetBusy() => _isBusy = true;
    void ClearBusy() => _isBusy = false;


    void HandleUnitActionSelection()
    {
        if (!UnitIsSelected()) return; // return if not selecting any unit
        if (!_selectedAction) return; // retunr if not selecting any action

        if (Input.GetMouseButtonDown(1))
        {
            if (_selectedUnit == null) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseToWorld.Instance.GetMouseWorldPosition());

            if (_selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }

        }

        // switch (_selectedAction)
        // {
        //     case UnitMovementAction moveAction:
        //         GetUnitMovementAction(_selectedUnit).ShowReachableGridPosition();

        //         break;
        //     case UnitAttackAction attackAction:
        //         //Debug.Log("Attack");
        //         break;
        //     case UnitSelfTriggerAction specialAction:
        //         //Debug.Log("Special");
        //         break;
        // }
    }



    void SetSelectedUnit(Transform unit)
    {
        _selectedUnit = unit;
        _selectedAction = null;
        OnUnitSelecedChanged?.Invoke();
    }

    public Transform GetSelectedUnit()
    {
        if (_selectedUnit != null) return _selectedUnit;
        else return null;
    }

    public bool UnitIsSelected()
    {
        if (_selectedUnit == null) return false;
        return true;
    }

    public void SetSelectedAction(UnitBaseAction baseAction)
    {
        _selectedAction = baseAction;
    }

    public UnitBaseAction GetUnitCurrentAction() => _selectedAction;

    UnitMovementAction GetUnitMovementAction(Transform selectedUnit)
    {
        if (selectedUnit.TryGetComponent<UnitMovementAction>(out UnitMovementAction unitMovement))
        {
            return unitMovement;
        }
        else return null;
    }

}

