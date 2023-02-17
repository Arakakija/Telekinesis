using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100.0f;

    private float _health;
    

    private void Start()
    {
        _health = _maxHealth;
    }

    public void DealDamage(float amount)
    {
        if (_health == 0) return;

        _health = Mathf.Max(_health - amount, 0);
        
        Debug.Log($"Health: {_health}");
    }
}
