using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UnitHealthWorldUI : MonoBehaviour
{
    [SerializeField] Unit _unit;
    [SerializeField] Image _healthBarImage;
    [SerializeField] HealthSystem _unitHealthSystem;

    void Start()
    {
        _unitHealthSystem.OnDamaged += UnitHealthSystem_OnDamaged;
    }

    void UnitHealthSystem_OnDamaged()
    {
        UpdateHealthBarValue();
    }

    void UpdateHealthBarValue()
    {
        _healthBarImage.fillAmount = _unitHealthSystem.GetHealthValueNormalized();
    }




}
