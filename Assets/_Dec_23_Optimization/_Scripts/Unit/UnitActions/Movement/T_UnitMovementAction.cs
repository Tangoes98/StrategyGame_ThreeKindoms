using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitMovementAction : T_UnitActionBase
{
    [Header("LOCAL_VARIABLES")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _stopDistance;
    [SerializeField] T_GridPosition _unitGirdPosition;
    [SerializeField] int _maxMoveDistance;
    // [SerializeField] int _totalMoveableDistance;


    [Header("DEBUG_VIEW")]
    [SerializeField] int _currentPositionListIndex;
    [SerializeField] List<T_GridPosition> _gridPath;
    [SerializeField] bool _isUnitMoved;
    [SerializeField] List<T_GridPosition> _moveableGPList;







    #region ========== Publice Properties ==========
    public bool G_IsUnitMoved() => _isUnitMoved;

    #endregion ========================================


    protected override void Start()
    {
        base.Start();

        _isUnitMoved = false;

        _unitGirdPosition = T_LevelGridManager.Instance.G_WorldToGridPosition(_unit.transform.position);

    }


    protected override void Update()
    {
        base.Update();
        if (!P_isActive) return;

        switch (P_actionState)
        {
            case Action_State.Action_Selection:
                T_DrawPathline.Instance.G_ClearPathline();



                break;
            case Action_State.Action_Preview:

                CancelSelectedActionCheck();
                DrawPreviewPathline();
                if (!CheckActionInput(out T_GridPosition targetGridPosition)) return;
                if (!CheckSelectedMovePosition(targetGridPosition)) return;
                P_actionState = Action_State.Action_Busy;

                break;

            case Action_State.Action_Busy:
                SetAllActionButtonState(false);
                TakeAction();

                break;

            case Action_State.Action_Completed:
                SetAllActionButtonState(true);
                SetSingleActionButtonState(false);
                T_LevelGridManager.Instance.G_ClearAllGridValidationVisuals();
                Debug.Log("MOVEACTION_COMPLETION");
                P_isActive = false;

                break;

        }
    }

    #region ============ Parent Abstract Function Implementation =========

    protected override void TakeAction()
    {
        Debug.Log("ACTION START: " + P_actionName);
        MoveByGridPositionList(_gridPath);

    }
    protected override void PreviewActionValidPosition() => MovementRangePreview();

    #endregion ================================================

    void MovementRangePreview()
    {
        _moveableGPList = T_Pathfinding.Instance.G_ValidMoveableGPList(PotentialMovePositionList(), _unitGirdPosition, _maxMoveDistance);

        T_LevelGridManager.Instance.G_ShowGridValidationVisuals("MOVE_GRID", _moveableGPList);
    }

    void DrawPreviewPathline()
    {
        T_GridPosition gp = T_MouseController.Instance.G_GetMouseGridPosition();
        if (!CheckSelectedMovePosition(gp)) return;
        T_DrawPathline.Instance.G_DrawPathline(_gridPath);
    }


    bool CheckSelectedMovePosition(T_GridPosition selectedGridPosition)
    {
        if (!_moveableGPList.Contains(selectedGridPosition))
        {
            Debug.Log("NOT VALID POSITION");
            _gridPath = null;
            return false;
        }
        _currentPositionListIndex = 0;
        var startGridPosition = T_LevelGridManager.Instance.G_WorldToGridPosition(this.transform.position);
        _gridPath = T_Pathfinding.Instance.G_FindPath(startGridPosition, selectedGridPosition, _moveableGPList);
        return true;
    }



    List<T_GridPosition> PotentialMovePositionList()
    {
        List<T_GridPosition> gridPosList = new();
        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {

                int tempDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (tempDistance > _maxMoveDistance) continue;


                T_GridPosition offsetGridPosition = new T_GridPosition(x, z);
                T_GridPosition ValidGridposition = offsetGridPosition + _unitGirdPosition;


                if (!T_LevelGridManager.Instance.G_IsValidSystemGrid(ValidGridposition)) continue;

                gridPosList.Add(ValidGridposition);
            }
        }
        return gridPosList;
    }

    void MoveByGridPositionList(List<T_GridPosition> gpList)
    {
        List<Vector3> positionList = T_LevelGridManager.Instance.G_ConvertListGridToWorldPosition(gpList);

        // Unit class will update the vertical position information
        Vector3 unitHorizontalPosition = new Vector3(_unit.transform.position.x, 0, _unit.transform.position.z);

        if (Vector3.Distance(positionList[_currentPositionListIndex], unitHorizontalPosition) > _stopDistance)
        {
            //set the direction where unit move to
            Vector3 targetDirection = (positionList[_currentPositionListIndex] - unitHorizontalPosition).normalized;

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
                _isUnitMoved = true;
                P_actionState = Action_State.Action_Completed;
            }
        }
    }



    #region ============== DEBUG_ONLY ==============

    // void MouseClickToMove()
    // {
    //     if (Input.GetMouseButtonDown(1))
    //     {
    //         //test_wdList.Clear();
    //         _currentPositionListIndex = 0;


    //         P_isActive = true;

    //         _targetPosition = T_MouseController.Instance.G_GetMouseWorldPosition();
    //         var targetGridPosition = T_LevelGridManager.Instance.G_WorldToGridPosition(_targetPosition);

    //         if (!ValidMovePositionList().Contains(targetGridPosition))
    //         {
    //             Debug.Log("NOT VALID POSITION");
    //             P_isActive = false;
    //             return;
    //         }

    //         var startGridPosition = T_LevelGridManager.Instance.G_WorldToGridPosition(this.transform.position);

    //         List<T_GirdPosition> gpList = T_Pathfingding.Instance.G_FindPath(startGridPosition, targetGridPosition, ValidMovePositionList());

    //         test_wdList = gpList;


    //     }
    // }

    // void SelectValidMovePosition()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         _targetPosition = T_MouseController.Instance.G_GetMouseWorldPosition();
    //         var targetGridPosition = T_LevelGridManager.Instance.G_WorldToGridPosition(_targetPosition);
    //         if (!ValidMovePositionList().Contains(targetGridPosition))
    //         {
    //             Debug.Log("NOT VALID POSITION");
    //             P_isActive = false;
    //             return;
    //         }

    //         P_isActive = true;
    //         _currentPositionListIndex = 0;
    //         var startGridPosition = T_LevelGridManager.Instance.G_WorldToGridPosition(this.transform.position);
    //         List<T_GirdPosition> gpList = T_Pathfingding.Instance.G_FindPath(startGridPosition, targetGridPosition, ValidMovePositionList());
    //         test_wdList = gpList;
    //     }
    // }




    #endregion =================================







}
