using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitActionPointsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _actionPoitnsText;

    void Start()
    {
        UnitSelection.Instance.OnUnitSelecedChanged += UnitSelection_OnUnitSelecedChanged;
        UnitSelection.Instance.OnActionPointsSpend += UnitSelection_OnactionPointsSpend;

        HideText();
    }

    void Update()
    {
        UpdateActionPointsText();

        
    }

    void UnitSelection_OnUnitSelecedChanged() => UpdateActionPointsText();
    void UnitSelection_OnactionPointsSpend() => UpdateActionPointsText();

    void UpdateActionPointsText()
    {
        bool unitIsSelected = UnitSelection.Instance.UnitIsSelected();

        if (!unitIsSelected)
        {
            HideText();
            return;
        }

        Unit selectedUnit = UnitSelection.Instance.GetSelectedUnit().GetComponent<Unit>();

        ShowText();
        _actionPoitnsText.text = "ACTION POINT : " + selectedUnit.GetActionPoints();
    }

    void ShowText() => _actionPoitnsText.enabled = true;
    void HideText() => _actionPoitnsText.enabled = false;

}
