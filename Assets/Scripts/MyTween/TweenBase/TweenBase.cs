

using UnityEngine;

public class TweenBase : MonoBehaviour
{
    public AnimationCurve curve;
    public float playbackTime;
    public float multFact=1f;    

    [HideInInspector]
    public float value;
    [HideInInspector]
    public PlaybackDirection playbackDirection;
    [HideInInspector]
    public bool isPlaying = false;
    
    public virtual void PlayForward()
    {
        value = 0f;
        isPlaying = true;
        playbackDirection = PlaybackDirection.FORWARD;
    }

    public virtual void PlayBackward()
    {
        value = 0f;
        isPlaying = true;
        playbackDirection = PlaybackDirection.BACKWARD;
    }

    public virtual void StopTween(bool reset)
    {
        isPlaying = false;

        if (reset)
            OnReset();
    }

    public virtual void OnReset() { }
}
