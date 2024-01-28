using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusGun : MonoBehaviour
{
    public Transform muzzleTransform;
    public Audio audio;
    
    public const byte sfxClip_MAX = (byte)(gunSFX.STOP + 0x01);
    [NamedArrayAttribute(typeof(gunSFX))]
    public Audio.SFX[] sfxClip = new Audio.SFX[sfxClip_MAX];

    public enum gunSFX : byte
    {
        START=0x00,
        RUN = 0x01,
        STOP = 0x02,
    }

    protected virtual void DoFire() { }

    public void StartFire()
    {
        PlayGunSFX(gunSFX.START);
        DoFire();
    }
    
    public void StopFire()
    {
        PlayGunSFX(gunSFX.STOP);
    }

    protected void PlayGunSFX(gunSFX T)
    {
        byte index = (byte)(T);
        Audio.SFX sfx = sfxClip[index];
        audio.sfx_play(sfx);
    }
}
