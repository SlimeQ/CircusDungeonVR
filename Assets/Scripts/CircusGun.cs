using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusGun : MonoBehaviour
{
    public Transform muzzleTransform;

    protected virtual void DoFire() { }

    public void StartFire()
    {
        DoFire();
    }
    
    public void StopFire()
    {
        
    }
}
