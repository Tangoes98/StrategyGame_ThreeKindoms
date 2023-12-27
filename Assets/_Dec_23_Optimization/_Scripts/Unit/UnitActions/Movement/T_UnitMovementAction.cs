using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitMovementAction : T_UnitActionBase
{
    [Header("LOCAL_VARIABLES")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _stopDistance;
    [SerializeField] T_Unit _unit;
    [SerializeField] T_GirdPosition _unitGirdPosition;
    [SerializeField] int _maxMoveDistance;


    [Header("DEBUG_VIEW")]
    [SerializeField] Vector3 _targetPosition;
    [SerializeField] int _currentPositionListIndex;
    [SerializeField] bool _isActionCompleted;
    [SerializeField] List<Vector3> test_wdList;







    #region ========== Publice Properties ==========

    public void SetStartTargetPosition(Vector3 position) => _targetPosition = position;

    #endregion ========================================


    protected override void Start()
    {
        base.Start();

        _isActionCompleted = true;
        test_wdList = new();
        _unitGirdPosition = T_LevelGridManager.Instance.G_WorldToGridPosition(_unit.transform.position);

    }


    void Update()
    {
        if (T_UnitSelection.Instance.GetSelectedUnit() != _unit) return;

        MovementRangePreview();

        MouseClickToMove();

        if (_isActionCompleted) return;

        MoveByPositionList(test_wdList);
    }

    #region ============== DEBUG_ONLY ==============

    void MouseClickToMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //test_wdList.Clear();
            _currentPositionListIndex = 0;


            _isActionCompleted = false;

            _targetPosition = T_MouseController.Instance.GetMouseWorldPosition();

            var targetGridPosition = T_LevelGridManager.Instance.G_WorldToGridPosition(_targetPosition);
            var startGridPosition = T_LevelGridManager.Instance.G_WorldToGridPosition(this.transform.position);

            List<T_GirdPosition> gpList = T_Pathfingding.Instance.G_FindPath(startGridPosition, targetGridPosition);

            test_wdList = T_LevelGridManager.Instance.G_ConvertListGridToWorldPosition(gpList);


        }




    }
    #endregion =================================



    void TakeAction(List<Vector3> positionList)
    {
        MoveByPositionList(positionList);
    }

    void MovementRangePreview()
    {
        T_LevelGridManager.Instance.G_ShowGridValidationVisual_Move(GetValidMovePositionList());
    }

    List<T_GirdPosition> GetValidMovePositionList()
    {
        List<T_GirdPosition> gridPosList = new();
        for (int x = -_maxMoveDistance; x < _maxMoveDistance * 2; x++)
        {
            for (int z = -_maxMoveDistance; z < _maxMoveDistance * 2; z++)
            {

                int tempDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (tempDistance > _maxMoveDistance) continue;


                T_GirdPosition offsetGridPosition = new T_GirdPosition(x, z);
                T_GirdPosition ValidGridposition = offsetGridPosition + _unitGirdPosition;


                if (!T_LevelGridManager.Instance.G_IsValidSystemGrid(ValidGridposition)) continue;

                gridPosList.Add(ValidGridposition);

            }
        }

        return gridPosList;
    }

    void MoveByPositionList(List<Vector3> positionList)
    {
        if (Vector3.Distance(positionList[_currentPositionListIndex], _unit.transform.position) > _stopDistance)
        {
            //set the direction where unit move to
            Vector3 targetDirection = (positionList[_currentPositionListIndex] - _unit.transform.position).normalized;

            // unit movement to target direction
            _unit.transform.position += targetDirection * _moveSpeed * Time.deltaTime;

            // unit rotation animation
            _unit.transform.forward = Vector3.Slerp(transform.forward, targetDirection, Time.deltaTime * _rotateSpeed);
        }
        else
        {
            _currentPositionListIndex++;

            if (_currentPositionListIndex == positionList.Count)
            {
                _isActionCompleted = true;
            }
        }
    }









}
