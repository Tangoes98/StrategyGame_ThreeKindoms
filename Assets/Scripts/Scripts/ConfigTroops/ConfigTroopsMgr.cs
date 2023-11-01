using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigTroopsMgr : MonoBehaviour
{
    private void Awake()
    {
        UIManager.GetInstance().ShowPanel<UI_ConfigTroopsPanel>("ConfigTroopsPanel", E_UI_Layer.Mid);
        UIManager.GetInstance().ShowPanel<UI_ConfigTroopsPanelMain>("ConfigTroopsPanelMain", E_UI_Layer.Bot);
        UIManager.GetInstance().ShowPanel<UI_GeneralOwnedTroops>("TroopSelectPanel", E_UI_Layer.Bot);

        UIManager.GetInstance().ShowPanel<UI_PassiveSkillChute>("PassiveSkillsScrollView", E_UI_Layer.Bot);
        UIManager.GetInstance().ShowPanel<UI_PassiveSkillPool>("PassiveSkillsPool", E_UI_Layer.Bot);

        UIManager.GetInstance().ShowPanel<UI_ActiveSkillChute>("ActiveSkillScrollView", E_UI_Layer.Bot);
        UIManager.GetInstance().ShowPanel<UI_ActiveSkillPool>("ActiveSkillPool", E_UI_Layer.Bot);
    }

}
