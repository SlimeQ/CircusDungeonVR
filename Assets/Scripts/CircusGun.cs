using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusGun : MonoBehaviour
{
    public Transform muzzleTransform;
    public float fireRate = 0.7f;

    protected virtual void DoFire() { }

    private float lastFireTime;
    public void StartFire()
    {
        if (Time.time > lastFireTime + fireRate)
        {
            DoFire();
            lastFireTime = Time.time;
        }
    }
    
    public void StopFire()
    {
        
    }
}
