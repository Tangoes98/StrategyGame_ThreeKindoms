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
        //���ݵ�ǰ�½ھ�����ȡ���ֵ�ͼ�͹ؿ�����
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

    //�������õĲ���ִ��
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

    //����ʾ��ϸ��ͼ
    public void showChapterMap()
    {

        if (t1 > 1.1f && currentChpater <= 2)
        {
            mapChapter1and2.SetActive(true);
            spriteRenderer = mapChapter1and2.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null) // ȷ�� spriteRenderer ����ȷ��ȡ
            {
                if (spriteRenderer.color.a <= 1.0f) // ע��͸���ȵķ�Χ�� [0, 1]
                {
                    // ��ȡԭʼ��ɫ
                    Color originalColor = spriteRenderer.color;

                    // ������͸����
                    float newAlphaValue = originalColor.a + Time.deltaTime * 8;

                    // ����͸���ȷ�Χ�� [0, 1]
                    newAlphaValue = Mathf.Clamp01(newAlphaValue);

                    // �����µ���ɫ��ֻ����͸����
                    Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, newAlphaValue);

                    // ���µ���ɫ��ֵ�� SpriteRenderer �� color ����
                    spriteRenderer.color = newColor;
                }
            }
        }
    }

    /// <summary>
    /// ���ݵ�ǰ�½���ʾ��Ӧ��panel
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
    /// ���õ�һ�µ���ϸ��ͼ�Ĳ���
    /// </summary>
    public void MapSet_Chapter1()
    {
        targetMapPosition = new Vector3(-16, -9, 10);
        scaleFactor = 3.0f;
        moveSpeed = 1.0f;
        
    }
}
