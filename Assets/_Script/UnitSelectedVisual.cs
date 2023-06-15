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
    }

    void UnitSelection_OnUnitSelecedChanged(object sender, EventArgs e)
    {
        if (UnitSelection.Instance.GetSelectedUnit() == _unit)
        {
            OnSelected(true);
        }
        else OnSelected(false);
    }


    public void OnSelected(bool isSelected)
    {
        if (isSelected) _selectedVisual.enabled = true;
        if (!isSelected) _selectedVisual.enabled = false;
    }



}
