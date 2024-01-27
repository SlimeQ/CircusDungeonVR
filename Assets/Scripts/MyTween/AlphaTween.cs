using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlphaTween : TweenBase
{
    public float from;
    public float to;

    private SpriteRenderer SR;
    private MaskableGraphic image;
    private CanvasGroup group;

    void Start()
    {
        group = GetComponent<CanvasGroup>();
        image = GetComponent<MaskableGraphic>();
        SR = GetComponent<SpriteRenderer>();
    }

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
                    if(group)
                        group.alpha = Mathf.Lerp(from, to, curve.Evaluate(value));
                    else if(image)
                    {
                        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(from, to, curve.Evaluate(value)));
                    }
                    else if(SR)
                    {
                        SR.color = new Color(SR.color.r,SR.color.g,SR.color.b, Mathf.Lerp(from, to, curve.Evaluate(value)));
                    }
                }
                else
                {
                    isPlaying = false;

                    if (group)
                        group.alpha = Mathf.Lerp(from, to, curve.Evaluate(1f));
                    else if(image)
                    {
                        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(from, to, curve.Evaluate(1f)));
                    }
                    else if(SR)
                    {
                        SR.color = new Color(SR.color.r,SR.color.g,SR.color.b, Mathf.Lerp(from, to, curve.Evaluate(1f)));
                    }
                }
                break;
            case PlaybackDirection.BACKWARD:
                value += Time.unscaledDeltaTime / playbackTime;

                if (value < 1f)
                {
                    if (group)
                    {
                        group.alpha = Mathf.Lerp(to, from, curve.Evaluate(value));
                    }
                    else if(image)
                    {
                        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(to, from, curve.Evaluate(value)));
                    }
                    else if(SR)
                    {
                        SR.color = new Color(SR.color.r,SR.color.g,SR.color.b, Mathf.Lerp(to, from, curve.Evaluate(value)));
                    }

                }
                else
                {
                    isPlaying = false;

                    if (group)
                    {
                        group.alpha = Mathf.Lerp(to, from, curve.Evaluate(1f));
                    }
                    else if(image)
                    {
                        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(to, from, curve.Evaluate(1f)));
                    }
                    else if(SR)
                    {
                        SR.color = new Color(SR.color.r,SR.color.g,SR.color.b, Mathf.Lerp(to, from, curve.Evaluate(1f)));
                    }
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
        if(image)
            image.color = Color.white;
    }
}