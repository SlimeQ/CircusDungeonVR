using UnityEngine;

public class CanvasGroupAlpha : TweenBase
{
    public bool unscaledTime = false;
    public float from;
    public float to;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (!isPlaying)
            return;

        switch (playbackDirection)
        {
            case PlaybackDirection.FORWARD:
                if (unscaledTime == false)
                {
                    value += Time.deltaTime / playbackTime;
                }
                else
                {
                    value += Time.unscaledDeltaTime / playbackTime;
                }

                if (value < 1f)
                    canvasGroup.alpha = Mathf.Lerp(from, to, curve.Evaluate(value));
                else
                {
                    isPlaying = false;
                    canvasGroup.alpha = Mathf.Lerp(from, to, curve.Evaluate(1f));
                }
                break;
            case PlaybackDirection.BACKWARD:
                if (unscaledTime == false)
                {
                    value += Time.deltaTime / playbackTime;
                }
                else
                {
                    value += Time.unscaledDeltaTime / playbackTime;
                }

                if (value < 1f)
                    canvasGroup.alpha = Mathf.Lerp(to, from, curve.Evaluate(value));
                else
                {
                    isPlaying = false;
                    canvasGroup.alpha = Mathf.Lerp(to, from, curve.Evaluate(1f));
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
        if(canvasGroup)
            canvasGroup.alpha = 1f;
    }
}