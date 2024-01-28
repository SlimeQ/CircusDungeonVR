using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusDamagable : MonoBehaviour
{
    public int health = 100;

    private ICircusKillable killable;
    private void Awake()
    {
        killable = GetComponent<ICircusKillable>();
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            killable.Kill();
        }
    }
}
