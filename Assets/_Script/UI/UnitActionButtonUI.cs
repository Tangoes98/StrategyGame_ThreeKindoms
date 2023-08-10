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



                if (baseAction is UnitBaseConstructAction action)
                {
                    if (action.IsOverUseCount()) Debug.Log("Cant use this Action!");


                    if (action is ConstructFloatingBridge floatingBridgeAction)
                    {
                        int actionCountDown = 2;
                        floatingBridgeAction.SetBuildActionCountDown(actionCountDown);
                        floatingBridgeAction.SetIsSecondAction(false);
                    }

                }
            }
        );
    }









}
