using UnityEngine;
using ByteSheep.Events;

public class SizeTween : TweenBase
{
    public bool unscaledTime = false;         //Use deltaTime or unscaledDeltaTime?
    public Vector2 from;                    //Start size (width/height)
    public Vector2 to;                      //End size (width/height)
    public AdvancedEvent backwardEvent;     //Event to play post-backward
    public AdvancedEvent forwardEvent;      //Event to play post-forward

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();        
    }

    void Update()
    {
        if (!isPlaying)
            return;

        switch (playbackDirection)
        {
            case PlaybackDirection.FORWARD:
                if (unscaledTime)
                {
                    value += Time.unscaledDeltaTime / playbackTime;
                }
                else
                {
                    value += Time.deltaTime / playbackTime;
                }

                if (value < 1f)
                {
                    rectTransform.sizeDelta = Vector2.Lerp(from, to, curve.Evaluate(value));
                }
                else
                {
                    isPlaying = false;
                    rectTransform.sizeDelta = Vector2.Lerp(from, to, curve.Evaluate(1f));
                    forwardEvent.Invoke();
                }
                break;
            case PlaybackDirection.BACKWARD:
                if (unscaledTime)
                {
                    value += Time.unscaledDeltaTime / playbackTime;
                }
                else
                {
                    value += Time.deltaTime / playbackTime;
                }

                if (value < 1f)
                {
                    rectTransform.sizeDelta = Vector2.Lerp(to, from, curve.Evaluate(value));
                }
                else
                {
                    isPlaying = false;
                    rectTransform.sizeDelta = Vector2.Lerp(to, from, curve.Evaluate(1f));
                    backwardEvent.Invoke();
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
        if (isPlaying)
        {
            //If was playing, determine direction and then fire appropriate events
            if (playbackDirection == PlaybackDirection.BACKWARD)
            {
                backwardEvent.Invoke();
            }
            else
            {
                forwardEvent.Invoke();
            }
        }
        base.StopTween(reset);
    }

    public override void OnReset()
    {        
        rectTransform.sizeDelta = Vector2.one * 100f;
    }
}