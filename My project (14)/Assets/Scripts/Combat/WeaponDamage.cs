using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider _ownCollider;

    private readonly List<Collider> _alreadyCollidedWith = new List<Collider>();

    private void OnEnable()
    {
        _alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == _ownCollider) return;
        
        if (_alreadyCollidedWith.Contains(other)) return;
        _alreadyCollidedWith.Add(other);
        if (other.TryGetComponent<Health>(out var health))
        {
            health.DealDamage(10.0f);
        }
    }
}
