using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] MeshRenderer _selectedVisual;
    [SerializeField] Transform _unit;

    void Awake()
    {
        _selectedVisual = gameObject.GetComponent<MeshRenderer>();
        _selectedVisual.enabled = false;

    }

    void Start()
    {
        UnitSelection.Instance.OnUnitSelecedChanged += UnitSelection_OnUnitSelecedChanged;
        UnitSelection.Instance.OnSelectEmpty += UnitSelection_OnSelectEmpty;
    }

    void UnitSelection_OnUnitSelecedChanged()
    {
        if (UnitSelection.Instance.GetSelectedUnit() == _unit)
        {
            OnSelected(true);
        }
        else OnSelected(false);
    }

    void UnitSelection_OnSelectEmpty()
    {
        OnSelected(false);
    }

    public void OnSelected(bool isSelected)
    {
        if (isSelected) _selectedVisual.enabled = true;
        if (!isSelected) _selectedVisual.enabled = false;
    }



}
