using UnityEngine;

/// <summary>
/// Handles a ControlsScreen object
/// </summary>
public class ContolsScreen : MonoBehaviour
{
    /*
    public ControlsSwitch[] controlsSwitches;   //List of controls switches

    /// <summary>
    /// Calls ControlsSwitch.TogglePlayer on all elements in controlsSwitches
    /// </summary>
    //public void TogglePlayer(int id)
    //{
    //    foreach (ControlsSwitch c in controlsSwitches)
    //    {
    //        c.TogglePlayer(id);
    //        c.Assign();
    //    }
    //}

    /// <summary>
    /// Resets all controlsSwitches
    /// </summary>
    public void ResetAll()
    {
        GameManager.instance.CreateDefaultControls();
        foreach (ControlsSwitch controlSwitch in controlsSwitches)
        {
            //!@
            //controlSwitch.Assign();
        }
    }

    /// <summary>
    /// Saves the controls to file
    /// </summary>
    public void SaveControls()
    {
        GameManager.instance.SaveControls();
    }

    /// <summary>
    /// Sets the joystick ID
    /// </summary>
    /// <param name="id">ID</param>
    public void SetJoystickID(int id)//player 1 only
    {        
        GameManager.currentControls.controls[0].joystickId = id;
        Debug.Log("SetJoystickID run: " + GameManager.currentControls.controls[0].joystickId.ToString());
    }

    /// <summary>
    /// Set the joystick type for the player
    /// </summary>
    //public void SetJoystickType_P1(int id)
    public void SetJoystickType(int id)
    {
        GameManager.currentControls.controls[0].joystickType = id;
    }
    public void SetJoystickType_P2(int id)
    {
        GameManager.currentControls.controls[1].joystickType = id;
    }
    */
}
