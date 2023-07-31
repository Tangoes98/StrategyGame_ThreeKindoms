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
    [SerializeField] int _unitCurrentMoveDistance;
    [SerializeField] int _actionCost;

    List<Vector3> _targetPositionList;
    int _currentPositionIndex;
    const int PathfindingDistanceMultiplier = 10;



    protected override void Awake()
    {
        base.Awake();
        //_targetPosition = transform.position;
        _targetPositionList = new List<Vector3>();

        _unitCurrentMoveDistance = _unitMaxMoveDistance;

    }

    protected override void Update()
    {
        base.Update();

        if (!_isActive) return;

        Move(_targetPositionList);

        HeightCheck();

    }

    public override bool IsEnabled() => _isActionEnabled;


    #region Exposed Variables

    public override string GetActionName() => "Test_Move";
    public override int GetActionCost() => _actionCost;
    public int GetMaxMoveDistance() => _unitMaxMoveDistance;
    public void SetMoveDistance(int moveDistance) => _unitCurrentMoveDistance = moveDistance;






    #endregion


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







    public override void TakeAction(GridPosition mouseGridPosition, Action onActionCompleted)
    {
        _isActive = true;

        this._onActionCompleted = onActionCompleted;

        if (!IsValidActionGridPosition(mouseGridPosition)) return;

        // _targetPositionList = new List<Vector3>();

        _currentPositionIndex = 0;

        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(_unitGridPosition, mouseGridPosition, out int pathLength);

        UpdateMoveDistance(pathGridPositionList);

        SetTargetPositionList(ConvertPathListToWorldPositionList(pathGridPositionList));


        #region // set only one gridposition as target position
        // Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        // SetTargetPosition(worldPosition);
        #endregion
    }

    void UpdateMoveDistance(List<GridPosition> pathGridPositionList)
    {
        int distance = Pathfinding.Instance.CalculateTotalMoveDistance(pathGridPositionList);
        Debug.Log(distance);
        _unitCurrentMoveDistance -= distance / PathfindingDistanceMultiplier;
    }

    List<Vector3> ConvertPathListToWorldPositionList(List<GridPosition> gridPositionList)
    {
        List<Vector3> worldPositionList = new List<Vector3>();

        foreach (GridPosition gridPosition in gridPositionList)
        {
            worldPositionList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        return worldPositionList;
    }


    public List<GridPosition> GetPredictedMovePathGridPositionList(GridPosition mouseGridPosition)
    {
        List<GridPosition> pathGridPositionList = new List<GridPosition>();

        pathGridPositionList = Pathfinding.Instance.FindPath(_unitGridPosition, mouseGridPosition, out int pathLength);

        return pathGridPositionList;
    }






    // Update unit heicht
    void HeightCheck()
    {
        //GridPosition unitGridPosition = _unit.GetUnitGridPosition();
        Vector3 gridWorldPosition = LevelGrid.Instance.GetWorldPositionWithHeight(_unitGridPosition);

        if (transform.position.y != gridWorldPosition.y) transform.position = gridWorldPosition;
    }


    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -_unitCurrentMoveDistance; x <= _unitCurrentMoveDistance; x++)
        {
            for (int z = -_unitCurrentMoveDistance; z <= _unitCurrentMoveDistance; z++)
            {
                // Unit will not move over than max move distance
                int moveDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (moveDistance > _unitCurrentMoveDistance) continue;

                // Get ideal moveable gridposiiton
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition ValidGridposition = offsetGridPosition + _unitGridPosition;

                // check if gridPosition is not unit gridPosition and have unit on it
                if (ValidGridposition != _unitGridPosition && LevelGrid.Instance.HasUnitOnGridPosition(ValidGridposition)) continue;

                // Check if the girdPosition is inside the entire gridSystem
                if (!LevelGrid.Instance.IsValidGridPosition(ValidGridposition)) continue;

                // Check if gridposition is walkable
                if (!Pathfinding.Instance.IsWalkableGridPosition(ValidGridposition)) continue;

                // Check if gridposition is reachable
                if (!Pathfinding.Instance.HasPathToGridPosition(_unitGridPosition, ValidGridposition)) continue;

                // Check if the path is too long
                int pathfindingDistanceMultiplier = 10;
                List<GridPosition> tempPathList = Pathfinding.Instance.FindPath(_unitGridPosition, ValidGridposition, out int path);
                if (Pathfinding.Instance.CalculateTotalMoveDistance(tempPathList) > _unitCurrentMoveDistance * pathfindingDistanceMultiplier) continue;

                // Check if is valid gridposition based on terrain moveCost check
                if (!Pathfinding.Instance.GetValidMoveGridPoisitionList(_unitGridPosition, _unitCurrentMoveDistance).Contains(ValidGridposition)) continue;

                validGridPositionList.Add(ValidGridposition);
            }
        }
        return validGridPositionList;
    }

    bool IsUnitGridPosition(GridPosition gridPosition)
    {
        return gridPosition == _unitGridPosition;
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
