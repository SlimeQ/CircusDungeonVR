// Rewired Constants
// This list was generated on 10/13/2019 2:43:34 AM
// The list applies to only the Rewired Input Manager from which it was generated.
// If you use a different Rewired Input Manager, you will have to generate a new list.
// If you make changes to the exported items in the Rewired Input Manager, you will
// need to regenerate this list.

namespace RewiredConsts
{
    using UnityEngine;
    public static partial class Action
    {
        // Default
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Yaw")]
        public const int Yaw = 0;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Pitch")]
        public const int Pitch = 1;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Roll")]
        public const int Roll = 2;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "BRoll-")]
        public const int BRollM = 6;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "BRoll+")]
        public const int BRollP = 7;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Throttle")]
        public const int Throttle = 3;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Throttle_Brake")]
        public const int ThrottleM = 5;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Throttle_Accel")]
        public const int ThrottleP = 4;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Shoot")]
        public const int Shoot = 8;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Special")]
        public const int Special = 9;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "UTurn")]
        public const int UTurn = 10;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Back")]
        public const int Back = 12;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Start")]
        public const int Start = 11;

        //Constant Rewired action names
        public static readonly Vector2 deadNorm = new Vector2(RW_deadMin_norm, RW_deadMax_norm);        //Deadzone for normal stuff (+-.1f)
        public static readonly Vector2 deadThrot = new Vector2(RW_deadMin_throt, RW_deadMax_throt);     //Deadzone for flightstick throttle axis (.25f-.75f)
        //Min/Max of normal deadzone
        public const float RW_deadMin_norm = -.1f;
        public const float RW_deadMax_norm = .1f;
        //Min/Max of throttle deadzone
        public const float RW_deadMin_throt = .25f;
        public const float RW_deadMax_throt = .75f;

        /// <summary>
        /// Types of controllerMap actions for the game (simple)
        /// </summary>
        public enum Actions
        {
            YAW,
            PITCH,
            ROLL,
            THROTTLE,
            THROTTLE_ACCEL,
            THROTTLE_BRAKE,
            BROLL_M,
            BROLL_P,
            SHOOT,
            SPECIAL,
            UTURN,
            START,
            BACK,
            MAX = BACK
        }

        /// <summary>
        /// Types of controllerMap actions for the game (including +/- axises)
        /// </summary>
        public enum ActionsFull
        {
            YAW_M,
            YAW_P,
            PITCH_M,
            PITCH_P,
            ROLL_M,
            ROLL_P,
            THROTTLE_M,
            THROTTLE_P,
            THROTTLE_ACCEL,
            THROTTLE_BRAKE,
            BROLL_M,
            BROLL_P,
            SHOOT,
            SPECIAL,
            UTURN,
            START,
            BACK,
            MAX = BACK
        }
    }
    public static partial class Category {
        public const int Default = 0;
    }
    public static partial class Layout {
        public static partial class Joystick
        {
            //!@
            public const int Default = 0;

            /// <summary>
            /// Types of gamepads supported by StarEagle
            /// </summary>
            public enum Types
            {
                UNKNOWN,        //Unknown controller
                KBD,            //Keyboard
                MOUSE,          //Mouse
                CUSTOM,         //Custom

                CTRLR,          //Controller
                GPAD,           //Other gamepad (non dual-analog)
                T_DUAL_ANALOG,  //Dual analog temlate

                XINPUT,         //XInput
                X360,           //XBox 360
                XBONE,          //XBox One                

                T_HOTAS,        //HOTAS Flightstick template
                ST290P,         //Saitek ST290 Pro flighstick
                MAX = ST290P    //MAX
            }
        }
        public static partial class Keyboard {
            public const int Default = 0;
        }
        public static partial class Mouse {
            public const int Default = 0;
        }
        public static partial class CustomController {
            public const int Default = 0;
        }
    }
    public static partial class Player {
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "System")]
        public const int System = 9999999;
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "Player0")]
        public const int Player0 = 0;
    }
    public static partial class CustomController {
    }
}
