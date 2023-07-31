using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitActionButtonUI : MonoBehaviour
{



    [SerializeField] TextMeshProUGUI _textMeshPro;
    [SerializeField] Button _button;

    public void SetBaseAction(UnitBaseAction baseAction)
    {
        _textMeshPro.text = baseAction.GetActionName();

        _button.onClick.AddListener
        (
            () =>
            {
                if (UnitSelection.Instance.IsBusy()) return;

                UnitSelection.Instance.SetSelectedAction(baseAction);

                // if (baseAction is UnitMovementAction)
                // {
                //     UnitMoveGridPositionDebug((UnitMovementAction)baseAction);
                // }



                if (baseAction is UnitBaseConstructAction)
                {
                    UnitBaseConstructAction action = (UnitBaseConstructAction)baseAction;
                    if (action.IsOverUseCount()) Debug.Log("Cant use this Action!");


                    if (action is ConstructFloatingBridge)
                    {
                        ConstructFloatingBridge floatingBridgeAction = (ConstructFloatingBridge)action;
                        int actionCountDown = 2;
                        floatingBridgeAction.SetBuildActionCountDown(actionCountDown);
                        floatingBridgeAction.SetIsSecondAction(false);
                    }

                }
            }
        );
    }

    // void UnitMoveGridPositionDebug(UnitMovementAction action)
    // {
    //     var positionList = action.GetValidGridPositionList();
    //     foreach (GridPosition position in positionList)
    //     {
    //         Debug.Log(position);
    //     }
    // }










}
