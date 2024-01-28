using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusDamagable : MonoBehaviour
{
    public Audio _audio;
    public Audio.SFX[] hitSFX = new Audio.SFX[0x01];
    public Audio.SFX[] deathSFX = new Audio.SFX[0x01];
    public int health = 100;

    private ICircusKillable killable;
    private void Awake()
    {
        killable = GetComponent<ICircusKillable>();
    }

    public void DealDamage(int damage)
    {
        //!@ Add onDeath/onHit cawbacks
        byte max = 0x00;
        byte index = 0x00;
        Audio.SFX sfx = Audio.SFX.SFX_NULL;

        health -= damage;
        if (health <= 0)
        {
            max = (byte)(deathSFX.GetLength(0));
            index = (byte)(UnityEngine.Random.Range(0x00, max));
            sfx = deathSFX[index];            
            killable.Kill();
        }
        else
        {
            max = (byte)(hitSFX.GetLength(0));
            index = (byte)(UnityEngine.Random.Range(0x00, max));
            sfx = hitSFX[index];
        }
        _audio.sfx_play(sfx);
    }
}
