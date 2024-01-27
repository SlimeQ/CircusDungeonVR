using ByteSheep.Events;
using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Similar to Filename/PasswordInputField, but only accepts kbd. Used for text or float inputs
/// </summary>
public class TextField : MonoBehaviour
{
    public string currentString;
    public InputField inputField;
    public Audio audio;                         //Audio script    
    public ScreenList SList;                    //Screenlist this belongs textField belongs to
    public int SListIndex;                      //The index for this item in ScreenList    

    public bool useDigs = true;                 //Accept digits?  (0-9,.)
    public bool useChar = true;                 //Accept alpha?
    public byte decDigs = 2;                    //Amount of digits past dec place
    public Vector2 numRange = Vector2.up;       //Range for numeric values. x=min float, y=max float
    public byte maxChar;                        //Max character limit for text field

    public bool ready = false; //Is this component ready for text input?

    public AdvancedStringEvent onEndEdit;       //The onEndEdit event on TextField is unreliable, and may fire (?) on game init. Use this workaround instead

    //Fetch the audio script
    void Start()
    {
        audio = this.gameObject.GetComponent<Audio>();        
    }

    /// <summary>
    /// If using a kbd, do kbd button presses
    /// </summary>
    void LateUpdate()
    {
        //Only process keypresses if ready
        if (ready == false)
        {
            return;
        }        
        inputField.textComponent.horizontalOverflow = HorizontalWrapMode.Wrap;
        if (SList.currentIndex==SListIndex)
        { 
            if(Input.inputString.Length != 0)
            {
                char chr = Input.inputString[0];    //Char to potentially input
                bool cond = false;                  //If test condition is true, add text

                //If useDigs, check if char is a dig char
                if (useDigs)
                {
                    cond |=((chr >= '0' && chr <= '9')||(chr == '.'));
                }

                //If useChar, check if char is a char (A-Z low/upper)
                if(useChar)
                {
                    cond |=((chr >= 'a' && chr <= 'z') || (chr >= 'A' && chr <= 'Z'));
                }
                
                //if condition passed, add text
                if(cond)
                {
                    AddText(chr.ToString(),false);
                }

                //If backspace, then remove char
                if (Input.GetKeyDown(KeyCode.Backspace) == true)
                {
                    RemoveChar();
                }
            }
        }
    }

    public void AddText(string text, bool overriden)
    {
        //Debug.Log("Bool: " + (inputField.text.Length < 5).ToString() + " || " + overriden.ToString());
        bool good = ((inputField.text.Length < maxChar) || (overriden));
        //Debug.Log("Good: " + good.ToString());
        if(good)
        {
            inputField.text = currentString + text;
            //audio.sfx_play(Audio.SFX.SFX_KBD);    //!@
        }
    }

    //called by inputField when value is changed
    public void SetText(string text)
    {
        currentString = text;
        inputField.text = text;
    }

    public void RemoveChar()
    {
        if(currentString.Length > 0)
        {
            currentString = currentString.Substring(0, currentString.Length - 1);
            AddText(null,true);
            //  audio.sfx_play(Audio.SFX.SFX_KBD);  //!@
        }
    }

    /// <summary>
    /// For numeric input, validates the text. 
    /// Clamps value between min/max acceptable ranges and sets new text, and sets new text to min if invalid text input
    /// Used by float inputs in Video Settings menu
    /// </summary>
    /// <param name="str">String float to validate</param>
    public void ValidateNumber(string str)
    {
        //if ((str == "8.88") || (str == "8888.88"))
        //{
        //    Debug.LogError("Your mom");
        //}

        //Try parsing the string as float; if not parsable, set float as min value of acceptable range
        Debug.Log("ValidateNumber: " + str);
        float fl = 0f;
        bool good = float.TryParse(str, out fl);
        Debug.Log("Good? " + good.ToString());
        if (!good)
        {
            fl = numRange.x;
        }

        //Clamp the float between min and max range
        Debug.Log("Clamp: " + numRange.x.ToString() +", "+ numRange.y.ToString());
        fl = Mathf.Clamp(fl, numRange.x, numRange.y);

        //Set new string as float with N digs, then set the new string
        str = fl.ToString("F" + decDigs.ToString());
        Debug.Log("New val: " + fl + " " + str);
        SetText(str);        
    }

    /// <summary>
    /// Invoke onEndEdit event for the current inputField text
    /// </summary>
    public void invoke_onEndEdit()
    {        
        Debug.Log("Invoking onEndEdit");
        onEndEdit.Invoke(inputField.text);
    }
}
