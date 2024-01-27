
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GrayTween : TweenBase
{
    public float from;
    public float to;    
    public _2dxFX_GrayScale[] gray;
    private float timeValue=0f;

    void Update()
    {
        if (!isPlaying)
            return;

        switch (playbackDirection)
        {
            case PlaybackDirection.FORWARD:
                value += Time.unscaledDeltaTime / playbackTime;

                if (value < 1f)
                {
                    timeValue = Mathf.Lerp(from, to, curve.Evaluate(value));
                }
                else
                {
                    isPlaying = false;
                    timeValue = Mathf.Lerp(from, to, curve.Evaluate(1f));
                }
                break;
            case PlaybackDirection.BACKWARD:
                value += Time.unscaledDeltaTime / playbackTime;

                if (value < 1f)
                {
                    timeValue = Mathf.Lerp(to, from, curve.Evaluate(value));
                }
                else
                {
                    isPlaying = false;
                    timeValue = Mathf.Lerp(to, from, curve.Evaluate(1f));
                }
                break;
        }
        Apply(timeValue);
    }

    public override void PlayForward()
    {
        base.PlayForward();
    }

    public override void PlayBackward()
    {
        base.PlayBackward();
    }

    public override void StopTween(bool reset)
    {
        base.StopTween(reset);
    }

    public override void OnReset()
    {
        timeValue = Mathf.Lerp(from, to, curve.Evaluate(0f));
        Apply(timeValue);
    }

    private void Apply(float value)
    {
        foreach (_2dxFX_GrayScale g in gray)
        {
            if (g != null)
            {
                g._EffectAmount = value;
            }
        }
    }
}