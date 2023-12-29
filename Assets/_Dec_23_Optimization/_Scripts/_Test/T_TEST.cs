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
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     for (int i = 1; i < 7; i++)
        //     {
        //         int num = Random.Range(0, 100);
        //         Debug.Log($"Dice index: {i} , Dice result: {num}");
        //     }
        // }
        // if (Input.GetKeyDown(KeyCode.Y))
        // {
        //     DICE1D6();
        // }



    }

    void DICE1D6()
    {
        int num = Random.Range(1, 6);
        Debug.Log($"Dice 1d6 , Dice result: {num}");
    }


}
