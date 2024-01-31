using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class T_UnitSelection : MonoBehaviour
{
    public static T_UnitSelection Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }

    [SerializeField] LayerMask _unitLayerMask;

    [SerializeField] T_Unit _selectedUnit;
    [SerializeField] bool _canSelectUnit;

    #region ========== PUBLIC PROPERTIES ==========

    public event Action E_UnitSelected;
    public event Action E_UnitDeselected;

    public T_Unit G_GetSelectedUnit() => _selectedUnit;

    public void G_SetCanSelectUnit(bool value) => _canSelectUnit = value;


    #endregion =======================================

    void Start()
    {
        _canSelectUnit = true;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (!_canSelectUnit) return;

        SelectUnit();
        //DeselectUnit();
    }



    // =================================================

    // Casting ray to check unit
    Transform RaycastingUnit()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask);

        return raycastHit.transform;
    }

    // Mouse click to select/assign unit
    void SelectUnit()
    {
        if (!RaycastingUnit()) return;

        if (T_MouseController.Is_LMB_Down())
        {
            _selectedUnit = RaycastingUnit().GetComponent<T_Unit>();

            E_UnitSelected?.Invoke();
        }



    }

    void DeselectUnit()
    {
        if (Input.GetMouseButtonDown(0) && !RaycastingUnit())
        {
            _selectedUnit = null;
            E_UnitDeselected?.Invoke();
        }
    }





}
