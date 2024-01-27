

using UnityEngine;
using UnityEngine.Events;

public class RandomEventSelection : MonoBehaviour
{
    public UnityEvent[] randomEventsSet;

    public void InvokeRandom()
    {
        if (randomEventsSet.Length > 0)
        {
            //GameManager.instance.RandomSeed();
            randomEventsSet[Random.Range(0, randomEventsSet.Length)].Invoke();
        }
    }
}
