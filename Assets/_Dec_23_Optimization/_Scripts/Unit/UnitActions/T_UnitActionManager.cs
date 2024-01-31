using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitActionManager : MonoBehaviour
{
    public static T_UnitActionManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }




    [SerializeField] T_Unit _currentUnit;

    [Header("DEBUG")]
    [SerializeField] T_UnitActionBase _currentSelecedAction;
    [SerializeField] List<T_UnitActionBase> _currentActions;

    //event Action<T_UnitActionBase> E_ActionSelected;


    #region ========== PUBLIC PROPERTIES =================

    public event Action<List<T_UnitActionBase>> E_CurrentUnitSelected;

    public T_Unit G_GetCurrentUnit() => _currentUnit;
    public List<T_UnitActionBase> G_GetCurrentUnitActions() => _currentActions;
    public void G_SetCurrentSeletedAction(T_UnitActionBase action) => _currentSelecedAction = action;




    #endregion ===================================================



    #region ========= START_UPDATE ==========
    void Start()
    {
        T_EventCenter.Instance.EventCenter_UnitSelected += SelectCurrentUnitEvent;
        T_EventCenter.Instance.EventCenter_UnitDeselected += DeselectCurrentUnitEvent;

        T_EventCenter.Instance.EventCenter_ActionSelected += ActionSelectedEvent;
    }

    void Update()
    {
        //GetCurrentUnit();
        // if (!_currentSelecedAction) return;
        // else E_ActionSelected?.Invoke(_currentSelecedAction);

    }

    #endregion ====================================


    #region ============ Unit Selelction Event ============
    void SelectCurrentUnitEvent()
    {
        _currentUnit = T_UnitSelection.Instance.G_GetSelectedUnit();

        if (!_currentUnit) return;

        _currentActions = _currentUnit.G_GetUnitActions();

        E_CurrentUnitSelected?.Invoke(_currentActions);
    }

    void DeselectCurrentUnitEvent()
    {
        _currentUnit = null;
    }
    #endregion ====================================


    #region ============ Unit Action Selection Event ============

    void ActionSelectedEvent(T_UnitActionBase action)
    {
        // Clear all grid visuals
        T_LevelGridManager.Instance.G_ClearAllGridValidationVisuals();

        // Enable action
        action.G_SetIsActive(true);
        action.G_SetActionState(T_UnitActionBase.Action_State.Action_Preview);
        action.G_PreviewActionValidPosition();

        // Disable mouse selection
        T_UnitSelection.Instance.G_SetCanSelectUnit(false);

    }





    #endregion ====================================






}
