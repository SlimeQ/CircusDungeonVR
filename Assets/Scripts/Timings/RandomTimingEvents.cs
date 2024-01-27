

using UnityEngine;
using UnityEngine.Events;

public class RandomTimingEvents : MonoBehaviour
{
    public UnityEvent events;
    public AudioClip[] clips;   //AudioClips for RandomClips

    [HideInInspector]
    public float from = 1f;
    [HideInInspector]
    public float to = 2f;
    [HideInInspector]
    public float offsetFrom = 1f;
    [HideInInspector]
    public float offsetTo = 2f;

    public bool onStart = false;
    public bool repeat = false;

    void Start()
    {
        if (onStart)
            CallWithDefaultOffset();
    }

    public void CallWithDefaultOffset()
    {
        CallEvents(Random.Range(offsetFrom, offsetTo));
    }

    public void CallEvents(float offset = 0f)
    {
        Invoke("ExecuteEvents", Random.Range(from, to) + offset);
    }

    public void ExecuteEvents()
    {
        events.Invoke();

        if (repeat)
            CallEvents();
    }

    public void StopRepeating()
    {
        CancelInvoke();
    }

    /// <summary>
    /// Play a random clip in pool
    /// </summary>
    public void RandomClip()
    {
        AudioSource AS = this.gameObject.GetComponent<AudioSource>();
        if (AS)
        {
            byte max=(byte)(clips.GetLength(0));
            byte ind = (byte)(UnityEngine.Random.Range(0, max));
            AudioClip clip=clips[ind];
            AS.PlayOneShot(clip);
        }
    }
}
