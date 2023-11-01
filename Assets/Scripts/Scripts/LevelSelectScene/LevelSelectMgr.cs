using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class LevelSelectMgr : MonoBehaviour
{
    public GameObject map;
    public GameObject mapChapter1and2;
    public int currentChpater;

    public float scaleFactor;
    public float moveSpeed;

    private Vector3 targetMapPosition;
    private Vector3 initialMapScale;
    private Vector3 initialMapPosition;
    private float t1 = 0.0f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        UIManager.GetInstance().ShowPanel<UI_LevelSelectPanel>("LevelSelectPanel", E_UI_Layer.Mid);
    }

    private void Start()
    {
        t1 = 0.0f;
        //根据当前章节决定读取哪种地图和关卡设置
        currentChpater = GameDataMgr.GetInstance().PlayerDataInfo.currentChapter;
        {
            initialMapScale = map.transform.localScale;
            initialMapPosition = map.transform.position;
            switch (currentChpater)
            {
                case 1:
                    MapSet_Chapter1();

                    break;
                case 2:
                    MapSet_Chapter1();

                    break;
            }
        }

    }

    private void Update()
    {
        MapMove();
        showChapterMap();
        showButtons();
    }

    //根据设置的参数执行
    public void MapMove()
    {

        t1 += Time.deltaTime * moveSpeed;

        if (t1 <= 2.0f)
        {
            // Interpolate scale
            map.transform.localScale = Vector3.Lerp(initialMapScale, initialMapScale * scaleFactor, t1);

            // Interpolate position
            map.transform.position = Vector3.Lerp(initialMapPosition, targetMapPosition, t1);
        }
    }

    //逐渐显示详细地图
    public void showChapterMap()
    {

        if (t1 > 1.1f && currentChpater <= 2)
        {
            mapChapter1and2.SetActive(true);
            spriteRenderer = mapChapter1and2.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null) // 确保 spriteRenderer 已正确获取
            {
                if (spriteRenderer.color.a <= 1.0f) // 注意透明度的范围是 [0, 1]
                {
                    // 获取原始颜色
                    Color originalColor = spriteRenderer.color;

                    // 逐渐增加透明度
                    float newAlphaValue = originalColor.a + Time.deltaTime * 8;

                    // 限制透明度范围在 [0, 1]
                    newAlphaValue = Mathf.Clamp01(newAlphaValue);

                    // 创建新的颜色，只更改透明度
                    Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, newAlphaValue);

                    // 将新的颜色赋值给 SpriteRenderer 的 color 属性
                    spriteRenderer.color = newColor;
                }
            }
        }
    }

    /// <summary>
    /// 根据当前章节显示对应的panel
    /// </summary>
    public void showButtons()
    {
        if (t1 > 1.1f && currentChpater == 1)
        {
            UIManager.GetInstance().ShowPanel<UI_Chapter1LevelOption>("Chapter1Options");
        }
        else if(t1 > 1.1f && currentChpater == 2)
        {
            UIManager.GetInstance().ShowPanel<UI_Chapter2LevelOption>("Chapter2Options");
        }
    }

    /// <summary>
    /// 设置第一章的详细地图的参数
    /// </summary>
    public void MapSet_Chapter1()
    {
        targetMapPosition = new Vector3(-16, -9, 10);
        scaleFactor = 3.0f;
        moveSpeed = 1.0f;
        
    }
}
