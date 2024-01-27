using ByteSheep.Events;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class TimingEventsSet : MonoBehaviour
{
    public bool onStart = false;

    public TimingEvent[] eventsSet;

    private int currentIndex = 0;

    void Start()
    {
        if (onStart)
            StartTimer();
    }

    public void StartTimer()
    {
        StopAllCoroutines();
        currentIndex = 0;

        if(eventsSet.Length > 0 && gameObject.activeInHierarchy)
            StartCoroutine(wait(eventsSet[currentIndex].time));
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        eventsSet[currentIndex].onTimerEnd.Invoke();

		if((currentIndex < eventsSet.Length - 1) && gameObject.activeSelf)
        {
            currentIndex++;
            StartCoroutine(wait(eventsSet[currentIndex].time));
        }
    }

    /// <summary>
    /// Toggles the game cursor
    /// </summary>
    /// <param name="state">State</param>
    public void ToggleCursor(bool state)
    {
        GameManager.instance.ToggleCursor(state);
    }

    public void Shake(float quake)
    {
        /*
        GameObject LO = GameObject.Find("LevelObjects");
        if (LO)
        {
            Shake sh = LO.GetComponent<Shake>();
            if (sh)
            {
                sh.ShakeItBabe(quake);
            }
            //!@
            //GameManager.instance.audio.playrumble();
        }
        */
    }

    /// <summary>
    /// StarEagle-specific function to launch EagleSoft and Intro FMVs, then jump to MainMenu
    /// </summary>
    public void BOOT()
    {
        GameManager.instance.BOOT();
    }
}

[Serializable]
public class TimingEvent
{
    public float time;
    public AdvancedEvent onTimerEnd;
}
