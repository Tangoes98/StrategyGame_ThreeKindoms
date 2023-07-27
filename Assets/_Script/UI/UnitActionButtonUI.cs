using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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


                if (baseAction.IsConstructionAction())
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



}
