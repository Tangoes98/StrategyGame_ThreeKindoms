using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T_UnitActionUIManager : MonoBehaviour
{
    [SerializeField] Transform _unitActionButtonPrefab;
    T_UnitActionManager _unitActionManager;




    void Start()
    {
        _unitActionManager = T_UnitActionManager.Instance;

        ClearAllActionButtons();



        _unitActionManager.E_CurrentUnitSelected += SetUnitActionButton;
        T_EventCenter.Instance.EventCenter_UnitDeselected += DeselectCurrentUnitEvent;


    }

    void Update()
    {
        // if (!_unitActionManager.G_GetCurrentUnit()) return;
        // else SetUnitActionButton(_unitActionManager.G_GetCurrentUnitActions());

    }


    void ClearAllActionButtons()
    {
        foreach (Transform item in this.transform)
        {
            Destroy(item.gameObject);
        }
    }

    void SetUnitActionButton(List<T_UnitActionBase> actions)
    {
        ClearAllActionButtons();
        foreach (T_UnitActionBase item in actions)
        {
            var actionButton = Instantiate(_unitActionButtonPrefab, this.gameObject.transform);
            actionButton.GetComponentInChildren<TextMeshProUGUI>().text = item.G_ActionName();
        }
    }

    void DeselectCurrentUnitEvent() => ClearAllActionButtons();











}
