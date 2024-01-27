
using UnityEngine;
using UnityEngine.UI;

public class fillTween : TweenBase
{
    public Image img;
    public Vector3 from;
    public Vector3 to;

    void Update()
    {
        Vector3 v = Vector3.zero;
        if (!isPlaying)
            return;

        switch (playbackDirection)
        {
            case PlaybackDirection.FORWARD:
                value += Time.deltaTime / playbackTime;
                if (value < 1f)
                {
                    v = Vector3.Lerp(from, to, curve.Evaluate(value));
                    img.fillAmount = v.x;
                }
                else
                {
                    isPlaying = false;
                    v = Vector3.Lerp(from, to, curve.Evaluate(1f));
                    img.fillAmount = v.x;
                }
                break;
            case PlaybackDirection.BACKWARD:
                value += Time.deltaTime / playbackTime;

                if (value < 1f)
                {
                    v = Vector3.Lerp(to, from, curve.Evaluate(value));
                    img.fillAmount = v.x;
                }
                else
                {
                    isPlaying = false;
                    v = Vector3.Lerp(to, from, curve.Evaluate(1f));
                    img.fillAmount = v.x;
                }
                break;
        }
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
        transform.localScale = Vector3.one;
    }
}