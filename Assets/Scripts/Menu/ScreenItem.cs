using ByteSheep.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class ScreenItem : MonoBehaviour
{
    //Support flags for this Screen
    public bool supWin = true;      //Enabled on Windows?
    public bool supLinux = true;    //Linux?
    public bool supMac = true;      //Mac?
    //public bool supXB = true;     //XB?
    public bool supDemo = true;     //Must be true to be used in demo. If not demo, is disabled. This overrides all other above Sup platforms

    public Sprite active;           //Active sprite to use for ScreenItem box
    public Sprite inactive;         //Inactive sprite
    
    //!@
    //public UnityEvent onFocus;
    //public UnityEvent onUnFocus;
    //public UnityEvent onActivate;     //Usually used for 1st activation; however, if swapActivate==true, used for 2nd. This is used for PauseMenu "Are you sure?"
    //public UnityEvent onActivate2;    //Used for 1st activation
    public AdvancedEvent onFocus;       //Events to run on focus
    public AdvancedEvent onUnFocus;     //Events to run when focus is lost
    public AdvancedEvent onActivate;    //Usually used for 1st activation; however, if swapActivate==true, used for 2nd. This is used for PauseMenu "Are you sure?"
    public AdvancedEvent onActivate2;   //Used for 1st activation
    public bool swapActivate = false;   //???
    private bool swapRunOnce = false;   //???
    public bool enabled = true;         //Is this ScreenItem enabled?    
    
    //used for left/right events
    //public FloatUnityEvent onValue;
    public AdvancedFloatEvent onValue;  //Event to run on float value change

    /// <summary>
    /// Give focus and run events
    /// </summary>
    public void Focus()
    {
        onFocus.Invoke();
    }

    /// <summary>
    /// Remove focus and run events
    /// </summary>
    public void UnFocus()
    {
        onUnFocus.Invoke();
    }

    /// <summary>
    /// Run events on float value change
    /// </summary>
    /// <param name="value">New value</param>
    public void OnValue(float value)
    {
        onValue.Invoke(value);
    }

    /// <summary>
    /// Activates the screen item and runs events
    /// </summary>
    public void Activate()
    {
        if (swapActivate == false)
        {
            if (enabled == true)
            {
                onActivate.Invoke();
            }
        }
        else
        {
            if (enabled == true)
            {
                if (swapRunOnce == false)
                {
                    onActivate2.Invoke();
                    swapRunOnce = true;
                }
                else
                {
                    onActivate.Invoke();
                }
            }
        }
    }

    /// <summary>
    /// Resets swapRunOnce
    /// </summary>
    public void resetActivateRunonce()
    {
        swapRunOnce = false;
    }

    /// <summary>
    /// Get the count of items in the activation event
    /// </summary>
    /// <returns>int of items</returns>
    public int GetActivateCount()
    {
        int result = onActivate.GetPersistentEventCount();
        return result;
    }

    /// <summary>
    /// Toggles the enable state of ScreenItem and its text, pointer, and box
    /// </summary>
    /// <param name="val">State as bit</param>
    public void ToggleEnabled(int val)
    {
        try
        {
            enabled = (val == 1);
            List<Text> TT = new List<Text>();                           //List of text elements
            Text[] T = this.gameObject.GetComponentsInChildren<Text>(); //Get all text in children
            foreach (Text t in T)
            {
                //!@
                //See if this text element has a TextField
                /*
                TextField tf = t.transform.parent.gameObject.GetComponent<TextField>();
                if (tf)
                {
                    //If so, additionally toggle its input
                    tf.ready = (val == 1);
                }
                */
                TT.Add(t);  //Add text to list
            }

            foreach (Text t in TT)
            {
                Color clr = new Color(1f, 1f, 1f, 1f);  //If state=on, white; else grey color
                if (val == 0)
                {
                    clr = new Color(.5f, .5f, .5f, 1f);
                }
                t.color = clr;  //Apply color
            }

            //Change the animation type of pointer (if enabled)
            GameObject TP = this.gameObject.transform.Find("Pointer").gameObject;
            if (TP)
            {
                //Get Pointer's aniamtor, set the disable flag for animation ("X" object)
                Animator anim = TP.GetComponent<Animator>();
                if (anim)
                {
                    if (anim.isActiveAndEnabled)
                    {
                        anim.SetBool("dis", !enabled);
                    }
                }
            }

            //Change the sprite of the box image
            Image img = this.gameObject.GetComponent<Image>();
            if (img)
            {
                if (enabled == true)
                {
                    img.sprite = active;
                }
                else
                {
                    img.sprite = inactive;
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.Log("Caught error in ScreenItem.ToggleEnabled");
            Debug.LogWarning(ex.Message);
        }
    }

    /// <summary>
    /// Changes the sprite of the ScreenItem between active and inactive sprites
    /// </summary>
    public void ToggleSprite()
    {
        //Change the sprite of the box image
        Image img = this.gameObject.GetComponent<Image>();
        if (img)
        {
            if (img.sprite == active)
            {
                img.sprite = inactive;
            }
            else
            {
                img.sprite = active;
            }
        }       
    }
}