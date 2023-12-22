using System.Collections;
using System.Collections.Generic;
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


    #region Public Properties






    #endregion






    void Start()
    {
        _levelGridManagerInstance = T_LevelGridManager.Instance;
        FirstStartSetUp();
        SetStartTargetPosition();

    }

    void Update()
    {
        UpdateUnitGridPosition();
        UpdateGridPositionData();

    }

    #region UpdateUnitGirdPosition and GridData

    void FirstStartSetUp()
    {
        UpdateUnitGridPosition();
        _oldGridPosition = _currentGridPosition;
        _currentGridData = CurrentGridData(_currentGridPosition);
        _oldGridData = _currentGridData;
        _currentGridData.AddUnit(this);
        _startPosition = _levelGridManagerInstance.GridToWorldPosition(_currentGridPosition);
        this.transform.position = _startPosition;
    }

    void UpdateUnitGridPosition()
    {
        _currentGridPosition = _levelGridManagerInstance.WorldToGridPosition(this.transform.position);
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

    T_GridData CurrentGridData(T_GirdPosition gp) => _levelGridManagerInstance.GetGridPosData(gp);

    #endregion



    #region UnitMovement

    void SetStartTargetPosition()
    {
        T_UnitMovementAction movementAction = this.GetComponentInChildren<T_UnitMovementAction>();
        movementAction.SetStartTargetPosition(_startPosition);
    }
    #endregion










}
