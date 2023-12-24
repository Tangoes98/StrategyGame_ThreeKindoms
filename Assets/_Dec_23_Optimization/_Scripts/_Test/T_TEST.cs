using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_TEST : MonoBehaviour
{

    void Start()
    {
        //Debug.Log(new T_GirdPosition(2, 5));
    }

    void Update()
    {
        TEST();
    }

    void TEST()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            T_LevelGridManager.Instance.G_GetGridValidationVisual(new T_GirdPosition(1, 1)).G_GetGridValidationVisualDictionary()["MOVE"].enabled = true;

        }
    }


}
