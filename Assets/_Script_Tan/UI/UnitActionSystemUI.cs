using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform _actionButtonPrefab;
    [SerializeField] Transform _actionButtonContainerTransform;

    [SerializeField] Transform _unitConstructActionButtonPrefab;
    [SerializeField] Transform _unitConstructActionButtonContainerTransform;





    void Start()
    {
        UnitSelection.Instance.OnUnitSelecedChanged += UnitSelection_OnUnitSelecedChanged;
        UnitSelection.Instance.OnSelectEmpty += UnitSelection_OnSelectEmpty;

        CreateUnitActionButtons();
    }

    void CreateUnitActionButtons()
    {
        DestoryActionButton();

        if (!UnitSelection.Instance.UnitIsSelected()) return;

        Unit selectedUnit = UnitSelection.Instance.GetSelectedUnit().GetComponent<Unit>();

        List<UnitBaseAction> unitActionList = selectedUnit.GetUnitBaseActionList();

        foreach (UnitBaseAction unitAction in unitActionList)
        {
            Transform actionButton = Instantiate(_actionButtonPrefab, _actionButtonContainerTransform);
            UnitActionButtonUI actionButtonUI = actionButton.GetComponent<UnitActionButtonUI>();
            actionButtonUI.SetBaseAction(unitAction);
        }

        List<UnitBaseConstructAction> unitConstructActionList = selectedUnit.GetUnitConstructActionList();
        foreach (UnitBaseConstructAction unitAction in unitConstructActionList)
        {
            Transform actionButton = Instantiate(_unitConstructActionButtonPrefab, _unitConstructActionButtonContainerTransform);
            UnitActionButtonUI actionButtonUI = actionButton.GetComponent<UnitActionButtonUI>();
            actionButtonUI.SetBaseAction(unitAction);
        }

    }
    void DestoryActionButton()
    {
        foreach (Transform action in _actionButtonContainerTransform)
        {
            Destroy(action.gameObject);
        }
        foreach (Transform action in _unitConstructActionButtonContainerTransform)
        {
            Destroy(action.gameObject);
        }
    }

    void UnitSelection_OnUnitSelecedChanged() => CreateUnitActionButtons();
    void UnitSelection_OnSelectEmpty() => DestoryActionButton();

}
