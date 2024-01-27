
using System;
using System.Collections.Generic;

/// <summary>
/// Holds controls (duh)
/// </summary>
[Serializable]
public class ControlsHolder
{
    public List<ControlsSetup> controls;                            //List of controlsetups

    public ControlsHolder(params ControlsSetup[] initialControls)
    {
        controls = new List<ControlsSetup>();

        if(initialControls.Length > 0)
            controls.AddRange(initialControls);
    }
}
