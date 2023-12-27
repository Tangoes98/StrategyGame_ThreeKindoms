using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class T_UnitActionBase : MonoBehaviour
{
    public enum Action_State
    {
        Action_Points_Validation,
        Action_Selection,
        Action_Preview,
        Action_Busy,
        Action_Completion
    }

    [SerializeField] protected Action_State _actionState;
    [SerializeField] protected string _actionName;

    #region ========== PUBLIC PROPERTIES =================

    public string G_ActionName() => _actionName;



    #endregion ===================================================






    protected virtual void Start()
    {
        _actionState = Action_State.Action_Completion;

    }


}
