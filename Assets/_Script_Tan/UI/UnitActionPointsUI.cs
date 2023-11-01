using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitActionPointsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _actionPoitnsText;
    [SerializeField] TextMeshProUGUI _constructionPointsText;

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
        _constructionPointsText.text = "CONSTRUCTION POINT : " + selectedUnit.GetUnitConstructionPoints();
    }

    void ShowText()
    {
        _actionPoitnsText.enabled = true;
        _constructionPointsText.enabled = true;
    }
    void HideText()
    {
        _actionPoitnsText.enabled = false;
        _constructionPointsText.enabled = false;
    }

}
