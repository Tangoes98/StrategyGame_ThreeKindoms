using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSuppliesMgr : MonoBehaviour
{
    private void Awake()
    {
        UIManager.GetInstance().ShowPanel<UI_ConfigPublicMS>("ManageSuppliesPanel", E_UI_Layer.Mid);
        UIManager.GetInstance().ShowPanel<UI_BagPanel>("BagPanel", E_UI_Layer.Bot);
        UIManager.GetInstance().ShowPanel<UI_ShopPanel>("ShopPanel", E_UI_Layer.Bot);
    }

}
