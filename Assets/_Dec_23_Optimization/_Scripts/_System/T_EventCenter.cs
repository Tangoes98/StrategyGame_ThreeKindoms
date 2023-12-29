using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class T_EventCenter : MonoBehaviour
{
    public static T_EventCenter Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }



    void Start()
    {
        T_UnitSelection.Instance.E_UnitSelected += UnitSelectedEvent;
        T_UnitSelection.Instance.E_UnitDeselected += UnitDeselectedEvent;
        T_UnitActionUIManager.Instance.E_ActionSelected += ActionSelectedEvent;





    }

    #region ========== Public Events =========

    public event Action EventCenter_UnitSelected;
    public event Action EventCenter_UnitDeselected;
    public event Action<T_UnitActionBase> EventCenter_ActionSelected;




    #endregion ==============================

    void UnitSelectedEvent() => EventCenter_UnitSelected?.Invoke();

    void UnitDeselectedEvent() => EventCenter_UnitDeselected?.Invoke();

    void ActionSelectedEvent(T_UnitActionBase action) => EventCenter_ActionSelected?.Invoke(action);

}
