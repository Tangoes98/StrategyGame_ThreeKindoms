using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] T_Unit _unit;
    [SerializeField] SpriteRenderer _unitSelectedVisual;

    void Start()
    {
        _unit = this.GetComponentInParent<T_Unit>();
    }

    void Update()
    {
        _unitSelectedVisual.enabled = isSelected();
    }


    bool isSelected()
    {
        if (T_UnitSelection.Instance.GetSelectedUnit() == _unit) return true;
        else return false;
    }




}
