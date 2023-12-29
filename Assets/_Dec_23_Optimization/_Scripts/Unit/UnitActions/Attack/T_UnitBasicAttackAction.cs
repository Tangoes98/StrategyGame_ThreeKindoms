using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitBasicAttackAction : T_UnitActionBase
{
    [Header("LOCAL_VARIABLES")]
    [SerializeField] T_Unit _unit;
    [SerializeField] int _attackRange;
    //[SerializeField] T_GirdPosition _unitGirdPosition;





    protected override void Start()
    {
        base.Start();
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


                break;

            case Action_State.Action_Busy:
                SetAllActionButtonState(false);
                TakeAction();

                break;

            case Action_State.Action_Completed:
                SetSingleActionButtonState(true);
                T_LevelGridManager.Instance.G_ClearAllGridValidationVisuals();
                Debug.Log("ATTACK_COMPLETE");
                P_isActive = false;

                break;

        }
    }

    #region ============ Parent Abstract Function Implementation =========

    protected override void TakeAction()
    {
        Debug.Log("ACTION START: " + P_actionName);
    }
    protected override void PreviewActionValidPosition()
    {
        AttackRangePreview();

    }

    #endregion ================================================
    void AttackRangePreview()
    {
        T_LevelGridManager.Instance.G_ShowGridValidationVisuals("ATTACK_RANGE", ValidMovePositionList());
    }

    List<T_GirdPosition> ValidMovePositionList()
    {
        List<T_GirdPosition> gridPosList = new();
        T_GirdPosition unitGp = T_LevelGridManager.Instance.G_WorldToGridPosition(_unit.transform.position);
        for (int x = -_attackRange; x < _attackRange * 2; x++)
        {
            for (int z = -_attackRange; z < _attackRange * 2; z++)
            {

                int tempDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (tempDistance > _attackRange) continue;


                T_GirdPosition offsetGridPosition = new T_GirdPosition(x, z);
                T_GirdPosition ValidGridposition = offsetGridPosition + unitGp;


                if (!T_LevelGridManager.Instance.G_IsValidSystemGrid(ValidGridposition)) continue;

                gridPosList.Add(ValidGridposition);
            }
        }
        return gridPosList;
    }








}
