using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] int _health;
    int _maxHealth;

    public event Action OnDamaged;
    public event Action OnDead;

    void Start()
    {
        _maxHealth = _health;

    }

    public void OnDamage(int damageValue)
    {
        _health -= damageValue;
        OnDamaged?.Invoke();
        if (_health <= 0) _health = 0;
        if (_health == 0) Die();
    }

    void Die()
    {
        Debug.Log("Unit Died");
        OnDead?.Invoke();
    }

    public float GetHealthValueNormalized() => (float)_health / _maxHealth;

}
