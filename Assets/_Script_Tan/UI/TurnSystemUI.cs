using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] Button _endTurnButton;
    [SerializeField] TextMeshProUGUI _turnNumberText;

    void Start()
    {
        _endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });
    }

    void Update()
    {
        _turnNumberText.text = "TURN : " + TurnSystem.Instance.GetTurnNumber();
    }



}
