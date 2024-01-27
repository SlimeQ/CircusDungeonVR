using UnityEngine;

public class PositionTween : TweenBase
{
    public bool global = false; //!@ Use global coords for dynamic tween (Tto/Tfrom)? If so, you must use 3D coords and transform vs. Rectransform for now.
    public Vector3 from;        //!@ Extended to Vector3D
    public Vector3 to;          //!@ Extended to Vector3D

    //Dynamic to/from positions
    public Transform Tfrom;     //From transform
    public Transform Tto;       //To transform

    private RectTransform rectTransform;

    void Update()
    {        
        if (!isPlaying)
            return;

        Vector3 A;  //from vector for dynamic
        Vector3 B;  //to vector for dynamic

        switch (playbackDirection)
        {
            case PlaybackDirection.FORWARD:
                value += Time.deltaTime / playbackTime;

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
                        A = Tfrom.transform.localPosition;
                        B = Tto.transform.localPosition;
                    }
                    else
                    {
                        A = Tfrom.transform.position;
                        B = Tto.transform.position;
                    }
                }

                //Do the lerp
                if (value < 1f)
                {
                    if (rectTransform)
                    {                 
                        rectTransform.anchoredPosition = Vector2.Lerp(A, B, curve.Evaluate(value));
                    }
                    else
                    {
                        //Use appropriate local/global pos
                        if (global == false)
                        {
                            transform.localPosition = Vector3.Lerp(A, B, curve.Evaluate(value));
                        }
                        else
                        {
                            transform.position = Vector3.Lerp(A, B, curve.Evaluate(value));
                        }
                    }
                }
                else
                {
                    isPlaying = false;

                    //Do the lerp
                    if (rectTransform)
                    {
                        rectTransform.anchoredPosition = Vector2.Lerp(A, B, curve.Evaluate(1f));
                    }
                    else
                    {
                        //Use appropriate local/global pos
                        if (global == false)
                        {
                            transform.localPosition = Vector3.Lerp(A, B, curve.Evaluate(1f));
                        }
                        else
                        {
                            transform.position = Vector3.Lerp(A, B, curve.Evaluate(1f));
                        }
                    }
                }
                break;

            case PlaybackDirection.BACKWARD:
                value += Time.deltaTime / playbackTime;

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
                        A = Tfrom.transform.localPosition;
                        B = Tto.transform.localPosition;
                    }
                    else
                    {
                        A = Tfrom.transform.position;
                        B = Tto.transform.position;
                    }
                }

                if (value < 1f)
                {
                    //Do the lerp
                    if (rectTransform)
                    {
                        rectTransform.anchoredPosition = Vector2.Lerp(B, A, curve.Evaluate(value));
                    }
                    else
                    {
                        //Use appropriate local/global pos
                        if (global == false)
                        {
                            transform.localPosition = Vector3.Lerp(B, A, curve.Evaluate(value));
                        }
                        else
                        {
                            transform.position = Vector3.Lerp(B, A, curve.Evaluate(value));
                        }
                    }
                }
                else
                {
                    isPlaying = false;

                    //Do the lerp
                    if (rectTransform)
                    {
                        rectTransform.anchoredPosition = Vector2.Lerp(B, A, curve.Evaluate(1f));
                    }
                    else
                    {
                        //Use appropriate local/global pos
                        if (global == false)
                        {
                            transform.localPosition = Vector3.Lerp(B, A, curve.Evaluate(1f));
                        }
                        else
                        {
                            transform.position = Vector3.Lerp(B, A, curve.Evaluate(1f));
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
        transform.localPosition = Vector3.zero;
    }
}