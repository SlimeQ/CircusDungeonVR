using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Rewired;

/// <summary>
/// Handles controls for the players, for in-game actions
/// </summary>
public class PlayerControls : MonoBehaviour
{
    //public static float specialActionResetTime = .1f; //!@ Dead Coffee code, timer used to delay spamming of special attack
    public List<MenuScreen> relativeScreens = new List<MenuScreen>();                   //Relative MenuScreen (if any)
    //public bool controlledByKeyboard = true;          //Should player be controlled by Keyboard?
    public const byte MAXKEYS = 15;                     //!@Max amount of keys used by game
    
    public Action onA;          //Attack pressed
    public Action onAHeld;      //!@ Attack held
    public Action onAReleased;  //!@ Attack released
    public Action onB;          //Special attack pressed

    public Action onX;          //Brake pressed
    public Action onXPressed;   //Brake held down
    public Action onXReleased;  //Brake released 

    public Action onY;

    public Action onL3;         //!@ Acceleration 
    public Action onL3Pressed;
    public Action onL3Released;

    public Action onLB;         //!@ Hard/BRoll- 
    public Action onLBReleased;
    public Action onRB;         //!@ Hard/BRoll+
    public Action onRBReleased;

    public Action onStart;      //Start btn
    public Action onBack;       //Back button
    public Action onSpecial;    //!@ Dead Coffee Code. If special timer delay is still running, do onA, else special

    public Player player;
    public Action<Vector2> PitchYawAxis;    //X/Z rotation
    public Action<Vector2> RollAxis;        //Y rotation

    //keyboard values
    /*
    private KeyCode upKeyValue;
    private KeyCode downKeyValue;
    private KeyCode leftKeyValue;
    private KeyCode rightKeyValue;
    private KeyCode left2KeyValue;   //!@ RStick X- for roll-
    private KeyCode right2KeyValue;  //!@ RStick X+ for roll+
    private KeyCode aKeyValue;
    private KeyCode bKeyValue;
    private KeyCode xKeyValue;
    private KeyCode yKeyValue;
    private KeyCode L3KeyValue;     //!@
    private KeyCode LBKeyValue;     //!@
    private KeyCode RBKeyValue;     //!@
    private KeyCode startKeyValue;
    private KeyCode backKeyValue;

    //3rd-party gamepad values    
    private InControl.InputControlType aBtnValue;
    private InControl.InputControlType bBtnValue;
    private InControl.InputControlType xBtnValue;
    private InControl.InputControlType yBtnValue;
    private InControl.InputControlType L3BtnValue;     //!@
    private InControl.InputControlType LBBtnValue;     //!@
    private InControl.InputControlType RBBtnValue;     //!@
    private InControl.InputControlType startBtnValue;
    private InControl.InputControlType backBtnValue;
    //--------------
    */

    private ControlledBy controlledBy;
    private float lastThrotarea = 0f;                   //!@ Previous deadzone are of the throttle axis for flightstick support
    private Vector2 LStickAxis = new Vector2();         //DirStick
    private Vector2 RStickAxis = new Vector2();         //CStick
    //private float specialTimer;                         //!@ Dead Coffee Code. Delay timer for spamming special attack

    void Update()
    {
        //Don't enable player controller outside of particular relative screens
        //This prevents say shooting say in pause screen etc
        byte i = 0;         //Generic iterator
        bool good = false;  //Any good screens?

        if (relativeScreens != null)
        {
            //If array populated

            //Iterate through all screens in relative screens
            foreach (MenuScreen s in relativeScreens)
            {
                //good = good | (if currnscreen = this relative screen);
                good |= (MainMenuController.instance.currentScreen == relativeScreens[i]);
                i++;
            }
        }
        else
        {
            //If array empty, we are good
            good = true;
        }

        //if no good, return
        if (!good)
        {
            return;
        }

        LStickAxis *= .8f;
        RStickAxis *= .8f;  //!@

        //Decrement the special timer if time still remains
        //!@Dead Coffee Code
        //if (specialTimer > 0f)
            //specialTimer -= Time.deltaTime;

        /*
        if (controlledByKeyboard)
        {
            //Handle kbd controls
            if (Input.GetKeyDown(backKeyValue))
                StartCoroutine(InvokeEvent(onBack));

            if (Input.GetKeyDown(startKeyValue))
                StartCoroutine(InvokeEvent(onStart));

            if (Input.GetKeyDown(aKeyValue))
            {
                //!@Dead coffee code
                //if (specialTimer > 0f)
                    //StartCoroutine(InvokeEvent(onSpecial));
                //else
                    StartCoroutine(InvokeEvent(onA));
            }

            if (Input.GetKeyDown(bKeyValue))
            {
                StartCoroutine(InvokeEvent(onB));
                //specialTimer = specialActionResetTime;
            }

            if (Input.GetKeyUp(xKeyValue))
            {
                StartCoroutine(InvokeEvent(onXReleased));
            }
            else if (Input.GetKeyDown(xKeyValue))
            {
                StartCoroutine(InvokeEvent(onX));
                StartCoroutine(InvokeEvent(onXPressed));
            }

            if (Input.GetKeyDown(yKeyValue))
            {
                StartCoroutine(InvokeEvent(onY));
            }

            if (Input.GetKeyUp(L3KeyValue))
            {
                StartCoroutine(InvokeEvent(onL3Released));
            }
            else if (Input.GetKeyDown(L3KeyValue))
            {
                StartCoroutine(InvokeEvent(onL3));
                StartCoroutine(InvokeEvent(onL3Pressed));
            }

            if (Input.GetKeyDown(LBKeyValue))
            {
                StartCoroutine(InvokeEvent(onLB));                
                goto skipRB;
            }
            else if (Input.GetKeyUp(LBKeyValue))
            {
                StartCoroutine(InvokeEvent(onLBReleased));
                goto skipRB;
            }

            if (Input.GetKeyDown(RBKeyValue))
            {
                StartCoroutine(InvokeEvent(onRB));
            }
            else if (Input.GetKeyUp(RBKeyValue))
            {
                StartCoroutine(InvokeEvent(onRBReleased));
            }
        skipRB:

            //Handle X direction on DirStick keys
            //if (Input.GetKey(rightKeyValue) || Input.GetKey(leftKeyValue))
            if (Input.GetKey(rightKeyValue) != Input.GetKey(leftKeyValue))
                LStickAxis.x = Input.GetKey(rightKeyValue) ? 1f : -1f;

            //Handle Y direction on DirStick keys
            //if (Input.GetKey(upKeyValue) || Input.GetKey(downKeyValue))
            if (Input.GetKey(upKeyValue) != (Input.GetKey(downKeyValue)))
                LStickAxis.y = Input.GetKey(upKeyValue) ? 1f : -1f;

            //!@
            //Handle x directon on CStick keys for roll
            if (Input.GetKey(right2KeyValue) != Input.GetKey(left2KeyValue))
                RStickAxis.x = Input.GetKey(right2KeyValue) ? 1f : -1f;
        }
        */

        //Handle gamepad controls
        InvokeEventsFromPlayer((int)controlledBy);

        if(PitchYawAxis != null)
            PitchYawAxis.Invoke(LStickAxis);
        //!@
        if (RollAxis != null)
            RollAxis.Invoke(RStickAxis);
    }

    /// <summary>
    /// Handles gamepad controls
    /// </summary>
    /// <param name="index">ControlledBy player index</param>
    private void InvokeEventsFromPlayer(int index)
    {
        Controller c;

        ActionElementMap throttle = null;
        Vector2 axis;           //Axis value
        Vector2 deadAxis;       //Deadzone for axises (.x = min, .y = max)
        Vector2 deadArea;       //Return result from ControlsSetup.DeadZone function
        int ID = 0;
        //bool yawRoll_swap = false;
        float pitch_invert = 1f;
        bool state = false;     //Generic button press state/bool
        /*
        if (ControlsHandler.instance.inputDevices[index] == null)
        {
            //Debug.Log("Null device detected! Not running controller code...");
            return;
        }

        //Debug.Log("GM_CC_C_Count: " + (GameManager.currentControls.controls.Count-1).ToString());

        if (index > GameManager.currentControls.controls.Count - 1)
        {
            //Debug.Log("Controller index OOB. Not running controller code...");
            return;
        }
        */

        //Type of gamepad
        //int StandX360 = GameManager.currentControls.controls[index].joystickType;
        //if (StandX360 == 0)
        //{
            //X360 controller
            //Handle DirStick X/Y dirs
            
            player = ReInput.players.GetPlayer(index);   //Get the player
            c = player.controllers.GetLastActiveController();   //Get the last active controller
            if(c!=null)
            {
                //!@ Note: For a controller to be considered "inactive" as a return result from p.c.GLAC()
                //All buttons must be up, and axises at deadzone.
                //Due to this, flighstick will only be considered inactive if throttle axis is at deadzone
                //(remember, most flighsticks throttle axis like with ST290 Pro do NOT snap back to deadzone rest
                //Therefore, throttle must be kept at deadzone (no throttle) to be disregarded for say a kbd to have priority!
                //Bug not feature :S

                //Get that controller's dedicated throttle axis (if any)
                throttle = player.controllers.maps.GetFirstAxisMapWithAction(c, RewiredConsts.Action.Throttle, true);
            }

            /*
            yawRoll_swap = GameManager.currentControls.controls[index].yawRoll_Swap;
            bool invert = GameManager.currentControls.controls[index].pitch_invert;
            if (invert)
            {
                pitch_invert = -1f;
            }
            else
            {
                pitch_invert = 1f;
            }

            //if (ControlsHandler.instance.inputDevices[index].LeftStick.State)
             if (throttle != null)
             {
                 if (yawRoll_swap)
                 {
                     ID = RewiredConsts.Action.Roll;
                 }
                 else
                 {
                     ID = RewiredConsts.Action.Yaw;
                 }
             }
             else
             {
            */
                 //!@ Apply Pitch inversion flag from settings
                 pitch_invert = GameManager.currentControls.controls[index].HandlePitchInvert(false, false);
                 ID = RewiredConsts.Action.Yaw;
             //}
            axis = player.GetAxis2D(ID,RewiredConsts.Action.Pitch);   //Get axis2D of yaw and & pitch
            deadAxis = RewiredConsts.Action.deadNorm;                 //DeadAxis area  = +-.1f
            deadArea = ControlsSetup.RW_DeadAxis(axis, deadAxis, 0);  //Get deadzone result for both axises
            //If the result is outside of the deadzone, store axis value into LStick axis
            if (deadArea != Vector2.zero)
            {
                axis.y *= pitch_invert;
                LStickAxis = axis;
            }

            //!@
            //Handle CStick X/Y dirs
            //if (ControlsHandler.instance.inputDevices[index].RightStick.State)
            /*
            if (throttle != null)
            {
                if (yawRoll_swap)
                {
                    ID = RewiredConsts.Action.Yaw;
                }
                else
                {
                    ID = RewiredConsts.Action.Roll;
                }
            }
            else
            {
            */
                ID = RewiredConsts.Action.Roll;
            //}            
            axis = new Vector2(player.GetAxis(ID), 0f);          //Set axis for roll            
            deadAxis = RewiredConsts.Action.deadNorm;                                      //DeadAxis area = +-.1f
            deadArea = ControlsSetup.RW_DeadAxis(axis, deadAxis, -1);               //Get deadzone result for .x axis only
            //If the result is outside of the deadzone, store axis value into RStickAxis
            if (deadArea != Vector2.zero)
                RStickAxis = axis;

            //Handle DPad X/Y dirs
            //if (ControlsHandler.instance.inputDevices[index].DPad.State)
                //LStickAxis.Set(ControlsHandler.instance.inputDevices[index].DPad.X, ControlsHandler.instance.inputDevices[index].DPad.Y);

            //!@
            //Handle back btn
            //bool back = ((ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Back).WasPressed) || (ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.View).WasPressed));
            //bool back = ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Back).WasPressed;
            state = player.GetButtonDown(RewiredConsts.Action.Back);
            if(state)
            {
                StartCoroutine(InvokeEvent(onBack));
            }

            //!@
            //Handle start btn
            //if (ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Start).WasPressed
                //|| ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Menu).WasPressed)
            //if (ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Start).WasPressed)
            if (player.GetButtonDown(RewiredConsts.Action.Start))
                StartCoroutine(InvokeEvent(onStart));

            //Handle A btn
            //if (ControlsHandler.instance.inputDevices[index].Action1.WasPressed)

            int shoot = RewiredConsts.Action.Shoot;  //String name for shoot action
            if (player.GetButtonDown(shoot))
            {
                //If shoot button pressed down, fire onA event
                StartCoroutine(InvokeEvent(onA));
            }
            else if (player.GetButtonUp(shoot))
            {
                //If shoot button released, fire onAReleased event
                StartCoroutine(InvokeEvent(onAReleased));
            }
            else
            {
                //If neither up or down

                //Get amount of time shoot button has been pressed; if > 0f, then fire onAHeld event
                float held = player.GetButtonTimePressed(shoot);
                if (held > 0f)
                {
                    StartCoroutine(InvokeEvent(onAHeld));
                }
            }

            //Handle B btn
            //if (ControlsHandler.instance.inputDevices[index].Action2.WasPressed)
            if (player.GetButtonDown(RewiredConsts.Action.Special))
            {
                StartCoroutine(InvokeEvent(onB));
                //specialTimer = specialActionResetTime;
            }

            //Handle throttle (accel/break)

            //Check if one of the controllers has a dedicated throttle axis (for flightsticks etc)            
            if (throttle != null)
            {
                //If a controller has a throttle axis (flightsticks etc), handle specially

                axis = new Vector2(player.GetAxis(RewiredConsts.Action.Throttle), 0f);  //Set axis for throttle on x axis
                deadAxis = RewiredConsts.Action.deadThrot;                                 //Set deadAxis as special deadThrot area (.25f - .75f)
                deadArea = ControlsSetup.RW_DeadAxis(axis, deadAxis, -1);           //Get the deadArea for the x axis

                if (deadArea.x == -1f)
                {
                    //If the throttle is below the dead zone (brake)

                    if (lastThrotarea != -1f)
                    {
                        //If the previous throttle deadArea was NOT below (brake), Fire onL3Released (release accel button)
                        StartCoroutine(InvokeEvent(onL3Released));
                    }
                    else
                    {
                        //If the previous throttle is below (brake), fire onX and onXPressed event (do brake)
                        StartCoroutine(InvokeEvent(onX));           
                        StartCoroutine(InvokeEvent(onXPressed));
                    }
                }                
                else if (deadArea.x == 1f)
                {
                    //If the throttle is above the dead zone (accel)

                    if (lastThrotarea != 1f)
                    {
                        //If the previous throttle deadArea was NOT above (accel), fire onXReleased (release brake button)
                        StartCoroutine(InvokeEvent(onXReleased));
                    }
                    else
                    {
                        //If the previous throttle is above (accel), fire onL3 and onL3Pressed event (do accel)
                        StartCoroutine(InvokeEvent(onL3));
                        StartCoroutine(InvokeEvent(onL3Pressed));
                    }
                }
                else
                {
                    //If the throttle is within the dead zone (neither brake/accel, normal speed)

                    if (lastThrotarea == -1f)
                    {
                        //If the previous throttle deadArea was below (brake), fire onXReleased (release brake button)
                        StartCoroutine(InvokeEvent(onXReleased));
                    }
                    else if (lastThrotarea == 1f)
                    {
                        //If the previous throttle deadArea was above (accel), fire onL3Released (release accel button)
                        StartCoroutine(InvokeEvent(onL3Released));
                    }
                }
                lastThrotarea = deadArea.x; //Cache current deadArea.x axis for later delta frame comparisons
            }
            else
            {
                //If a controller does NOT have a throttle axis (gamepads, kbd, etc), handle normally

                lastThrotarea = 0f;     //Set lastThrotAreas as the deadzone (not using this var, dummy val)
                //Handle X btn (break)
                //if (ControlsHandler.instance.inputDevices[index].Action3.WasReleased)
                if (player.GetButtonUp(RewiredConsts.Action.ThrottleM))
                {
                    //If break button released, release brake
                    StartCoroutine(InvokeEvent(onXReleased));
                }
                //else if (ControlsHandler.instance.inputDevices[index].Action3.WasPressed)
                else if (player.GetButtonDown(RewiredConsts.Action.ThrottleM))
                {
                    //If break button pressed, fire onX and onXPressed (do brake)
                    StartCoroutine(InvokeEvent(onX));
                    StartCoroutine(InvokeEvent(onXPressed));
                }

                //!@
                //Handle DirStick btn (L3 accel)
                //if (ControlsHandler.instance.inputDevices[index].LeftStickButton.WasReleased)
                if (player.GetButtonUp(RewiredConsts.Action.ThrottleP))
                {
                    //If accel button released, release accel
                    StartCoroutine(InvokeEvent(onL3Released));
                }
                //else if (ControlsHandler.instance.inputDevices[index].LeftStickButton.WasPressed)
                else if (player.GetButtonDown(RewiredConsts.Action.ThrottleP))
                {
                    //If accel button pressed, fire onL3 and onL3Pressed (do accel)
                    StartCoroutine(InvokeEvent(onL3));
                    StartCoroutine(InvokeEvent(onL3Pressed));
                }
            }

            //Handle Y btn
            //if (ControlsHandler.instance.inputDevices[index].Action4.WasPressed)
            if (player.GetButtonDown(RewiredConsts.Action.UTurn))
                StartCoroutine(InvokeEvent(onY));
            

            //!@
            //Handle LB 
            //if (ControlsHandler.instance.inputDevices[index].LeftBumper.WasPressed)
            if (player.GetButtonDown(RewiredConsts.Action.BRollM))
            {
                //if LB button pressed, do onLB
                StartCoroutine(InvokeEvent(onLB));
            }
            //else if (ControlsHandler.instance.inputDevices[index].LeftBumper.WasReleased)
            else if (player.GetButtonUp(RewiredConsts.Action.BRollM))
            {
                //else if LB button release, release onLB
                StartCoroutine(InvokeEvent(onLBReleased));
            }

            //!@
            //Handle RB
            //if (ControlsHandler.instance.inputDevices[index].RightBumper.WasPressed)
            if (player.GetButtonDown(RewiredConsts.Action.BRollP))
            {
                //if RB button pressed, do onRB
                StartCoroutine(InvokeEvent(onRB));
            }
            //else if (ControlsHandler.instance.inputDevices[index].RightBumper.WasReleased)
            else if (player.GetButtonUp(RewiredConsts.Action.BRollP))
            {
                //else if RB button release, release onRB
                StartCoroutine(InvokeEvent(onRBReleased));
            }
        /*
        }        
        else
        {            
            //3rd-party gamepad

            //Handle DirStick
            if ((Mathf.Abs(ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Analog0).Value) != 0f) || (Mathf.Abs(ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Analog1).Value) != 0f))
                LStickAxis.Set(ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Analog0).Value, -(ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Analog1).Value));

            //!@Verify if correct analog IDs for RStick
            //Handle CStick
            if ((Mathf.Abs(ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Analog2).Value) != 0f) || (Mathf.Abs(ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Analog3).Value) != 0f))
                RStickAxis.Set(ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Analog2).Value, -(ControlsHandler.instance.inputDevices[index].GetControl(InputControlType.Analog3).Value));
            
            //Handle back
            if (ControlsHandler.instance.inputDevices[index].GetControl(backBtnValue).WasPressed)
                StartCoroutine(InvokeEvent(onBack));

            //Handle start
            if (ControlsHandler.instance.inputDevices[index].GetControl(startBtnValue).WasPressed)
                StartCoroutine(InvokeEvent(onStart));

            //Handle A btn
            if (ControlsHandler.instance.inputDevices[index].GetControl(aBtnValue).WasPressed)
            {
                //if (specialTimer > 0f)
                //    StartCoroutine(InvokeEvent(onSpecial));
                //else
                    StartCoroutine(InvokeEvent(onA));
            }

            //Handle B btn
            if (ControlsHandler.instance.inputDevices[index].GetControl(bBtnValue).WasPressed)
            {
                StartCoroutine(InvokeEvent(onB));
                //specialTimer = specialActionResetTime;
            }

            //Handle X btn
            if (ControlsHandler.instance.inputDevices[index].GetControl(xBtnValue).WasReleased)
            {
                StartCoroutine(InvokeEvent(onXReleased));
            }
            else if (ControlsHandler.instance.inputDevices[index].GetControl(xBtnValue).WasPressed)
            {
                StartCoroutine(InvokeEvent(onX));
                StartCoroutine(InvokeEvent(onXPressed));
            }
            
            //Y btn
            if (ControlsHandler.instance.inputDevices[index].GetControl(yBtnValue).WasPressed)
                StartCoroutine(InvokeEvent(onY));

            //!@
            //DirStick btn
            if (ControlsHandler.instance.inputDevices[index].GetControl(L3BtnValue).WasReleased)
            {
                StartCoroutine(InvokeEvent(onL3Released));
            }
            else if (ControlsHandler.instance.inputDevices[index].GetControl(L3BtnValue).WasPressed)
            {
                StartCoroutine(InvokeEvent(onL3));
                StartCoroutine(InvokeEvent(onL3Pressed));
            }
                

            //!@
            //LB btn
            if (ControlsHandler.instance.inputDevices[index].GetControl(LBBtnValue).WasPressed)
            {
                StartCoroutine(InvokeEvent(onLB));
            }
            else if (ControlsHandler.instance.inputDevices[index].GetControl(LBBtnValue).WasReleased)
            {
                StartCoroutine(InvokeEvent(onLBReleased));
            }

            //!@
            //RB btn
            if (ControlsHandler.instance.inputDevices[index].GetControl(RBBtnValue).WasPressed)
            {
                StartCoroutine(InvokeEvent(onRB));
            }
            else if (ControlsHandler.instance.inputDevices[index].GetControl(RBBtnValue).WasReleased)
            {
                StartCoroutine(InvokeEvent(onRBReleased));
            }
        }
        */
    }

    /// <summary>
    /// Invokes an event
    /// </summary>
    /// <param name="event">Event</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator InvokeEvent(Action @event)
    {
        yield return new WaitForEndOfFrame();

        if(@event != null)
            @event.Invoke();
    }

    /// <summary>
    /// Sets player 1's default controls for each key type in this script
    /// </summary>
    /// <returns>ControlledBy player</returns>
    //public ControlledBy LoadPlayer1Setup(bool forcePlayer1 = false)
    public ControlledBy LoadPlayer1Setup()
    {
        const byte val = 0; //Player device ID

        //Kbd values
        /*
        upKeyValue = GameManager.currentControls.controls[val].upArrow;
        downKeyValue = GameManager.currentControls.controls[val].downArrow;
        leftKeyValue = GameManager.currentControls.controls[val].leftArrow;
        rightKeyValue = GameManager.currentControls.controls[val].rightArrow;
        left2KeyValue = GameManager.currentControls.controls[val].leftArrow2;    //!@
        right2KeyValue = GameManager.currentControls.controls[val].rightArrow2;  //!@
        
        aKeyValue = GameManager.currentControls.controls[val].aKey;
        bKeyValue = GameManager.currentControls.controls[val].bKey;
        xKeyValue = GameManager.currentControls.controls[val].xKey;
        yKeyValue = GameManager.currentControls.controls[val].yKey;
        L3KeyValue = GameManager.currentControls.controls[val].L3Key;           //!@
        LBKeyValue = GameManager.currentControls.controls[val].LBKey;           //!@
        RBKeyValue = GameManager.currentControls.controls[val].RBKey;           //!@
        backKeyValue = GameManager.currentControls.controls[val].backKey;
        startKeyValue = GameManager.currentControls.controls[val].startKey;

        //3rd-party gamepad btns
        aBtnValue = GameManager.currentControls.controls[val].aBtn;
        bBtnValue = GameManager.currentControls.controls[val].bBtn;
        xBtnValue = GameManager.currentControls.controls[val].xBtn;
        yBtnValue = GameManager.currentControls.controls[val].yBtn;
        L3BtnValue = GameManager.currentControls.controls[val].L3Btn;           //!@
        LBBtnValue = GameManager.currentControls.controls[val].LBBtn;           //!@
        RBBtnValue = GameManager.currentControls.controls[val].RBBtn;           //!@
        startBtnValue = GameManager.currentControls.controls[val].startBtn;
        backBtnValue = GameManager.currentControls.controls[val].backBtn;

		Debug.Log("joystickID: " + GameManager.currentControls.controls[val].joystickId.ToString());
        */

        //if (forcePlayer1 == true)
        //{
            controlledBy=(int)ControlledBy.PLAYER1;
        /*
        }
        else
        {
            controlledBy=(ControlledBy)Enum.GetValues(typeof(ControlledBy)).GetValue(GameManager.currentControls.controls[0].joystickId);
        }
        */
        return controlledBy;
    }

    /// <summary>
    /// Sets player 2's controls
    /// </summary>
    /// <returns>ControlledBy player</returns>
    /*
    public ControlledBy LoadPlayer2Setup()
    {
        const byte val = 1;
        if (GameManager.currentControls.controls[0].joystickId == 1)
        {
            controlledBy = ControlledBy.PLAYER1;
        }
        else
        {
            controlledBy = ControlledBy.PLAYER2;
        }

        upKeyValue = GameManager.currentControls.controls[val].upArrow;
        downKeyValue = GameManager.currentControls.controls[val].downArrow;
        rightKeyValue = GameManager.currentControls.controls[val].rightArrow;
        leftKeyValue = GameManager.currentControls.controls[val].leftArrow;
        aKeyValue = GameManager.currentControls.controls[val].aKey;
        bKeyValue = GameManager.currentControls.controls[val].bKey;
        xKeyValue = GameManager.currentControls.controls[val].xKey;
        yKeyValue = GameManager.currentControls.controls[val].yKey;
        backKeyValue = GameManager.currentControls.controls[val].backKey;
        startKeyValue = GameManager.currentControls.controls[val].startKey;

        aBtnValue = GameManager.currentControls.controls[val].aBtn;
        bBtnValue = GameManager.currentControls.controls[val].bBtn;
        xBtnValue = GameManager.currentControls.controls[val].xBtn;
        yBtnValue = GameManager.currentControls.controls[val].yBtn;
        startBtnValue = GameManager.currentControls.controls[val].startBtn;
        backBtnValue = GameManager.currentControls.controls[val].backBtn;
        return controlledBy;
    }
    */

    /// <summary>
    /// Get who controls this player
    /// </summary>
    /// <returns>ControlledBy</returns>
    public ControlledBy GetCB()
    {
        return controlledBy;
    }

    /// <summary>
    /// Adds/Remove the CutScene MenuScreen from the relative screens
    /// Needed to dis/enable playerControls for certain cutscenes (e.g. boss destruction sequence)
    /// </summary>
    /// <param name="add">Add the CutScene MenuScreen to list?</param>
    /// <returns>Sucessful operation?</returns>
    public bool AddRemove_CutSceneDialog_RelScreen(bool add)
    {
        bool result = false;    //Result of operation

        byte cutIndex = (byte)(MainMenuController.GameMenus.CUTSCENE);      //Standard index of the cutscene index asByte
        MenuScreen cutscene = MainMenuController.instance.menu[cutIndex];   //The cutscene ref in the standard MMC GameUI prefab
        bool exists = relativeScreens.Contains(cutscene);                   //Does cutscene MenuScreen exists in the RelativeScreen List?

        if(cutscene)
        {
            //If the MMC has the cutscene menu

            if (add)
            {
                //If add operation

                if (!exists)
                {
                    //If on CutScene MS exists in relScreens, add it
                    result = true;
                    relativeScreens.Add(cutscene);
                }
                else
                {
                    //If it exists, don't add a duplicate (add op "failed")
                    result = false;
                }
            }
            else
            {
                //if remove operation

                //Remove the cutscene if exists, and report success of op
                result = relativeScreens.Remove(cutscene);
            }
        }
        else
        {
            //If no cutscene found in MMC, report error and failed op result
            Debug.LogError("No cutscene menu found in MMC!");
            result = false;
        }

        //Return the op result
        return result;
    }
}