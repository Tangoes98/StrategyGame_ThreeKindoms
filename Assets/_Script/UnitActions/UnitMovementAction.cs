using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitMovementAction : UnitBaseAction
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _stopDistance;
    Vector3 _targetPosition;
    [SerializeField] int _UnitMaxMoveDistance;



    protected override void Awake()
    {
        base.Awake();
        _targetPosition = transform.position;

    }

    void Update()
    {
        if (!_isActive) return;

        Move(_targetPosition);
    }

    void Move(Vector3 targetPosition)
    {
        if (Vector3.Distance(targetPosition, transform.position) > _stopDistance)
        {
            //set the direction where unit move to
            Vector3 targetDirection = (targetPosition - this.transform.position).normalized;

            // unit movement to target direction
            transform.position += targetDirection * _moveSpeed * Time.deltaTime;

            // unit rotation animation
            transform.forward = Vector3.Lerp(transform.forward, targetDirection, Time.deltaTime * _rotateSpeed);
        }
        else ActionCompleted();
    }

    public void SetTargetPosition(Vector3 targetPosition) => _targetPosition = targetPosition;

    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetUnitGridPosition();

        for (int x = -_UnitMaxMoveDistance; x <= _UnitMaxMoveDistance; x++)
        {
            for (int z = -_UnitMaxMoveDistance; z <= _UnitMaxMoveDistance; z++)
            {
                // Unit will not move over than max move distance
                int moveDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (moveDistance > _UnitMaxMoveDistance) continue;

                // Get ideal moveable gridposiiton
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition reachableGridposition = offsetGridPosition + unitGridPosition;

                // Check if the girdPosition is inside the entire gridSystem
                if (!LevelGrid.Instance.IsValidGridPosition(reachableGridposition)) continue;

                //Debug.Log(reachableGridposition);

                validGridPositionList.Add(reachableGridposition);

            }
        }
        return validGridPositionList;
    }

    public override string GetActionName() => "Test_Move";



    public override void TakeAction(GridPosition gridPos, Action onActionCompleted)
    {
        _isActive = true;

        this._onActionCompleted = onActionCompleted;

        if (!IsValidActionGridPosition(gridPos)) return;

        Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPos);

        SetTargetPosition(worldPosition);
    }



    #region Old SelectedUnitMoveMent Code // Reference use only

    // public void SelectedUnitMovement(Action onActionCompleted)
    // {
    //     this._onActionCompleted = onActionCompleted;

    //     Transform _selectedUnit = UnitSelection.Instance.GetSelectedUnit();

    //     // try to get Unitmovement class from selected unit
    //     if (_selectedUnit.TryGetComponent<UnitMovementAction>(out UnitMovementAction unitMovement))
    //     {
    //         unitMovement.SetTargetPosition(TargetMovePosition());
    //     }
    //     else return;
    // }

    // // Set target MovePosition to gridPosition
    // Vector3 TargetMovePosition()
    // {
    //     Transform _selectedUnit = UnitSelection.Instance.GetSelectedUnit();

    //     Vector3 mousePosition = MouseToWorld.Instance.GetMouseWorldPosition();

    //     GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(mousePosition);

    //     List<GridPosition> validGridPositionList = _selectedUnit.GetComponent<UnitMovementAction>().GetValidGridPositionList();

    //     if (!validGridPositionList.Contains(gridPosition)) return _selectedUnit.position;

    //     return LevelGrid.Instance.GetWorldPosition(gridPosition);
    // }

    // public void ShowReachableGridPosition()
    // {
    //     Transform _selectedUnit = UnitSelection.Instance.GetSelectedUnit();

    //     if (_selectedUnit == null) return;

    //     List<GridPosition> gridPositionList = _selectedUnit.GetComponent<UnitMovementAction>().GetValidGridPositionList();

    //     GridSystemVisual.Instance.UpdateGridSystemVisual();
    // }
    #endregion


}
