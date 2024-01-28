using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyProjectile : MonoBehaviour
{
    public int damage = 10;

    private Rigidbody _rigidbody;
    public CircusDamagable owner;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"OnCollisionEnter({other.gameObject.name})");
        var damagable = other.gameObject.GetComponentInParent<CircusDamagable>();
        if (damagable != null && damagable != owner)
        {
            damagable.DealDamage(damage);
            Destroy(gameObject);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        Debug.Log($"OnCollisionExit({other.gameObject.name})");
    }

    private void Update()
    {
        if (_rigidbody.IsSleeping())
        {
            Destroy(gameObject);
        }
    }
}
