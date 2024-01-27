using UnityEngine;

public class RotationTween : TweenBase
{
    public bool global = false; //!@ Use global coords for dynamic tween (Tto/Tfrom)? If so, you must use 3D coords and transform vs. Rectransform for now.
    public Vector3 from;
    public Vector3 to;

    //Dynamic to/from positions
    public Transform Tfrom;     //From transform
    public Transform Tto;       //To transform

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!isPlaying)
            return;

        Vector3 A;  //from euler vector for dynamic
        Vector3 B;  //to euler vector for dynamic

        switch (playbackDirection)
        {
            case PlaybackDirection.FORWARD:
                value += Time.unscaledDeltaTime / playbackTime;

                if (!Tfrom && !Tto)
                {
                    //if not using dynamic, set A & B as from/to respectively
                    A = from;
                    B = to;
                }
                else
                {
                    //if using dynamic
                    //set A & B as Tfrom/Tto respectively, and use either local/global pos based on global flag
                    if (global == false)
                    {
                        A = Tfrom.transform.localEulerAngles;
                        B = Tto.transform.localEulerAngles;
                    }
                    else
                    {
                        A = Tfrom.transform.eulerAngles;
                        B = Tto.transform.eulerAngles;
                    }
                }

                if (value < 1f)
                {
                    if (rectTransform)
                    {
                        rectTransform.localEulerAngles = Vector3.Lerp(A, B, curve.Evaluate(value));
                    }
                    else
                    {
                        //Use appropriate local/global rot
                        if (global == false)
                        {
                            transform.localEulerAngles = Vector3.Lerp(A, B, curve.Evaluate(value));
                        }
                        else
                        {
                            transform.eulerAngles = Vector3.Lerp(A, B, curve.Evaluate(value));
                        }
                    }
                }
                else
                {
                    isPlaying = false;

                    if (rectTransform)
                    {
                        rectTransform.localEulerAngles = Vector3.Lerp(A, B, curve.Evaluate(1f));
                    }
                    else
                    {
                        //Use appropriate local/global pos
                        if (global == false)
                        {
                            transform.localEulerAngles = Vector3.Lerp(A, B, curve.Evaluate(1f));
                        }
                        else
                        {
                            transform.eulerAngles = Vector3.Lerp(A, B, curve.Evaluate(1f));
                        }
                    }
                }
                break;
            case PlaybackDirection.BACKWARD:
                value += Time.unscaledDeltaTime / playbackTime;

                if (!Tfrom && !Tto)
                {
                    //if not using dynamic, set A & B as from/to respectively
                    A = from;
                    B = to;
                }
                else
                {
                    //if using dynamic
                    //set A & B as Tfrom/Tto respectively, and use either local/global pos based on global flag
                    if (global == false)
                    {
                        A = Tfrom.transform.localEulerAngles;
                        B = Tto.transform.localEulerAngles;
                    }
                    else
                    {
                        A = Tfrom.transform.eulerAngles;
                        B = Tto.transform.eulerAngles;
                    }
                }

                if (value < 1f)
                {
                    if (rectTransform)
                    {
                        rectTransform.localEulerAngles = Vector3.Lerp(B, A, curve.Evaluate(value));
                    }
                    else
                    {
                        //Use appropriate local/global pos
                        if (global == false)
                        {
                            transform.localEulerAngles = Vector3.Lerp(B, A, curve.Evaluate(value));
                        }
                        else
                        {
                            transform.eulerAngles = Vector3.Lerp(B, A, curve.Evaluate(value));
                        }
                    }
                }
                else
                {
                    isPlaying = false;
                    if (rectTransform)
                    {
                        rectTransform.localEulerAngles = Vector3.Lerp(B, A, curve.Evaluate(1f));
                    }
                    else
                    {
                        //Use appropriate local/global pos
                        if (global == false)
                        {
                            transform.localEulerAngles = Vector3.Lerp(B, A, curve.Evaluate(1f));
                        }
                        else
                        {
                            transform.eulerAngles = Vector3.Lerp(B, A, curve.Evaluate(1f));
                        }
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
        if (rectTransform)
        {
            rectTransform.eulerAngles = Vector3.zero;
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
}