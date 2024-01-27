using Rewired;
using System;
using UnityEngine;

/// <summary>
/// Handles all controllers
/// </summary>
/*
public class ControlsHandler : MonoBehaviour
{
    public static ControlsHandler instance; //Singleton instance

    public InputDevice[] inputDevices;      //All of the input devices attached
    [HideInInspector]
    public int devicesCount;                //The count of devices

    void Start()
    {
        //Singleton setup
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        //Create cawbacks for DeviceDe/attached
        InputManager.OnDeviceAttached += InputManager_OnDeviceAttached;
        InputManager.OnDeviceDetached += InputManager_OnDeviceDetached;

        SetupDevices();
    }

    /// <summary>
    /// Setup the devices
    /// </summary>
    public void SetupDevices()
    {
        devicesCount = Enum.GetNames(typeof(ControlledBy)).Length - 1;
        Debug.Log("DevicesCount: " + devicesCount.ToString());
        Debug.Log("DevicesCount_InputManager: " + InputManager.Devices.Count);
        inputDevices = new InputDevice[devicesCount];

        for (int i = 0; i < devicesCount; i++)
        {
            if (i < InputManager.Devices.Count)
            {
                inputDevices[i] = InputManager.Devices[i];
                Debug.Log("Input device #" + i.ToString() + " mapped! Name: " + InputManager.Devices[i].Name);
            }
        }

        Debug.Log("Input device count: " + inputDevices.Length.ToString());
    }

    /// <summary>
    /// Cawback to setup devices on detach
    /// </summary>
    /// <param name="obj"></param>
    private void InputManager_OnDeviceDetached(InputDevice obj)
    {
        SetupDevices();
    }

    /// <summary>
    /// Cawback to setup devices on attach
    /// </summary>
    /// <param name="obj"></param>
    private void InputManager_OnDeviceAttached(InputDevice obj)
    {
        SetupDevices();
    }
}
*/

/// <summary>
/// Who controls this device?
/// </summary>
[Serializable]
public enum ControlledBy
{
    PLAYER1,
    //PLAYER2,
    //PLAYER3,
    //PLAYER4,
    ANY_PLAYER
}