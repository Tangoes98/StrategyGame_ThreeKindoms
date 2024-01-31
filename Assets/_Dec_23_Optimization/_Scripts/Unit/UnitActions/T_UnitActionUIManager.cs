using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class T_UnitActionUIManager : MonoBehaviour
{
    public static T_UnitActionUIManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }


    [SerializeField] Transform _unitActionButtonPrefab;
    T_UnitActionManager _unitActionManager;
    [SerializeField] List<Button> _actionButtons;

    Dictionary<string, T_UnitActionBase> _actionDictionary;

    private event Action<Button> E_ButtonClicked;

    #region ============ Public Properties =========

    public event Action<T_UnitActionBase> E_ActionSelected;
    // public List<Button> G_GetButtons() => _actionButtons;
    // public Dictionary<string, T_UnitActionBase> G_GetActionDictionary() => _actionDictionary;


    //public void G_SetIsButtonInteractable(bool condition) => IsButtonInteractable(condition);
    public void G_SetIsButtonInteractable(string actionName, bool condition) => IsActionButtonInteractable(actionName, condition);
    public void G_SetIsAllButtonInteractable(bool condition) => IsAllButtonInteractable(condition);



    #endregion ====================================




    void Start()
    {
        _unitActionManager = T_UnitActionManager.Instance;
        _actionButtons = new();
        _actionDictionary = new();

        ClearAllActionButtons();

        T_EventCenter.Instance.EventCenter_UnitDeselected += DeselectCurrentUnitEvent;

        _unitActionManager.E_CurrentUnitSelected += SetUnitActionButton;
        E_ButtonClicked += OnButtonClicked;

    }

    void Update()
    {
        if (T_MouseController.Is_LMB_Down()) ActionButtonFunction();

    }


    #region ============ Action Buttons Functions =========


    void ClearAllActionButtons()
    {
        foreach (Transform item in this.transform)
        {
            Destroy(item.gameObject);
        }
        _actionButtons.Clear();
        _actionDictionary.Clear();
    }

    void SetUnitActionButton(List<T_UnitActionBase> actions)
    {
        ClearAllActionButtons();

        foreach (T_UnitActionBase item in actions)
        {
            Transform actionButton = Instantiate(_unitActionButtonPrefab, this.gameObject.transform);
            string actionName = item.G_GetActionName();

            actionButton.GetComponentInChildren<TextMeshProUGUI>().text = actionName;
            actionButton.gameObject.name = actionName;
            string actionButtonName = actionName;

            _actionButtons.Add(actionButton.GetComponentInParent<Button>());
            _actionDictionary.Add(actionButtonName, item);
        }
    }

    void DeselectCurrentUnitEvent() => ClearAllActionButtons();

    void IsAllButtonInteractable(bool condition)
    {
        foreach (var button in _actionButtons)
        {
            button.interactable = condition;
        }
    }

    void IsActionButtonInteractable(string actionName, bool condition)
    {
        foreach (var button in _actionButtons)
        {
            if (button.name != actionName) continue;
            button.interactable = condition;
        }
    }

    void ResetActionButtonState()
    {
        foreach (var action in _actionDictionary)
        {
            action.Value.G_SetActionState(T_UnitActionBase.Action_State.Action_Selection);
        }
    }


    #endregion ================================================

    #region ============ Action Button Events ============

    void ActionButtonFunction()
    {
        foreach (Button button in _actionButtons)
        {
            button.onClick.AddListener
            (() => E_ButtonClicked?.Invoke(button));
        }
    }

    void OnButtonClicked(Button button)
    {
        Debug.Log(button.name);
        Debug.Log(_actionDictionary[button.name].G_GetActionName());

        var action = _actionDictionary[button.name];
        _unitActionManager.G_SetCurrentSeletedAction(action);
        ResetActionButtonState();
        E_ActionSelected?.Invoke(action);
    }

    bool IsMouseClicked() => Input.GetMouseButtonDown(0);




    #endregion ================================================


}
