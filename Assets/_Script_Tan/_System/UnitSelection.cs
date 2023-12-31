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
    public event Action OnActionPointsSpend;
    [SerializeField] LayerMask _unitLayerMask;
    [SerializeField] Transform _selectedUnit;

    [SerializeField] UnitBaseAction _selectedAction;

    [SerializeField] bool _isBusy = false;










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

        // Check if its players turn
        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        // Left mouse click to select and deselect unit
        if (Input.GetMouseButtonDown(0))
        {
            GetUnitSelection();
        }

        HandleUnitActionSelection();



        #region //Right mouse click to move

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
        #endregion

        //Debug.Log(_isBusy);
    }

    #region Unit Selection & Unit Action Selection

    void GetUnitSelection()
    {
        // cast ray to Unitlayer to see if his a unit
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _unitLayerMask))
        {
            Transform hittedUnit = hit.transform;

            // return if trying select enemy unit
            //if (hittedUnit.GetComponent<Unit>().IsEnemyUnit()) return;

            SetSelectedUnit(hit.transform);
        }
        else
        {
            _selectedUnit = null;
            _selectedAction = null;
            OnSelectEmpty?.Invoke();
        }
    }


    void HandleUnitActionSelection()
    {
        // return if not selecting any unit
        if (!UnitIsSelected()) return;
        // retunr if not selecting any action
        if (!_selectedAction) return;

        if (Input.GetMouseButtonDown(1))
        {

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseToWorld.Instance.GetMouseWorldPosition());

            Unit unit = _selectedUnit.GetComponent<Unit>();

            // return if not selecting valid grid position
            if (!_selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;

            // return if not having enough action points to take action
            if (!unit.TrySpendActionPointsToTakeAction(_selectedAction)) return;


            if (_selectedAction is UnitBaseConstructAction)
            {
                Debug.Log("action is ConstructAction");
                UnitBaseConstructAction action = (UnitBaseConstructAction)_selectedAction;
                if (action.IsOverUseCount()) return;

                if (action.IsSpendingConstructionCost())
                {
                    if (!unit.TrySpendConstructionPoints(action)) return;
                }



            }

            SetBusy();

            _selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionPointsSpend?.Invoke();

        }
        #endregion


        #region // Use switch to implement selected action

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
        #endregion
    }

    void SetBusy() => _isBusy = true;
    void ClearBusy() => _isBusy = false;
    public bool IsBusy() => _isBusy;


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

    public void SetSelectedAction(UnitBaseAction baseAction) => _selectedAction = baseAction;

    public UnitBaseAction GetUnitCurrentAction() => _selectedAction;


}

