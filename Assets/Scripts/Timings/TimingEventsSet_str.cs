using ByteSheep.Events;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

/// <summary>
/// Same as TimingEventsSet, but handles events requiring string input
/// </summary>
public class TimingEventsSet_str: MonoBehaviour
{
    public bool onStart = false;
    public string DaString = "";            //String param to use for current event. Set this to something for onStart event

    public TimingEvent_str[] eventsSet;

    private int currentIndex = 0;

    void Start()
    {
        //If onStart event, fire it with DaString
        if (onStart)
            StartTimer(DaString);
    }

    /// <summary>
    /// Starts the TES string events
    /// </summary>
    /// <param name="str">String event param</param>
    public void StartTimer(string str)
    {
        StopAllCoroutines();
        currentIndex = 0;

        if(eventsSet.Length > 0 && gameObject.activeInHierarchy)
            StartCoroutine(wait(eventsSet[currentIndex].time));
        DaString = str; //Set DaString to param
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        eventsSet[currentIndex].onTimerEnd.Invoke(DaString);    //Invoke event with DaString

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
}

[Serializable]
/// <summary>
/// TimingEvent string event
/// </summary>
public class TimingEvent_str
{
    public float time;                      //Time for event
    public AdvancedStringEvent onTimerEnd;  //String event to fire onTimerEnd
}
