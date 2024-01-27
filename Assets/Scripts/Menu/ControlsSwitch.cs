using System.Collections;
using UnityEngine;
using ByteSheep.Events;
using UnityEngine.UI;
//using Rewired;

/// <summary>
/// Complex script for enabling the button remapping on the appropriate kbd and gamepad menus
/// </summary>
public class ControlsSwitch : MonoBehaviour
{
    /*
    public ActionKey key;                           //Key type for remapping
    public ControlledBy playerId;                   //Who controls this script? (ControlledBy)

    public AdvancedIntEvent onJoystickIDLoaded;     //Event to run on    JoystickID loaded
    public AdvancedIntEvent onJoystickTypeLoaded;   //~                 ~Joystick type loaded

    public AdvancedEvent onChanged;                 //Event to run on key remap changed
    public AdvancedEvent onCanceled;                //Event to run on key remap cancelled

    private Text uiText;                            //Text for displaying the name of the remapped key
    private bool isWaitingForInput = false;         //Is the remapping waiting for remap input?
    private bool isReady = false;                   //Is this script ready?
    private string prevText;                        //Previous text for uiText

    void Awake()
    {
        //On awake, set uiText to this object' Text component
        uiText = GetComponent<Text>();
    }

    IEnumerator Start()
    {
        //Yield to prevent NREs,
        //and to wait until all keys toggle once in kbdRef start thread
        //for synchronization purposes

        yield return new WaitForSeconds(.5f);
        bool isKbd = ((key >= ActionKey.ARROW_UP) && (key <= ActionKey.BACK_KEY));
        if (isKbd == false)
        {
            //If key is not a kbd key, handle gamepad
            Assign();
        }
        else
        {
            //If kbd, assign for both player's keysets
            //ControlledBy i = ControlledBy.PLAYER1;
            //for (i = ControlledBy.PLAYER1; i <= ControlledBy.PLAYER2; i++)
            //for (i = ControlledBy.PLAYER1; i < ControlledBy.PLAYER2; i++)
            //{
                //playerId = i;
                //Assign();
            //}

            //If kbd, assign for Player1
            playerId = ControlledBy.PLAYER1;
            Assign();   //Switch back to P1 after the switch
        }

        //Enter and esc keys are hardcoded to Start/Back, respectively for P1, so highlgiht those keys
        //kbdRef.instance.ToggleKey("ENTER", true);
        //kbdRef.instance.ToggleKey("ESC", true);
    }

    /// <summary>
    /// Code to run OnGUI for remapping
    /// </summary>
    void OnGUI()
    {
        //Text string of old/new keys
        string oldKey = "";
        string newKey = "";
        char t = ' ';               //Generic char (Result from ArrowGlpyh func)

        if (isWaitingForInput)
        {
            //Do stuff if waiting for remap input

            bool isBtn = (key >= ActionKey.A_BTN && key <= ActionKey.BACK_BTN); //Is key a button?
            bool isDup=false;                                                   //Is remap key a duplicate?
            ActionKey conflict = ActionKey.JOYSTICK_1;                          //Which Actionkey got a conflict?

            if(isBtn==false)
            {
                //If not a button (kbd)

                //Only accept non-null key, keys, A-Z, Left/Right/Up/Down, Escape
                bool valid=((Event.current.keyCode != KeyCode.None) && (Event.current.isKey));
                //bool valid2 = ((((Event.current.keyCode >= KeyCode.A) && (Event.current.keyCode <= KeyCode.Z)) || (Event.current.keyCode == KeyCode.Escape) || (Event.current.keyCode == KeyCode.Tab)) || ((Event.current.keyCode >= KeyCode.UpArrow) && (Event.current.keyCode <= KeyCode.LeftArrow)));
                bool valid2 = ((((Event.current.keyCode >= KeyCode.A) && (Event.current.keyCode <= KeyCode.Z)) || (Event.current.keyCode == KeyCode.Escape) || (Event.current.keyCode == KeyCode.Tab) || (Event.current.keyCode == KeyCode.Space)) || ((Event.current.keyCode >= KeyCode.UpArrow) && (Event.current.keyCode <= KeyCode.LeftArrow)));
                bool valid3 = (valid && valid2);
                if (valid3)
                {
                    //If input is valid

                    KeyCode pressed = Event.current.keyCode;    //Get keycode of input
                    switch (pressed)
                    {
                        //cancel
                        case KeyCode.Escape:
                            uiText.text = prevText;
                            t = ArrowGlyphs(uiText.text);   //Try converting a key string to an arrow glyph
                            if (t != ' ')
                            {
                                //If it succeeded, set text to new glyph instead
                                uiText.text = t.ToString();
                            }
                            isWaitingForInput = false;
                            onCanceled.Invoke();            //Run cancel event
                            return;
                            break;

                        default:
                            //if ((pressed != KeyCode.Return) && (pressed != KeyCode.Escape) && (pressed != KeyCode.Space) && (pressed != KeyCode.Tab))
                            if ((pressed != KeyCode.Return) && (pressed != KeyCode.Escape) && (pressed != KeyCode.Tab))
                            {
                                //Check for duplicates to appease QAhole ppl
                                isDup = CheckForDuplicates(true, pressed.ToString(), key, ref conflict);
                                switch (key)
                                {
                                    case ActionKey.ARROW_UP:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].upArrow);
                                        }
                                        oldKey=GameManager.currentControls.controls[(int)playerId].upArrow.ToString().ToUpper();    //Get string of     old key
                                        newKey = pressed.ToString().ToUpper();                                                      //                  ~new key
                                        GameManager.currentControls.controls[(int)playerId].upArrow = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);                                                   //Reset oldKey's    ~highlight
                                        //kbdRef.instance.ToggleKey(newKey, true);                                                    //~ set newKey's    ~
                                        break;
                                    case ActionKey.ARROW_DOWN:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].downArrow);
                                        }
                                        //Ditto for all other keys
                                        oldKey = GameManager.currentControls.controls[(int)playerId].downArrow.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].downArrow = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;
                                    case ActionKey.ARROW_LEFT:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].leftArrow);
                                        }
                                        oldKey = GameManager.currentControls.controls[(int)playerId].leftArrow.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].leftArrow = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;
                                    case ActionKey.ARROW_RIGHT:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].rightArrow);
                                        }
                                        oldKey = GameManager.currentControls.controls[(int)playerId].rightArrow.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].rightArrow = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;
                                    //!@
                                    case ActionKey.ARROW_LEFT2:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].leftArrow2);
                                        }
                                        oldKey = GameManager.currentControls.controls[(int)playerId].leftArrow2.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].leftArrow2 = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;
                                    //!@
                                    case ActionKey.ARROW_RIGHT2:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].rightArrow2);
                                        }
                                        oldKey = GameManager.currentControls.controls[(int)playerId].rightArrow2.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].rightArrow2 = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;
                                    case ActionKey.A_KEY:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].aKey);
                                        }
                                        oldKey = GameManager.currentControls.controls[(int)playerId].aKey.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].aKey = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;
                                    case ActionKey.B_KEY:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].bKey);
                                        }                                        
                                        oldKey = GameManager.currentControls.controls[(int)playerId].bKey.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].bKey = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;
                                    case ActionKey.X_KEY:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].xKey);
                                        }                                        
                                        oldKey = GameManager.currentControls.controls[(int)playerId].xKey.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].xKey = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;
                                    case ActionKey.Y_KEY:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].yKey);
                                        }                                        
                                        oldKey = GameManager.currentControls.controls[(int)playerId].yKey.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].yKey = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;

                                    //!@
                                    case ActionKey.L3_KEY:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].L3Key);
                                        }
                                        oldKey = GameManager.currentControls.controls[(int)playerId].L3Key.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].L3Key = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;

                                    //!@
                                    case ActionKey.LB_KEY:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].LBKey);
                                        }
                                        oldKey = GameManager.currentControls.controls[(int)playerId].LBKey.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].LBKey = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;

                                    //!@
                                    case ActionKey.RB_KEY:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].RBKey);
                                        }
                                        oldKey = GameManager.currentControls.controls[(int)playerId].RBKey.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].RBKey = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;

                                    case ActionKey.START_KEY:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].startKey);
                                        }
                                        oldKey = GameManager.currentControls.controls[(int)playerId].startKey.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].startKey = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;
                                    case ActionKey.BACK_KEY:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].backKey);
                                        }
                                        oldKey = GameManager.currentControls.controls[(int)playerId].backKey.ToString().ToUpper();
                                        newKey = pressed.ToString().ToUpper();
                                        GameManager.currentControls.controls[(int)playerId].backKey = pressed;
                                        //kbdRef.instance.ToggleKey(oldKey, false);
                                        //kbdRef.instance.ToggleKey(newKey, true);
                                        break;
                                }

                                //Attempt converting the pressed key string to an arrow glyph
                                t = ArrowGlyphs(pressed.ToString());
                                if (t != ' ')
                                {
                                    //Converted to arrow glyph, set the text appropriately
                                    uiText.text = t.ToString() + "-";
                                }
                                else
                                {
                                    //Wasn't convertible to an arrow glyph
                                    uiText.text = pressed.ToString() + "-";
                                }
                                isWaitingForInput = false;
                                onChanged.Invoke();             //Run change event
                            }
                            return;
                            break;
                    }
                }
            }
                else
                {
                    //Handle gamepad

                    InputDevice dev = ControlsHandler.instance.inputDevices[(int)playerId]; //Get input device
                    //If input device DNE (due to disconnecting it, set input to false and get outta here
                    if (dev==null)
                    {
                        Debug.Log("Device was disconnected; halt mapping");
                        isWaitingForInput = false;
                        onCanceled.Invoke();
                        return;
                    }

                    //!@                    
                    InputControlType btn = dev.AnyButton.Target;    //Get the button's type

                    //Handle LStick button, LBumper, and RBumper keys specially because they are handled weirdly for InControl
                    bool LStick = dev.LeftStickButton.IsPressed;    //Is LStick button pressed?
                    bool LBumper = dev.LeftBumper.IsPressed;        //Is LBumper button pressed?
                    bool RBumper = dev.RightBumper.IsPressed;       //Is RBumper button pressed?

                    //Set the button type correctly based on if LStick, and L/RBumper were pressed
                    if (LStick)
                    {
                        btn = dev.LeftStickButton.Target;
                    }
                    else if (LBumper)
                    {
                        btn = dev.LeftBumper.Target;
                    }
                    else if (RBumper)
                    {
                        btn = dev.RightBumper.Target;
                    }

                    if (btn != InputControlType.None)
                    {
                        //Log the button name
                        Debug.Log("Button: " + btn.ToString());
                    }

                    //If esc was pressed, cancel mapping and restore old button name
                    KeyCode pressed = Event.current.keyCode;
                    if (pressed == KeyCode.Escape)
                    {                        
                        uiText.text = prevText;
                        //t = ArrowGlyphs(uiText.text);   //Try converting a key string to an arrow glyph
                        //if (t != ' ')
                        //{
                        //    //If it succeeded, set text to new glyph instead
                        //    uiText.text = t.ToString();
                        //}
                        onCanceled.Invoke();
                        return;
                    }

                    if (btn != InputControlType.None)
                    {
                        isDup = CheckForDuplicates(false, btn.ToString(), key, ref conflict);

                                switch (key)
                                {
                                    case ActionKey.A_BTN:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].aBtn);
                                        }
                                        GameManager.currentControls.controls[(int)playerId].aBtn = btn;
                                        break;
                                    case ActionKey.B_BTN:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].bBtn);
                                        }
                                        GameManager.currentControls.controls[(int)playerId].bBtn = btn;
                                        break;
                                    case ActionKey.X_BTN:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].xBtn);
                                        }
                                        GameManager.currentControls.controls[(int)playerId].xBtn = btn;
                                        break;
                                    case ActionKey.Y_BTN:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].yBtn);
                                        }
                                        GameManager.currentControls.controls[(int)playerId].yBtn = btn;
                                        break;
                                    //!@
                                    case ActionKey.L3_BTN:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].L3Btn);
                                        }
                                        GameManager.currentControls.controls[(int)playerId].L3Btn= btn;
                                        break;
                                    //!@
                                    case ActionKey.LB_BTN:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].LBBtn);
                                        }
                                        GameManager.currentControls.controls[(int)playerId].LBBtn = btn;
                                        break;
                                    //!@
                                    case ActionKey.RB_BTN:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].RBBtn);
                                        }
                                        GameManager.currentControls.controls[(int)playerId].RBBtn = btn;
                                        break;
                                    case ActionKey.START_BTN:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].startBtn);
                                        }
                                        GameManager.currentControls.controls[(int)playerId].startBtn = btn;
                                        break;
                                    case ActionKey.BACK_BTN:
                                        if (isDup == true)
                                        {
                                            Assign(conflict, GameManager.currentControls.controls[(int)playerId].backBtn);
                                        }
                                        GameManager.currentControls.controls[(int)playerId].backBtn = btn;
                                        break;
                                }
                                uiText.text = btn.ToString()+"-";
                                isWaitingForInput = false;
                                onChanged.Invoke();
                        }
                    }
        }
    }

    /// <summary>
    /// A routine to check for duplicate keys.
    /// </summary>
    /// <param name="isKbd">Is this to check against kbd buttons?</param>
    /// <param name="newKey">String of new key button name</param>
    /// <param name="currentKey">Current Action key being processed</param>
    /// <returns>Were there dups? (Bln)</returns>
    private bool CheckForDuplicates(bool isKbd, string newKey, ActionKey currentKey, ref ActionKey conflictingKey)
    {
        bool foundDuplicate=false;
        string key = "";       
        ActionKey i = 0, a = 0, b = 0;

        if (isKbd == false)
        {
            a = ActionKey.A_BTN;
            b = ActionKey.BACK_BTN;
            for (i = a; i <= b; i++)
            {
                //Skip checking against current key (duh)
                if (i == currentKey)
                {
                    continue;
                }

                switch (i)
                {
                    case ActionKey.A_BTN:
                        key = GameManager.currentControls.controls[(int)playerId].aBtn.ToString();
                        break;
                    case ActionKey.B_BTN:
                        key = GameManager.currentControls.controls[(int)playerId].bBtn.ToString();
                        break;
                    case ActionKey.X_BTN:
                        key = GameManager.currentControls.controls[(int)playerId].xBtn.ToString();
                        break;
                    case ActionKey.Y_BTN:
                        key = GameManager.currentControls.controls[(int)playerId].yBtn.ToString();
                        break;
                    //!@
                    case ActionKey.L3_BTN:
                        key = GameManager.currentControls.controls[(int)playerId].L3Btn.ToString();
                        break;
                    //!@
                    case ActionKey.LB_BTN:
                        key = GameManager.currentControls.controls[(int)playerId].LBBtn.ToString();
                        break;
                    case ActionKey.RB_BTN:
                        key = GameManager.currentControls.controls[(int)playerId].RBBtn.ToString();
                        break;
                    case ActionKey.START_BTN:
                        key = GameManager.currentControls.controls[(int)playerId].startBtn.ToString();
                        break;
                    case ActionKey.BACK_BTN:
                        key = GameManager.currentControls.controls[(int)playerId].backBtn.ToString();
                        break;
                }

                if (newKey == key)
                {
                    //A duplicate was found; you fail it
                    foundDuplicate = true;
                    conflictingKey = i;
                    break;
                }
            }
        }
        else
        {
            a=ActionKey.ARROW_UP;
            b=ActionKey.BACK_KEY;
            for (i = a; i <= b; i++)
            {
                //Skip checking against current key (duh)
                if (i == currentKey)
                {
                    continue;
                }

                switch (i)
                {
                    case ActionKey.ARROW_UP:
                        key = GameManager.currentControls.controls[(int)playerId].upArrow.ToString();
                        break;
                    case ActionKey.ARROW_DOWN:
                        key = GameManager.currentControls.controls[(int)playerId].downArrow.ToString();
                        break;
                    case ActionKey.ARROW_LEFT:
                        key = GameManager.currentControls.controls[(int)playerId].leftArrow.ToString(); 
                        break;
                    case ActionKey.ARROW_RIGHT:
                        key = GameManager.currentControls.controls[(int)playerId].rightArrow.ToString();
                        break;
                    //!@
                    case ActionKey.ARROW_LEFT2:
                        key = GameManager.currentControls.controls[(int)playerId].leftArrow2.ToString();
                        break;
                    //!@
                    case ActionKey.ARROW_RIGHT2:
                        key = GameManager.currentControls.controls[(int)playerId].rightArrow2.ToString();
                        break;
                    case ActionKey.A_KEY:
                        key = GameManager.currentControls.controls[(int)playerId].aKey.ToString();
                        break;
                    case ActionKey.B_KEY:
                        key = GameManager.currentControls.controls[(int)playerId].bKey.ToString();
                        break;
                    case ActionKey.X_KEY:
                        key = GameManager.currentControls.controls[(int)playerId].xKey.ToString();
                        break;
                    case ActionKey.Y_KEY:
                        key = GameManager.currentControls.controls[(int)playerId].yKey.ToString();
                        break;
                    //!@
                    case ActionKey.L3_KEY:
                        key = GameManager.currentControls.controls[(int)playerId].L3Key.ToString();
                        break;
                    //!@
                    case ActionKey.LB_KEY:
                        key = GameManager.currentControls.controls[(int)playerId].LBKey.ToString();
                        break;
                    //!@
                    case ActionKey.RB_KEY:
                        key = GameManager.currentControls.controls[(int)playerId].RBKey.ToString();
                        break;
                    case ActionKey.START_KEY:
                        key = GameManager.currentControls.controls[(int)playerId].startKey.ToString();
                        break;
                    case ActionKey.BACK_KEY:
                        key = GameManager.currentControls.controls[(int)playerId].backKey.ToString();
                        break;
                }

                if (newKey == key)
                {
                    //A duplicate was found; you fail it
                    foundDuplicate = true;
                    conflictingKey = i;
                    break;
                }
            }
        }
        return foundDuplicate;
    }

    /// <summary>
    /// Assigns the currently pressed key for this Key type
    /// </summary>
    public void Assign()
    {
        char t = ' ';
        string k="";
        switch (key)
        {
            case ActionKey.ARROW_UP:
                k = GameManager.currentControls.controls[(int)playerId].upArrow.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);

                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k+"-";                
                break;
            case ActionKey.ARROW_DOWN:
                k = GameManager.currentControls.controls[(int)playerId].downArrow.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k+"-";
                break;
            case ActionKey.ARROW_LEFT:
                k = GameManager.currentControls.controls[(int)playerId].leftArrow.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k+"-";                
                break;
            case ActionKey.ARROW_RIGHT:
                k = GameManager.currentControls.controls[(int)playerId].rightArrow.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k+"-";
                break;
            //!@
            case ActionKey.ARROW_LEFT2:
                k = GameManager.currentControls.controls[(int)playerId].leftArrow2.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k + "-";
                break;
            //!@
            case ActionKey.ARROW_RIGHT2:
                k = GameManager.currentControls.controls[(int)playerId].rightArrow2.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k + "-";
                break;
            case ActionKey.A_KEY:
                k = GameManager.currentControls.controls[(int)playerId].aKey.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k + "-";
                break;
            case ActionKey.B_KEY:
                k = GameManager.currentControls.controls[(int)playerId].bKey.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k + "-";
                break;
            case ActionKey.X_KEY:
                k = GameManager.currentControls.controls[(int)playerId].xKey.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k + "-";
                break;
            case ActionKey.Y_KEY:
                k = GameManager.currentControls.controls[(int)playerId].yKey.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k + "-";
                break;
            //!@
            case ActionKey.L3_KEY:
                k = GameManager.currentControls.controls[(int)playerId].L3Key.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k + "-";
                break;
            //!@
            case ActionKey.LB_KEY:
                k = GameManager.currentControls.controls[(int)playerId].LBKey.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k + "-";
                break;
            //!@
            case ActionKey.RB_KEY:
                k = GameManager.currentControls.controls[(int)playerId].RBKey.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k + "-";
                break;
            case ActionKey.START_KEY:
                k = GameManager.currentControls.controls[(int)playerId].startKey.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k + "-";
                break;
            case ActionKey.BACK_KEY:
                k = GameManager.currentControls.controls[(int)playerId].backKey.ToString().ToUpper();
                //kbdRef.instance.ToggleKey(k, true);
                t = ArrowGlyphs(k);
                if (t != ' ')
                {
                    k = t.ToString();
                }
                uiText.text = k + "-";
                break;

            case ActionKey.JOYSTICK_1:
                onJoystickIDLoaded.Invoke(GameManager.currentControls.controls[(int)playerId].joystickId);
                break;
            case ActionKey.JOYSTICK_TYPE:
                onJoystickTypeLoaded.Invoke(GameManager.currentControls.controls[(int)playerId].joystickType);
                //onJoystickTypeLoaded.Invoke(GameManager.currentControls.controls[1].joystickType);
                break;

            case ActionKey.A_BTN:
                uiText.text = GameManager.currentControls.controls[(int)playerId].aBtn.ToString() + "-";
                break;
            case ActionKey.B_BTN:
                uiText.text = GameManager.currentControls.controls[(int)playerId].bBtn.ToString() + "-";
                break;
            case ActionKey.X_BTN:
                uiText.text = GameManager.currentControls.controls[(int)playerId].xBtn.ToString() + "-";
                break;
            case ActionKey.Y_BTN:
                uiText.text = GameManager.currentControls.controls[(int)playerId].yBtn.ToString() + "-";
                break;
            //!@
            case ActionKey.L3_BTN:
                uiText.text = GameManager.currentControls.controls[(int)playerId].L3Btn.ToString() + "-";
                break;
            //!@
            case ActionKey.LB_BTN:
                uiText.text = GameManager.currentControls.controls[(int)playerId].LBBtn.ToString() + "-";
                break;
            //!@
            case ActionKey.RB_BTN:
                uiText.text = GameManager.currentControls.controls[(int)playerId].RBBtn.ToString() + "-";
                break;
            case ActionKey.START_BTN:
                uiText.text = GameManager.currentControls.controls[(int)playerId].startBtn.ToString() + "-";
                break;
            case ActionKey.BACK_BTN:
                uiText.text = GameManager.currentControls.controls[(int)playerId].backBtn.ToString() + "-";
                break;
        }
    }

    /// <summary>
    /// Assign key by Actionkey type and KeyCode params (for kbd)
    /// </summary>
    /// <param name="k">Actionkey to handle</param>
    /// <param name="newKc">The new keyCode</param>
    public void Assign(ActionKey k, KeyCode newKc)
    {
        Text t;
        char T;
        string K;

        switch (k)
        {
            case ActionKey.ARROW_UP:
                GameManager.currentControls.controls[(int)playerId].upArrow=newKc;
                t = FindText(k);

                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            case ActionKey.ARROW_DOWN:
                GameManager.currentControls.controls[(int)playerId].downArrow = newKc;
                t = FindText(k);
                
                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            case ActionKey.ARROW_LEFT:
                GameManager.currentControls.controls[(int)playerId].leftArrow = newKc;
                t = FindText(k);
                
                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            case ActionKey.ARROW_RIGHT:
                GameManager.currentControls.controls[(int)playerId].rightArrow=newKc;
                t = FindText(k);
                
                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            //!@
            case ActionKey.ARROW_LEFT2:
                GameManager.currentControls.controls[(int)playerId].leftArrow2 = newKc;
                t = FindText(k);

                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            //!@
            case ActionKey.ARROW_RIGHT2:
                GameManager.currentControls.controls[(int)playerId].rightArrow2 = newKc;
                t = FindText(k);

                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            case ActionKey.A_KEY:
                GameManager.currentControls.controls[(int)playerId].aKey = newKc;
                t = FindText(k);
                
                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            case ActionKey.B_KEY:
                GameManager.currentControls.controls[(int)playerId].bKey=newKc;
                t = FindText(k);
                
                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            case ActionKey.X_KEY:
                GameManager.currentControls.controls[(int)playerId].xKey=newKc;
                t = FindText(k);
                
                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            case ActionKey.Y_KEY:
                GameManager.currentControls.controls[(int)playerId].yKey=newKc;
                t = FindText(k);
                
                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            //!@
            case ActionKey.L3_KEY:
                GameManager.currentControls.controls[(int)playerId].L3Key = newKc;
                t = FindText(k);

                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            //!@
            case ActionKey.LB_KEY:
                GameManager.currentControls.controls[(int)playerId].LBKey = newKc;
                t = FindText(k);

                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            //!@
            case ActionKey.RB_KEY:
                GameManager.currentControls.controls[(int)playerId].RBKey = newKc;
                t = FindText(k);

                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            case ActionKey.START_KEY:
                GameManager.currentControls.controls[(int)playerId].startKey= newKc;
                t = FindText(k);
                
                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
            case ActionKey.BACK_KEY:
                GameManager.currentControls.controls[(int)playerId].backKey = newKc;
                t = FindText(k);
                
                K = newKc.ToString();
                T = ArrowGlyphs(K);
                if (T != ' ')
                {
                    K = T.ToString();
                }
                t.text = K + "-";
                break;
        }
    }

    /// <summary>
    /// Assign key by Actionkey type and InputControlType (for gamepad)
    /// </summary>
    /// <param name="k">Action key to handle</param>
    /// <param name="newKc2">The new InputControlType</param>
    public void Assign(ActionKey k, InputControlType newKc2)
    {
        Text t;
        switch (k)
        {            
            case ActionKey.A_BTN:
                GameManager.currentControls.controls[(int)playerId].aBtn = newKc2;
                t = FindText(k);
                t.text = newKc2.ToString() + "-";
                break;
            case ActionKey.B_BTN:
                GameManager.currentControls.controls[(int)playerId].bBtn = newKc2;
                t = FindText(k);
                t.text = newKc2.ToString() + "-";
                break;
            case ActionKey.X_BTN:
                GameManager.currentControls.controls[(int)playerId].xBtn = newKc2;
                t = FindText(k);
                t.text = newKc2.ToString() + "-";
                break;
            case ActionKey.Y_BTN:
                GameManager.currentControls.controls[(int)playerId].yBtn = newKc2;
                t = FindText(k);
                t.text = newKc2.ToString() + "-";
                break;
            //!@
            case ActionKey.L3_BTN:
                GameManager.currentControls.controls[(int)playerId].L3Btn= newKc2;
                t = FindText(k);
                t.text = newKc2.ToString() + "-";
                break;
            //!@
            case ActionKey.LB_BTN:
                GameManager.currentControls.controls[(int)playerId].LBBtn = newKc2;
                t = FindText(k);
                t.text = newKc2.ToString() + "-";
                break;
            //!@
            case ActionKey.RB_BTN:
                GameManager.currentControls.controls[(int)playerId].RBBtn = newKc2;
                t = FindText(k);
                t.text = newKc2.ToString() + "-";
                break;
            case ActionKey.START_BTN:
                GameManager.currentControls.controls[(int)playerId].startBtn = newKc2;
                t = FindText(k);
                t.text = newKc2.ToString() + "-";
                break;
            case ActionKey.BACK_BTN:
                GameManager.currentControls.controls[(int)playerId].backBtn = newKc2;
                t = FindText(k);
                t.text = newKc2.ToString() + "-";
                break;
        }
    }

    /// <summary>
    /// Returns the Text element on the gameobject holding the specified ActionKey
    /// </summary>
    /// <param name="k">ActionKey to find</param>
    /// <returns>Text element</returns>
    private Text FindText(ActionKey k)
    {
        Text t = null;
        ControlsSwitch[] CS = GameObject.FindObjectsOfType<ControlsSwitch>();
        foreach (ControlsSwitch c in CS)
        {            
            //Only process text belong to the current playerID
            if (c.playerId==playerId)
            {
                //Found the ControlsSwitch for this key! Get it's text
                if (c.key == k)
                {
                    t = c.uiText;
                    break;
                }
            }
        }
        return t;
    }

    //!@ Dead Coffee Code
    /// <summary>
    /// Forces joystick to be for player 1. Called by Handle2P function, in order to change
    /// </summary>
    //public void InitJoystick1()
    //{
    //    int ind = (int)(ControlledBy.PLAYER1);
    //    onJoystickIDLoaded.Invoke(GameManager.currentControls.controls[ind].joystickId);
    //    GameManager.instance.SaveControls();
    //}

    /// <summary>
    /// Tries to convert a key text string to an arrow glpyh
    /// </summary>
    /// <param name="K">key text string</param>
    /// <returns>' ' if not convertible; else an arrow glyph char</returns>
    public char ArrowGlyphs(string K)
    {
        //Char to output
        char ch=' ';    

        //Key text strings
        string[] keys=
        {
            "UpArrow","DownArrow","LeftArrow","RightArrow"
        };
        
        //Their appropriate glyph ASCII chars
        //!@ These glyphs need hacked into the BankGothic-light font using FontForge!
        char[] glyphs=
        {
            '^',        //Up
            '\\',       //Down
            '<',        //Left
            '>'         //Right
        };

        byte i=0;   //Generic iterator
        //Iterate through all keys
        foreach (string k in keys)
        {
            //If the input key text string matches one in list,
            //set output char to the appropriate arrow glyph
            if (K.ToUpper()== k.ToUpper())
            {
                ch = glyphs[i];
                break;
            }
            //Increment index
            i++;    
        }
        return ch;
    }

    /// <summary>
    /// Changes Controls
    /// </summary>
    public void ChangeControls()
    {
        isWaitingForInput = true;   //We are waitin for input
        prevText = uiText.text;     //Set prev text to current
        uiText.text = "";           //Set current tex to nothing while waiting for remap
    }

    //!@Dead Coffee Code
    /// <summary>
    /// Toggles between Player 1 and Player 2 for this controlsSwitch's controlledBy param
    /// </summary>
    //public void TogglePlayer(int id)
    //{
    //    if (id==0)
    //    {
    //        playerId = ControlledBy.PLAYER2;
    //    }
    //    else
    //    {
    //        playerId = ControlledBy.PLAYER1;
    //    }
    //}
}

/// <summary>
/// Enums for each key/btn type
/// </summary>
public enum ActionKey
{
    JOYSTICK_1,     //Gamepad 1 ID
    //JOYSTICK_2,

    //Kbd keys
    ARROW_UP,
    ARROW_DOWN,
    ARROW_LEFT,
    ARROW_RIGHT,
    ARROW_LEFT2,    //!@
    ARROW_RIGHT2,   //!@
    A_KEY,
    B_KEY,
    X_KEY,
    Y_KEY,
    L3_KEY,         //!@
    LB_KEY,         //!@
    RB_KEY,         //!@
    START_KEY,
    BACK_KEY,
    JOYSTICK_TYPE,  //Joystick type

    //Joypad btns (other type)
    A_BTN,
    B_BTN,
    X_BTN,
    Y_BTN,    
    L3_BTN,
    LB_BTN,
    RB_BTN,
    START_BTN,
    BACK_BTN,
    */
}