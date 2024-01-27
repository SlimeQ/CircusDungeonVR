using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using ByteSheep.Events;

/// <summary>
/// Defines and handles events when dis/enabling an object
/// </summary>
public class OnEnable_Event : MonoBehaviour
{
    public AdvancedEvent onEnable;      //Events to run onEnable
    public AdvancedEvent onDisable;     //Events to run onDisable
    public bool state = false;          //Enable state

    /// <summary>
    /// Toggles enable state, run appropriate onDis/enable event
    /// </summary>
    /// <param name="_state">New state</param>
    public void ToggleEnable(bool _state)
    {
        //If a state change
        if (state != _state)
        {
            state = _state;         //Set new state
            //If disable, disable; else enable
            if (state == false)
            {
                Disable();
            }
            else
            {
                Enable();
            }
        }
    }

    /// <summary>
    /// Run enable events
    /// </summary>
    private void Enable()
    {
        onEnable.Invoke();
    }

    /// <summary>
    /// Run disable events
    /// </summary>
    private void Disable()
    {        
        onDisable.Invoke();
    }
}