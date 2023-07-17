using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConstructionHealthSystem : MonoBehaviour
{
    [SerializeField] int _health;
    int _maxHealth;

    public event Action OnDamaged;
    public event Action OnDestroyed;

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
        Debug.Log("CONSTRUCTION DESTROYED");
        OnDestroyed?.Invoke();
    }

    public float GetHealthValueNormalized() => (float)_health / _maxHealth;
}
