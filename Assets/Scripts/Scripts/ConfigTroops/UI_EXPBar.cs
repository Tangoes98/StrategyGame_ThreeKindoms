using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 经验值的经验条
/// </summary>
public class UI_EXPBar : MonoBehaviour
{
    private int currentEXP;
    private int MaxExp;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener<General>("GeneralExpChange", (general) =>
        {
            currentEXP = general.CurrentEXP;
            MaxExp = general.MaxEXP;
            float experienceRatio = (float)currentEXP / MaxExp;
            slider.value = experienceRatio;
        });
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener<General>("GeneralExpChange", (general) => { });
    }
}
