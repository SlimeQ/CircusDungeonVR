

using UnityEngine;

public class ScaleTween : TweenBase
{
    public Vector3 from;
    public Vector3 to;

    void Update()
    {
        if (!isPlaying)
            return;

        switch (playbackDirection)
        {
            case PlaybackDirection.FORWARD:
                value += Time.deltaTime / playbackTime;
                if (value < 1f)
                    transform.localScale = Vector3.Lerp(from, to, curve.Evaluate(value));
                else
                {
                    isPlaying = false;
                    transform.localScale = Vector3.Lerp(from, to, curve.Evaluate(1f));
                }
                break;
            case PlaybackDirection.BACKWARD:
                value += Time.deltaTime / playbackTime;

                if (value < 1f)
                    transform.localScale = Vector3.Lerp(to, from, curve.Evaluate(value));
                else
                {
                    isPlaying = false;
                    transform.localScale = Vector3.Lerp(to, from, curve.Evaluate(1f));
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

    /// <summary>
    /// Same as PlayBackward, but object owning this script self destructs afterwards, as well as an additional object ref
    /// </summary>
    public void PlayBackward_selfdestruct(GameObject go)
    {
        float delay = base.playbackTime;
        float shortdelay = delay - .01f;
        base.PlayBackward();
        Destroy(go, shortdelay);
        Destroy(this.gameObject, delay);
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