using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    private void Awake()
    {
        UIManager.GetInstance().ShowPanel<UI_ConfigPanel>("ConfigPanel", E_UI_Layer.Bot);
    }
}
