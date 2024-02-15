using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DebugConsole;

public class T_HealthSystem : MonoBehaviour
{
    T_Unit _unit;
    [Header("Required Components")]
    [SerializeField] Image _healthImage;

    [SerializeField] int _health;
    int _maxHealth;



    public event Action E_OnDamaged;
    // public event Action E_OnDead;

    void Start()
    {
        _unit = GetComponentInParent<T_Unit>();
        _maxHealth = _health;

    }

    //DEBUG
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnDamage(5);
            T_GameConsole.Instance.G_ConsoleLog("Damage 5");
        };
    }




    public void OnDamage(int damageValue)
    {
        _health -= damageValue;
        _healthImage.fillAmount = GetHealthValueNormalized();
        //E_OnDamaged?.Invoke();
        if (_health <= 0) _health = 0;
        if (_health == 0) Die();
    }

    void Die()
    {
        Debug.Log("Unit Died");
        _unit.gameObject.SetActive(false);
    }

    public float GetHealthValueNormalized() => (float)_health / _maxHealth;
}
