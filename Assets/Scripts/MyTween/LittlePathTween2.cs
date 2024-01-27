

using UnityEngine;

public class LittlePathTween2 : TweenBase
{
    public float xScale = 1f;
    public float yScale = 1f;

    private Vector3 origin;

    void Update()
    {
        if (!isPlaying)
            return;

        switch (playbackDirection)
        {
            case PlaybackDirection.FORWARD:
                value += Time.deltaTime / playbackTime;

                if (value < 1f)
                {
                    transform.localPosition = origin + new Vector3(value * xScale, curve.Evaluate(value) * yScale);
                }
                else
                {
                    isPlaying = false;

                    transform.localPosition  = origin + new Vector3(value * xScale, curve.Evaluate(1f) * yScale);
                }
                break;
            case PlaybackDirection.BACKWARD:
                value += Time.deltaTime / playbackTime;

                if (value < 1f)
                {
                    transform.localPosition = origin + new Vector3(-value * xScale, curve.Evaluate(value) * yScale);
                }
                else
                {
                    isPlaying = false;

                    transform.localPosition = origin + new Vector3(-value * xScale, curve.Evaluate(1f) * yScale);
                }
                break;
        }
    }

    public override void PlayForward()
    {
        base.PlayForward();
        origin = transform.localPosition;
    }

    public override void PlayBackward()
    {
        base.PlayBackward();
        origin = transform.localPosition;
    }
}
