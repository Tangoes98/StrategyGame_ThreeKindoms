using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using TMPro;
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

        if (TKeyPressed())
        {
            // var gp = new T_GirdPosition(5, 4);
            // var terrain = T_LevelGridManager.Instance.G_GetGridPosData(gp).GetSurfaceTerrain();
            // terrain.G_GetChildTerrainType<T_Forest>().G_SetIsFlaming(true);

            // // T_Forest i = (T_Forest)terrain;
            // // i.G_SetIsFlaming(true);

            // Debug.Log(terrain);

            // //Debug.Log();

        }



    }

    void DICE1D6()
    {
        int num = Random.Range(1, 6);
        Debug.Log($"Dice 1d6 , Dice result: {num}");
    }

    bool TKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.T)) return true;
        else return false;
    }


}
