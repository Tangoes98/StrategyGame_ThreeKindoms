using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuMgr : MonoBehaviour
{
    public GameObject soundObj;
    void Awake()
    {
        //������Ϸ���Ƚ���˵��������򿪲˵�panel
        if (soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "SoundEffect";
        }
        UIManager.GetInstance().ShowPanel<UI_StartManager>("StartPanel", E_UI_Layer.Mid);
        PlayMenuBGM();


        Debug.Log(Application.persistentDataPath);





    }

    public void PlayMenuBGM()
    {
        //�����������BGM
        MusicMgr.GetInstance().PlayBkMusic("Circles in the Sand");
        MusicMgr.GetInstance().ChangeBKValue(0.3f);
    }

}
