using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using ByteSheep.Events;

/// <summary>
/// Loads float PlayerPrefs
/// </summary>
public class LoadPrefsFloat : MonoBehaviour
{
    public AdvancedFloatEvent onValueLoaded;    //What to do onValueLoaded(float)
    public string key;                          //The playerprefs key string (see constants in GameManager script)

    void Start()
    {
        //ResetPlayerPrefs();
        float value;                                  //Value of the key

        //Does the key exist?
        bool exists = PlayerPrefs.HasKey(key);
        if (exists)
        {
            //If so, get value to use
            value = PlayerPrefs.GetFloat(key);
        }
        else
        {
            //if DNE, fetch default value for key
            value = KeyNREError(key);
        }

        Debug.Log("Key/Value: " + key + "," + value.ToString());
        onValueLoaded.Invoke(value);
    }

    /// <summary>
    /// Handles fetching default values for keys if DNE
    /// </summary>
    /// <param name="key">Key that is DNE</param>
    /// <returns>Default value for key</returns>
    private float KeyNREError(string key)
    {        
        float value = 0;  //Value to return
        
        //Dummy RetroGFX_Settings struct instance to fetch default values from constructor
        //!@ GameManager.RetroGFX_Settings dummy = new GameManager.RetroGFX_Settings();

        //Return default value for key type
        switch (key)
        {                        
            /*
            case GameManager.PAL_ditherSize_key:
                value = dummy.palette.dither_size;
                break;
            case GameManager.PAL_ditherStrength_key:
                value = dummy.palette.dither_strength;
                break;
            case GameManager.PSX_fogMinDist_key:
                value = dummy.psx_shader.fog_minDist;
                break;
            case GameManager.PSX_fogMaxDist_key:
                value = dummy.psx_shader.fog_maxDist;
                break;
            */
        }
        return value;
    }

    /// <summary>
    /// Function used to convert a string value to float, then runs onValueLoaded with float value
    /// Used for TextField components that accept string input for floats (video settings stuff)
    /// </summary>
    public void onStringValue_ToFloat()
    {
        //Get the textfield and input field components on this gameobject
        TextField TF = this.gameObject.GetComponent<TextField>();
        InputField IF  = this.gameObject.GetComponent<InputField>();
        if (IF)
        {
            string str = IF.text;                       //Get the string of the inputfield float value
            Debug.Log("onStringValue_ToFloat " + str);

            //If good float.tryParse on float string, then invoke the onValueLoaded event for good float val
            float val = 0f;
            bool good = float.TryParse(str, out val);
            Debug.Log("Good? " + good.ToString());
            if (good)
            {
                onValueLoaded.Invoke(val);
            }
        }
        
    }

    /// <summary>
    /// Opposite of onStringValue_ToFloat. Convert the float into a string, and then sets the text for the attached TextField on this gameobject
    /// Used for InputFields that accept floats
    /// </summary>
    /// <param name="fl">Float value</param>
    public void DisplayFloat(float fl)
    {
        //Debug.Log("DisplayFloat: " + fl.ToString());
        //Get the textfield on this gameobject
        TextField TF = this.gameObject.GetComponent<TextField>();
        if (TF)
        {
            string format = "F";            //Float format prefix
            byte decdigs = TF.decDigs;      //Get amt of digits for this textifled
            format += decdigs.ToString();   //Set format as n digits of float
            //Debug.Log("Setting text to " + fl.ToString(format));
            TF.SetText(fl.ToString(format));//Apply the float format
        }
    }

    /*
    #region VideoPal
    public void SetPAL_ditherSize(float val)
    {
        GameManager.instance.SetPAL_ditherSize(val);
    }
    public void SetPAL_ditherStrength(float val)
    {
        GameManager.instance.SetPAL_ditherStrength(val);
    }
    #endregion

    #region VideoPSX
    public void SetPSX_fogMinDist(float val)
    {
        GameManager.instance.SetPSX_fogMinDist(val);
    }
    public void SetPSX_fogMaxDist(float val)
    {
        GameManager.instance.SetPSX_fogMaxDist(val);
    }
    #endregion
    */

    /// <summary>
    /// Resets all playerPrefs
    /// </summary>
    public void ResetPlayerPrefs()
    {
        GameManager.instance.ResetPlayerPrefs();
    }
}