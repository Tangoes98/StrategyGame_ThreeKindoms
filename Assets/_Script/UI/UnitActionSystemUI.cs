using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform _actionButtonPrefab;
    [SerializeField] Transform _actionButtonContainerTransform;




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

        UnitBaseAction[] unitActionArray = selectedUnit.GetUnitBaseActionArray();

        foreach (UnitBaseAction unitAction in unitActionArray)
        {
            Transform actionButton = Instantiate(_actionButtonPrefab, _actionButtonContainerTransform);
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
    }

    void UnitSelection_OnUnitSelecedChanged() => CreateUnitActionButtons();
    void UnitSelection_OnSelectEmpty() => DestoryActionButton();

}
