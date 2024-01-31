using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class T_UnitActionBase : MonoBehaviour
{
    public enum Action_State
    {
        Action_Points_Validation,
        Action_Selection,
        Action_Preview,
        Action_Busy,
        Action_Completed
    }

    // P_ = parent_ // variable from parent class
    [SerializeField] protected Action_State P_actionState;
    [SerializeField] protected string P_actionName;
    // [SerializeField] protected bool P_isCurrentSelectedAction;
    [SerializeField] protected bool P_isActive;
    protected T_Unit _unit;



    #region ========== PUBLIC PROPERTIES =================
    public string G_GetActionName() => P_actionName;
    // public void G_SetIsCurrentSelectedAction(bool condition) => P_isCurrentSelectedAction = condition;

    public void G_SetIsActive(bool condition) => P_isActive = condition;
    public void G_SetActionState(Action_State actionState) => P_actionState = actionState;

    public void G_PreviewActionValidPosition() => PreviewActionValidPosition();



    #endregion ===================================================


    #region ============ Parent Functions ============
    protected virtual void Start()
    {
        P_actionState = Action_State.Action_Selection;
        // P_isCurrentSelectedAction = false;
        P_isActive = false;
        _unit = GetComponentInParent<T_Unit>();
    }
    protected virtual void Update()
    {

    }


    protected abstract void TakeAction();
    protected abstract void PreviewActionValidPosition();


    // Press RMB to to cancel action selection
    protected void CancelSelectedActionCheck()
    {
        if (T_MouseController.Is_RMB_Down())
        {
            T_UnitSelection.Instance.G_SetCanSelectUnit(true);
            T_LevelGridManager.Instance.G_ClearAllGridValidationVisuals();
            T_DrawPathline.Instance.G_ClearPathline();
            P_actionState = Action_State.Action_Selection;
        }
    }

    protected bool CheckActionInput(out T_GirdPosition targetGridPosition)
    {
        if (T_MouseController.Is_LMB_Down())
        {
            var targetWorldPostiion = T_MouseController.Instance.G_GetMouseWorldPosition();
            targetGridPosition = T_LevelGridManager.Instance.G_WorldToGridPosition(targetWorldPostiion);
            T_DrawPathline.Instance.G_ClearPathline();
            return true;
        }
        else
        {
            targetGridPosition = new(0, 0);
            return false;
        }
    }

    protected void SetSingleActionButtonState(bool condition)
    {
        T_UnitActionUIManager.Instance.G_SetIsButtonInteractable(P_actionName, condition);
    }

    protected void SetAllActionButtonState(bool condition)
    {
        T_UnitActionUIManager.Instance.G_SetIsAllButtonInteractable(condition);
    }






    #endregion ====================================





}
