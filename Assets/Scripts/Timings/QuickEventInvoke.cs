
//shoulda switch to quickevents before...

using ByteSheep.Events;
using UnityEngine;

public class QuickEventInvoke : MonoBehaviour
{
    public AdvancedEvent advancedEvent;

    public void InvokeEm()
    {
        advancedEvent.Invoke();
    }
}
