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
    [SerializeField] List<T_UnitActionBase> _currentActions;


    #region ========== PUBLIC PROPERTIES =================

    public event Action<List<T_UnitActionBase>> E_CurrentUnitSelected;

    public T_Unit G_GetCurrentUnit() => _currentUnit;
    public List<T_UnitActionBase> G_GetCurrentUnitActions() => _currentActions;




    #endregion ===================================================



    #region ========= START_UPDATE ==========

    void Start()
    {
        T_EventCenter.Instance.EventCenter_UnitSelected += SelectCurrentUnitEvent;
        T_EventCenter.Instance.EventCenter_UnitDeselected += DeselectCurrentUnitEvent;
    }

    void Update()
    {
        //GetCurrentUnit();
    }

    #endregion ====================================


    void SelectCurrentUnitEvent()
    {
        _currentUnit = T_UnitSelection.Instance.GetSelectedUnit();

        if (!_currentUnit) return;

        _currentActions = _currentUnit.G_GetUnitActions();

        E_CurrentUnitSelected?.Invoke(_currentActions);
    }

    void DeselectCurrentUnitEvent()
    {
        _currentUnit = null;
    }











}
