using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitMovementAction : UnitBaseAction
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _stopDistance;
    //Vector3 _targetPosition;
    [SerializeField] int _unitMaxMoveDistance;
    [SerializeField] int _actionCost;

    List<Vector3> _targetPositionList;
    int _currentPositionIndex;



    protected override void Awake()
    {
        base.Awake();
        //_targetPosition = transform.position;
        _targetPositionList = new List<Vector3>();


    }

    protected override void Update()
    {
        base.Update();

        if (!_isActive) return;

        Move(_targetPositionList);

        HeightCheck();

    }



    public override int GetActionCost()
    {
        return _actionCost;
    }



    public void SetTargetPositionList(List<Vector3> targetPositionList) => _targetPositionList = targetPositionList;

    void Move(List<Vector3> targetPositionList)
    {
        Vector3 unitHorizontalPosition = new Vector3(transform.position.x, 0, transform.position.z);

        Vector3 targetPosition = targetPositionList[_currentPositionIndex];

        if (Vector3.Distance(targetPosition, unitHorizontalPosition) > _stopDistance)
        {
            //set the direction where unit move to
            Vector3 targetDirection = (targetPosition - unitHorizontalPosition).normalized;

            // unit movement to target direction
            transform.position += targetDirection * _moveSpeed * Time.deltaTime;

            // unit rotation animation
            transform.forward = Vector3.Slerp(transform.forward, targetDirection, Time.deltaTime * _rotateSpeed);
        }
        else
        {
            _currentPositionIndex++;

            if (_currentPositionIndex >= _targetPositionList.Count)
            {
                ActionCompleted();
            }
        }
    }


    #region // Simple unit movement functions

    //public void SetTargetPosition(Vector3 targetPosition) => _targetPosition = targetPosition;

    // void Move(Vector3 targetPosition)
    // {
    //     Vector3 unitHorizontalPosition = new Vector3(transform.position.x, 0, transform.position.z);

    //     if (Vector3.Distance(targetPosition, unitHorizontalPosition) > _stopDistance)
    //     {
    //         //set the direction where unit move to
    //         Vector3 targetDirection = (targetPosition - unitHorizontalPosition).normalized;

    //         // unit movement to target direction
    //         transform.position += targetDirection * _moveSpeed * Time.deltaTime;

    //         // unit rotation animation
    //         transform.forward = Vector3.Slerp(transform.forward, targetDirection, Time.deltaTime * _rotateSpeed);
    //     }
    //     else
    //     {
    //             ActionCompleted();
    //     }
    // }

    #endregion


    // Update unit heicht
    void HeightCheck()
    {
        //GridPosition unitGridPosition = _unit.GetUnitGridPosition();
        Vector3 gridWorldPosition = LevelGrid.Instance.GetGridObjectWorldPosition(_unitGridPosition);

        if (transform.position.y != gridWorldPosition.y) transform.position = gridWorldPosition;
    }


    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        //GridPosition unitGridPosition = _unit.GetUnitGridPosition();

        for (int x = -_unitMaxMoveDistance; x <= _unitMaxMoveDistance; x++)
        {
            for (int z = -_unitMaxMoveDistance; z <= _unitMaxMoveDistance; z++)
            {
                // Unit will not move over than max move distance
                int moveDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (moveDistance > _unitMaxMoveDistance) continue;

                // Get ideal moveable gridposiiton
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition ValidGridposition = offsetGridPosition + _unitGridPosition;

                // Check if the girdPosition is inside the entire gridSystem
                if (!LevelGrid.Instance.IsValidGridPosition(ValidGridposition)) continue;

                // Check if gridposition is walkable
                if (!Pathfinding.Instance.IsWalkableGridPosition(ValidGridposition)) continue;

                // Check if gridposition is reachable
                if (!Pathfinding.Instance.HasPathToGridPosition(_unitGridPosition, ValidGridposition)) continue;

                // Check if the path is too long
                int pathfindingDistanceMultiplier = 10;
                if (Pathfinding.Instance.GetPathLength(_unitGridPosition, ValidGridposition) > _unitMaxMoveDistance * pathfindingDistanceMultiplier) continue;

                // Check if is valid gridposition based on terrain moveCost check
                if(!Pathfinding.Instance.GetValidMoveGridPoisitionList(_unitGridPosition, _unitMaxMoveDistance).Contains(ValidGridposition)) continue;



                //Debug.Log(reachableGridposition);

                validGridPositionList.Add(ValidGridposition);

            }
        }
        return validGridPositionList;
    }


    public override string GetActionName() => "Test_Move";
    public override void TakeAction(GridPosition gridPosition, Action onActionCompleted)
    {
        _isActive = true;

        this._onActionCompleted = onActionCompleted;

        if (!IsValidActionGridPosition(gridPosition)) return;

        // _targetPositionList = new List<Vector3>();

        _currentPositionIndex = 0;

        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(_unitGridPosition, gridPosition, out int pathLength);

        SetTargetPositionList(ConvertPathList(pathGridPositionList));



        // set only one gridposition as target position

        // Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

        // SetTargetPosition(worldPosition);
    }

    List<Vector3> ConvertPathList(List<GridPosition> gridPositionList)
    {
        List<Vector3> worldPositionList = new List<Vector3>();

        foreach (GridPosition gridPosition in gridPositionList)
        {
            worldPositionList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        return worldPositionList;
    }



    #region // Old SelectedUnitMoveMent Code 

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
