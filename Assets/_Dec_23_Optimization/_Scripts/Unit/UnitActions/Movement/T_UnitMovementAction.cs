using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitMovementAction : T_UnitActionBase
{
    [Header("LOCAL_VARIABLES")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _stopDistance;
    [SerializeField] T_GirdPosition _unitGirdPosition;
    [SerializeField] int _maxMoveDistance;


    [Header("DEBUG_VIEW")]
    [SerializeField] int _currentPositionListIndex;
    [SerializeField] List<T_GirdPosition> _gridPath;
    [SerializeField] bool _isUnitMoved;







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




                break;
            case Action_State.Action_Preview:

                CancelSelectedActionCheck();
                DrawPreviewPathline();
                if (!CheckActionInput(out T_GirdPosition targetGridPosition)) return;
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
    protected override void PreviewActionValidPosition()
    {
        MovementRangePreview();
    }

    #endregion ================================================

    void MovementRangePreview()
    {
        T_LevelGridManager.Instance.G_ShowGridValidationVisuals("MOVE_GRID", ValidMovePositionList());
    }

    void DrawPreviewPathline()
    {
        T_GirdPosition gp = T_MouseController.Instance.G_GetMouseGridPosition();
        if (!CheckSelectedMovePosition(gp)) return;
        T_DrawPathline.Instance.G_DrawPathline(_gridPath);
    }


    bool CheckSelectedMovePosition(T_GirdPosition selectedGridPosition)
    {
        if (!ValidMovePositionList().Contains(selectedGridPosition))
        {
            Debug.Log("NOT VALID POSITION");
            _gridPath = null;
            return false;
        }
        _currentPositionListIndex = 0;
        var startGridPosition = T_LevelGridManager.Instance.G_WorldToGridPosition(this.transform.position);
        _gridPath = T_Pathfinding.Instance.G_FindPath(startGridPosition, selectedGridPosition, ValidMovePositionList());
        return true;
    }



    List<T_GirdPosition> ValidMovePositionList()
    {
        List<T_GirdPosition> gridPosList = new();
        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
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

    void MoveByGridPositionList(List<T_GirdPosition> gpList)
    {
        List<Vector3> positionList = T_LevelGridManager.Instance.G_ConvertListGridToWorldPosition(gpList);

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
