using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class T_Unit : MonoBehaviour
{


    // unit data has to update to grid data(position, ...)

    T_LevelGridManager _levelGridManagerInstance;
    T_GirdPosition _oldGridPosition;
    T_GirdPosition _currentGridPosition;
    T_GridData _oldGridData;
    T_GridData _currentGridData;
    Vector3 _startPosition;


    [SerializeField] List<T_UnitActionBase> _unitActions;


    #region ========== Public Properties ====================

    public List<T_UnitActionBase> G_GetUnitActions() => _unitActions;




    #endregion ==================================================






    void Start()
    {
        _levelGridManagerInstance = T_LevelGridManager.Instance;
        UnitGridPositionStartup();
        UnitValidActionStartupCheck();


    }

    void Update()
    {
        UpdateUnitGridPosition();
        UpdateGridPositionData();

    }

    // When the unit is dead
    void OnDisable()
    {
        _currentGridData.RemoveUnit(this);
    }

    #region ========== UpdateUnitGirdPosition and GridData ==========

    void UnitGridPositionStartup()
    {
        UpdateUnitGridPosition();
        _oldGridPosition = _currentGridPosition;
        _currentGridData = CurrentGridData(_currentGridPosition);
        _oldGridData = _currentGridData;
        _currentGridData.AddUnit(this);
        _startPosition = _levelGridManagerInstance.G_GridToWorldPosition(_currentGridPosition);
        this.transform.position = _startPosition;
    }

    void UpdateUnitGridPosition()
    {
        _currentGridPosition = _levelGridManagerInstance.G_WorldToGridPosition(this.transform.position);
        //this.transform.position = _levelGridManagerInstance.GridToWorldPosition(_currentGridPosition);
    }

    void UpdateGridPositionData()
    {
        if (_currentGridPosition == _oldGridPosition) return;
        _oldGridData.RemoveUnit(this);
        _currentGridData = CurrentGridData(_currentGridPosition);
        _currentGridData.AddUnit(this);
        _oldGridData = _currentGridData;
        _oldGridPosition = _currentGridPosition;
    }

    T_GridData CurrentGridData(T_GirdPosition gp) => _levelGridManagerInstance.G_GetGridPosData(gp);

    #endregion ==================================================


    #region ================ Unit Action Validation ================

    void UnitValidActionStartupCheck()
    {
        _unitActions = new();
        T_UnitActionBase[] unitAllActions = GetComponentsInChildren<T_UnitActionBase>();
        foreach (T_UnitActionBase action in unitAllActions)
        {
            if (action.enabled) _unitActions.Add(action);
        }
    }


    #endregion ================================================




}
