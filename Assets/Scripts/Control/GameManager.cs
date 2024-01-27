using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Linq;
using UnityEngine;
//using Assets.Scripts.Cam.Effects;

//using Rewired;

/// <summary>
/// Global GameManager singleton. MISSION-CRITICAL!
/// </summary>
public class GameManager : MonoBehaviour
{
    //Singleton instance
    public static GameManager instance;
    public bool DEBUG = true;                      //Debug mode

    //Audio stuff
    public Audio audio;                             //Global Audio script
    public UnityEngine.Audio.AudioMixer AM_BGM;     //BGM Audio Mixer
    public UnityEngine.Audio.AudioMixer AM_SFX;     //SFX Audio Mixer

    #region GameState_enums
    /// <summary>
    /// Game States
    /// </summary>
    public enum GameState
    {
        Null,               //Dummy entry
        GameRails,          //In-Game, on-the-rails gameplay mode
        GameAllRange,       //In-Game, All Range gameplay mode
        GameSpace,          //In-Game, Space gameplay mode
        FMV,                //Watching an FMV
        IGNORE,             //Dummy entry (for bootloader processing)
        MODEMAX = GameSpace
    }

    /// <summary>
    /// Game types
    /// </summary>
    public enum GameType
    {
        Null,           //Dummy entry
        StoryMode,      //Story Mode 
        TrainingMode,   //Training Mode (progress isn't saved)
        IGNORE,         //Dummy entry (for bootloader processing)
    };
    

    //GameState stuff
    public byte SaveSlot = 0;                                   //GameSave slot (0-2)    
    public string SaveName = "NewGame";                         //Name for the savefile
    public GameState gameState = GameState.FMV;                 //Game State
    public GameType gameType = GameType.Null;                   //Game Type
    #endregion

    //Level/scene mgmt
    public bool levelCompleted = false;                         //Flag set when a level has been completed, used for camera sequence of new level in Map
    
    public string SceneName;                    //Scene Name        

    #region PlayerPrefKeys
    //Main game options
    public const string mvolKey = "mvol";                       //Music volume
    public const string svolKey = "svol";                       //Sfx/Vox volume
    public const string difficultyKey = "difficulty";           //Difficulty
    public const string langKey = "lang";                       //Localization Language
    public const string pitchKey = "pitch";                     //Invert pitch

    //!@ This and affected functions should be renamed to something about "Layouts"
    //Yaw/Roll swapping for Saitek ST290 Pro/other readonly flighsticks should be done with a change of layout
    //The affected var ControlsSetup.whateverLayoutVarNamebool affects layout used for readonly stuff
    //public const string yawRollKey = "yawroll";                 //Swap yaw/roll for flightstick

    //VectorVision options
    public const string VV_MonType_key= "vv_montype";           //Vector vision monitor type
    public const string VV_Mode_key = "vv_mode";                //Vector vision mode
    public const string VV_EdgeFilter_key = "vv_edgefilter";    //VectorVision Edgefilter type
    public const string VV_ScaleX_key = "vv_scalex";            //VectorVision scaleX component
    public const string VV_ScaleY_key = "vv_scaley";            //VectorVision scaleY component
    public const string VV_FPS_key = "vv_fps";                  //VectorVision FPS capture rate
    public const string VV_T1_key = "vv_t1";                    //VectorVision t1 param
    public const string VV_T2_key = "vv_t2";                    //VectorVision t2 param    

    //Logitech G19/InControl stuff
    public const string G19_flash_key = "g19flash";             //Flash kbd colors in-game?
    public const string G19_video_key = "g19video";             //Display video and stats on the GLCD?

    //Video options (palette,psx shaders)
    public const string PAL_palType_key = "pal_paltype";        //Palette type
    public const string PAL_dither_key = "pal_dither";          //Dither?
    public const string PAL_ditherTex_key = "pal_dithtex";      //Dither texture
    public const string PAL_ditherSize_key = "pal_dithSize";    //Size of dither as percentage of original
    public const string PAL_ditherStrength_key = "pal_dithstr"; //Dither strength as percentag of original
    public const string PSX_useFog_key = "psx_usefog";          //Use PSX fog?
    public const string PSX_fogColor_r_key = "psx_fogcolor_r";  //PSX fog color     r comp
    public const string PSX_fogColor_g_key = "psx_fogcolor_g";  //~                 g comp
    public const string PSX_fogColor_b_key = "psx_fogcolor_b";  //~                 b comp
    public const string PSX_fogColor_a_key = "psx_fogcolor_a";  //~                 a comp
    public const string PSX_fogMinDist_key = "psx_fogmindist";  //PSX fog   min dist
    public const string PSX_fogMaxDist_key = "psx_fogmaxdist";  //~         max dist
    public const string PSX_texDist_key = "psx_textdist";       //~         texture distortion factor
    #endregion

    public const string CONTROLFILE = "controllerConfig.ccs";   //Control config filename
    public static ControlsHolder currentControls;               //Current Contrls
    //
    
    //public GameModes gameMode = GameModes.COOP;
    //[HideInInspector] //only for solo mode
    public Characters selectedCharacter = Characters.EAGLE;
    //[HideInInspector]
    public Difficulties difficulty = Difficulties.MEDIUM;

    //Cheats
    //!@    

    //Important global prefabs/data
    public bool cursor = true;          //Allow cursor

    //Game data
    public List<PlayerProfile> players;                         //PlayerProfiles
    public int livesLeft = 3;                                   //Amount of lives left
    //--------

    //More sfx stuff
    public float mvol = 1f;                                 //Music volume
    private float[] bgm_rng = new float[2] {-80f, 0f};      //Min and max DB range for BGM
    public float svol = 1f;                                 //Sound volume
    private float[] sfx_rng = new float[2]{-80f, 0f};       //Min and max DB range for SFX

    /// <summary>
    /// Localization lagnuages
    /// </summary>
    public enum Language
    {
        LANG_ENGLISH,
        LANG_SPANISH,
        LANG_FRENCH,
        LANG_CHINESE,
        LANG_RUSSIAN,
        LANG_MAX = LANG_RUSSIAN
    };
    public Font[] CapFonts;                                 //List of LangPack fonts that require capitalization to work properly
    public Language lang = Language.LANG_ENGLISH;           //Current localization language

    private IEnumerator OnLevelWasLoaded()
    {
        string scene = Application.loadedLevelName;       
        yield return null;
        //!@ Do level loaded stuff here!
    }

    public void BOOT()
    {
        const string boot = "Main Menu";
        Application.LoadLevel(boot);
    }

    void Awake()
    {
        if(instance != null)
        {
            StopAllVibration();
            Destroy(gameObject);
            return;            
        }
        DEBUG = true;   //!@ 

        //If an Audio script DNE, create one
        if (!audio)
        {
            audio = this.gameObject.AddComponent<Audio>();
        }

        //Set the instance, and DDoL
        instance = this;    
        DontDestroyOnLoad(gameObject);

        //init arrays
        //gameModesArray = Enum.GetValues(typeof(GameModes));
        //charactersArray = Enum.GetValues(typeof(Characters)); //!@

        //controls setup
        /*
		ControlsHolder controls = (ControlsHolder)ClassSerializer.Load(Application.persistentDataPath, CONTROLFILE);
        if (controls == null)
        {
        */
            //file dont exists, create default controls handler
            CreateDefaultControls();
        /*
        }
        else
        {
            //Otherwise, set current to save ones in PlayerPrefs file
            currentControls = controls;
        }
        */
        ToggleCursor(false);    //Disable display of cursor

        //!@ Old Coffee Code, refactor for usage with StarEagle specifics
        //Create new list of players, set default player profiles
        players = new List<PlayerProfile>();
        players.AddRange(new PlayerProfile[]
        {
            PlayerProfile.GetEagleDefaultSetup(),
            PlayerProfile.GetBunnyDefaultSetup(),
            PlayerProfile.GetOwlDefaultSetup(),
            PlayerProfile.GetDogDefaultSetup()
        }
        );
        //InitLCD();
    }

    /// <summary>
    /// Goes to the Map screen
    /// </summary>
    public void GotoMap()
    {
        Application.LoadLevel("Map");   //Goto dedicated Map scene
    }

    /// <summary>
    /// Toggles mouse cursor
    /// </summary>
    /// <param name="state">State</param>
    public void ToggleCursor(bool state)
    {
        cursor = state;
        Cursor.visible = cursor;
    }

    /*void Update(){}*/

    /// <summary>
    /// Removes all achievements from user's account, for debug testing. !WARNING!
    /// </summary>
    /*
    public void ACH_DELETE()
    {        
        SteamUserStats.ResetAllStats(true);
        SteamUserStats.StoreStats();
        audio.sfx_play(Audio.SFX.SFX_ALARM);
        Debug.LogError("!ALL ACHIEVEMENTS NUKED!");
    }
    */

    /// <summary>
    /// Resets playerPrefs. !WARNING!
    /// </summary>
    public void ResetPlayerPrefs()
    {
        Debug.Log("PLAYER PREFS DELETED!");
        PlayerPrefs.DeleteAll();
    }

    /// <summary>
    /// When application quits, do this stuff
    /// </summary>
    public void OnApplicationQuit()
    {
        Debug.Log("Killing application...");

        //Stop all lingering controller vibration
        StopAllVibration();

        //Save Steam stats, shtudown SteamAPI if SteamManager initzd
        /*
        if (SteamManager.Initialized)
        {
            Debug.Log("Saving steamstats...");
            SteamUserStats.StoreStats();
            Debug.Log("Shutting down steamworks");
            SteamAPI.Shutdown();    //Attempt to shutdown Steamworks
        }
        */
        
        //Quit the application
        Application.Quit();
    }

    /// <summary>
    /// Stops all vibration for all devices
    /// </summary>
    public void StopAllVibration()
    {
       
        Debug.Log("Stopping all vibration");
        /*
        //if (ControlsHandler.instance)
        //{
            byte players = (byte)(ReInput.players.playerCount);
            for (byte i = 0; i < players; i++)
            {
                Player player = ReInput.players.GetPlayer(i);
                player.StopVibration();

                //if (ControlsHandler.instance.inputDevices[i] != null){ControlsHandler.instance.inputDevices[i].Vibrate(0f);}

    }
        //}
        */
    }

    #region LoadPrefsIntFloat
    /*
    /// <summary>
    /// Sets the current game difficulty
    /// </summary>
    /// <param name="difficultyId">int of Difficulty ID enum</param>
    public void SetDifficulty(int difficultyId)
    {
        if (difficultyId >= difficultiesArray.Length)
            return;

        difficulty = (Difficulties)difficultiesArray.GetValue(difficultyId);
        PlayerPrefs.SetInt(difficultyKey, (int)difficulty);
    }
    */

    /// <summary>
    /// Set the MVol volume based on index of option's TextSwitch
    /// </summary>
    /// <param name="mvolID">Volume index ID</param>
    public void SetMVol(int mvolID)
    {
        //Cap at 10
        if (mvolID > 10)
            return;
        
        mvol = mvolID * 0.1f;                       //Volume = index /10f;
        float newVal = GetVolDB(AM_BGM, mvol);      //Convert percent to DB
        AM_BGM.SetFloat("Volume", newVal);          //Set new volume
        PlayerPrefs.SetInt(mvolKey, (int)(mvolID)); //Save to PlayerPrefs
    }

    /// <summary>
    /// Given a percentage and the AudioMixer type, converts percentage into DB
    /// </summary>
    /// <param name="AM">AudioMixer type</param>
    /// <param name="pcent">Percent</param>
    /// <returns>Volume in DB, lerped between appropriate range</returns>
    public float GetVolDB(UnityEngine.Audio.AudioMixer AM, float pcent)
    {
        float DB = 0f;          //DB to return
        const byte min=0;       //Index for min in range
        const byte max=1;       //~         max

        float x = pcent;        //Set x to pcent
        x = Mathf.Sqrt(x);      //Take sqrt(x). This is so that DB is linear instead of logarithmic
        if(AM==AM_BGM)          //
        {
            //If AM_BGM, lerp between bgm range by x
            DB = Mathf.Lerp(bgm_rng[min], bgm_rng[max], x);
        }
        else if (AM==AM_SFX)
        {
            //If AM_SFX, lerp between sfx range by x
            DB = Mathf.Lerp(sfx_rng[min], sfx_rng[max], x);
        }
        //Return value
        return DB;
    }

    /// <summary>
    /// Sets the language.
    /// </summary>
    /// <param name="language">(int)(language)</param>
    public void SetLang(int language)
    {
        lang = (Language)(language);
        PlayerPrefs.SetInt(langKey, language);
    }

    /// <summary>
    /// Set the SVol volume based on index of option's TextSwitch
    /// </summary>
    /// <param name="svolID">Volume index ID</param>
    public void SetSVol(int svolID)
    {
        //Cap at 10
        if (svolID > 10)
            return;

        svol = svolID * 0.1f;                       //Volume = index /10f;
        float newVal = GetVolDB(AM_SFX, svol);      //Convert pcent to DB
        AM_SFX.SetFloat("Volume", newVal);          //Set new volume in DB
        PlayerPrefs.SetInt(svolKey, (int)(svolID)); //Save to player prefs
    }

    /// <summary>
    /// Sets the pitch inversion
    /// </summary>
    /// <param name="value">Bit</param>
    public void SetPitch(int value)
    {
        bool state = (value == 1);          //Convert value to bool
        currentControls.controls[0].pitch_invert = state;
        PlayerPrefs.SetInt(pitchKey, value);        
    }

    /*
    /// <summary>
    /// Swaps yaw/roll axises
    /// </summary>
    /// <param name="value">Bit</param>
    public void SetYawRoll(int value)
    {
        bool state = (value == 1);          //Convert value to bool
        currentControls.controls[0].yawRoll_Swap = state;
        PlayerPrefs.SetInt(yawRollKey, value);
    }
    */

    /*
    /// <summary>
    /// Sets the VV_MonType
    /// </summary>
    /// <param name="value">(int)(VV_MonType)</param>
    /// <param name="save">Save to playerprefs?</param>
    public void SetVV_MonType(int value, bool save)
    {
        VV_MonType state = (VV_MonType)(value);
        VectorVisionSettings.New.MonitorType = state;
        if (save) { PlayerPrefs.SetInt(VV_MonType_key, value); }
    }

    /// <summary>
    /// Sets the VV_Mode
    /// </summary>
    /// <param name="value">(int)(VV_RenderMode)</param>
    /// <param name="save">Save to playerprefs?</param>
    public void SetVV_Mode(int value, bool save)
    {
        VV_Mode state = (VV_Mode)(value);
        VectorVisionSettings.New.RenderMode = state;
        if (save) { PlayerPrefs.SetInt(VV_Mode_key, value); }
    }

    /// <summary>
    /// Sets the VV_EdgeFilter
    /// </summary>
    /// <param name="value">(int)(VV_EdgeFilter)</param>
    /// <param name="save">Save to playerprefs?</param>
    public void SetVV_EdgeFilter(int value, bool save)
    {
        VV_EdgeFilter state = (VV_EdgeFilter)(value);
        VectorVisionSettings.New.EdgeFilter = state;
        if (save) { PlayerPrefs.SetInt(VV_EdgeFilter_key, value);}
    }

    /// <summary>
    /// Sets the VV_Scale.x
    /// </summary>
    /// <param name="value">value</param>
    /// <param name="save">Save to playerprefs?</param>
    public void SetVV_ScaleX(int value, bool save)
    {
        VectorVisionSettings.New.Scale.x = value;
        if (save) { PlayerPrefs.SetInt(VV_ScaleX_key, value);}
    }
    /// <summary>
    /// Sets the VV_Scale.y
    /// </summary>
    /// <param name="value">value</param>
    /// <param name="save">Save to playerprefs?</param>
    public void SetVV_ScaleY(int value, bool save)
    {
        VectorVisionSettings.New.Scale.y = value;
        if (save) { PlayerPrefs.SetInt(VV_ScaleY_key, value);}
    }

    /// <summary>
    /// Sets the VV_FPS
    /// </summary>
    /// <param name="value">FPS index id</param>
    /// <param name="save">Save to playerprefs?</param>
    public void SetVV_FPS(int id, bool save)
    {
        float value = VV_FPS_vals[id];
        VectorVisionSettings.New.FPS = value;
        if (save) { PlayerPrefs.SetFloat(VV_FPS_key, id);}
    }

    /// <summary>
    /// Sets the VV_T1 param
    /// </summary>
    /// <param name="value">param value</param>
    /// <param name="save">Save to playerprefs?</param>
    public void SetVV_T1(int value, bool save)
    {
        VectorVisionSettings.New.t1 = (byte)(value);
        if (save) { PlayerPrefs.SetInt(VV_T1_key, value);}
    }

    /// <summary>
    /// Sets the VV_T2 param
    /// </summary>
    /// <param name="value">param value</param>
    /// <param name="save">Save to playerprefs?</param>
    public void SetVV_T2(int value, bool save)
    {
        VectorVisionSettings.New.t2 = (byte)(value);
        if (save) { PlayerPrefs.SetInt(VV_T2_key, value);}
    }

    /// <summary>
    /// Sets the G19 Flash setting
    /// </summary>
    /// <param name="value">param</param>
    public void SetG19_Flash(int value)
    {
        bool state = (value == 1);
        LogKbd.KbdSettings.flash = state;
        PlayerPrefs.SetInt(G19_flash_key, value);
    }

    /// <summary>
    /// Sets the G19 Flash setting
    /// </summary>
    /// <param name="value">param</param>
    public void SetG19_Video(int value)
    {
        bool state = (value == 1);
        LogKbd.KbdSettings.video = state;
        PlayerPrefs.SetInt(G19_video_key, value);
    }      

    /// <summary>
    /// Sets the Palette palType
    /// </summary>
    /// <param name="id">palType id</param>
    public void SetPAL_palType(int id)
    {
        video_settings.palette.paletteType = (RetroPixelMax.palType)(id);
        PlayerPrefs.SetInt(PAL_palType_key, id);
        StartCoroutine(UpdateRetroGFX());
    }

    /// <summary>
    /// Sets the dither state
    /// </summary>
    /// <param name="state">State as boolean bit</param>
    public void SetPAL_dither(int state)
    {
        bool s = (state == 1);
        video_settings.palette.dither = s;
        PlayerPrefs.SetInt(PAL_dither_key, state);
        StartCoroutine(UpdateRetroGFX());
    }

    /// <summary>
    /// Sets the dither texture by index
    /// </summary>
    /// <param name="id">Texture index</param>
    public void SetPAL_ditherTex(int id)
    {
        video_settings.palette.dither_texture = ditherTextures[id];
        PlayerPrefs.SetInt(PAL_ditherTex_key, id);
        StartCoroutine(UpdateRetroGFX());
    }

    /// <summary>
    /// Sets the ditherSize as percentage float
    /// </summary>
    /// <param name="val">Dither size as pcent (0-1f)</param>
    public void SetPAL_ditherSize(float val)
    {
        video_settings.palette.dither_size = val;
        PlayerPrefs.SetFloat(PAL_ditherSize_key, val);
        //Debug.Log("Key/value: " + PAL_ditherSize_key + ", " + val.ToString("F2"));
        StartCoroutine(UpdateRetroGFX());
    }
    /// <summary>
    /// Sets the ditherStrenght as percentage float
    /// </summary>
    /// <param name="val">Dither strength as pcent (0-1f)</param>
    public void SetPAL_ditherStrength(float val)
    {
        video_settings.palette.dither_strength = val;
        PlayerPrefs.SetFloat(PAL_ditherStrength_key, val);
        //Debug.Log("Key/value: " + PAL_ditherStrength_key + ", " + val.ToString("F2"));
        StartCoroutine(UpdateRetroGFX());
    }
    
    /// <summary>
    /// Sets the fog state
    /// </summary>
    /// <param name="state">State as boolean bit</param>
    public void SetPSX_useFog(int state)
    {
        bool s = (state==1);
        video_settings.psx_shader.useFog = s;
        PlayerPrefs.SetInt(PSX_useFog_key, state);
        StartCoroutine(UpdateRetroGFX());
    }

    /// <summary>
    /// Sets the fogColor by val and component dim
    /// </summary>
    /// <param name="val">Component value as $byte</param>
    /// <param name="dim">Component index. 0=r, 1=g, 2=b, 3=a</param>
    public void SetPSX_fogColor(int val, byte dim)
    {
        //PlayerPref Keys for each component
        string[] keys = {PSX_fogColor_r_key,PSX_fogColor_g_key,PSX_fogColor_b_key,PSX_fogColor_a_key};
        string key = keys[dim]; //Get the correct key for the dim component

        //Set the color value for the correct component
        switch (dim)
        {
            //r
            case 0:
                video_settings.psx_shader.fogColor.r = (byte)(val);
                break;
            //g
            case 1:
                video_settings.psx_shader.fogColor.g = (byte)(val);
                break;
            //b
            case 2:
                video_settings.psx_shader.fogColor.b = (byte)(val);
                break;
            //a
            case 3:
                video_settings.psx_shader.fogColor.a = (byte)(val);
                break;
        }        
        PlayerPrefs.SetInt(key, val);           //Save the appopriate key
        StartCoroutine(UpdateRetroGFX());
    }

    /// <summary>
    /// Sets the fog min dist
    /// </summary>
    /// <param name="val">Fog min dist</param>
    public void SetPSX_fogMinDist(float val)
    {
        video_settings.psx_shader.fog_minDist = val;
        PlayerPrefs.SetFloat(PSX_fogMinDist_key, val);
        //Debug.Log("Key/value: " + PSX_fogMinDist_key + ", " + val.ToString("F2"));
        StartCoroutine(UpdateRetroGFX());
    }

    /// <summary>
    /// Sets the fog max dist
    /// </summary>
    /// <param name="val">Fog max dist</param>
    public void SetPSX_fogMaxDist(float val)
    {
        video_settings.psx_shader.fog_maxDist = val;
        PlayerPrefs.SetFloat(PSX_fogMaxDist_key, val);
        //Debug.Log("Key/value: " + PSX_fogMaxDist_key + ", " + val.ToString("F2"));
        StartCoroutine(UpdateRetroGFX());
    }

    /// <summary>
    /// Sets the PSX shader texture distortion value as $Byte
    /// </summary>
    /// <param name="val">Texture dist value as $Byte</param>
    public void SetPSX_textDist(int val)
    {
        video_settings.psx_shader.tex_distortion = (byte)(val);
        PlayerPrefs.SetInt(PSX_texDist_key, val);
        StartCoroutine(UpdateRetroGFX());
    }

    /// <summary>
    /// Updates the RetroGFX settings (palette, PSX shader)
    /// </summary>
    /// <returns>yield</returns>
    private IEnumerator UpdateRetroGFX()
    {        
        yield return new WaitForSeconds(.25f);
        RetroGFX.instance.UpdateRetroGFX();
    }
    */
    #endregion

    /// <summary>
    /// XOR between 3 bools
    /// https://stackoverflow.com/a/6228356
    /// </summary>
    /// <param name="a">Reg a</param>
    /// <param name="b">Reg b</param>
    /// <param name="c">Reg c</param>
    /// <returns>3-way XOR result</returns>
    //!@ Dead Coffee Code?
    /*
    public bool TernaryXor(bool a, bool b, bool c)
    {
        //return ((a && !b && !c) || (!a && b && !c) || (!a && !b && c));

        // taking into account Jim Mischel's comment, a faster solution would be:
        return (!a && (b ^ c)) || (a && !(b || c));
    }
    */

    /// <summary>
    /// Get the active, non-dead players
    /// </summary>
    /// <returns></returns>
    public PlayerProfile[] GetActivePlayers()
    {
        List<PlayerProfile> active = new List<PlayerProfile>();
        foreach (PlayerProfile profile in players)
        {
            //!@ 
            /*
            if (profile.hpLeft > 0 && profile.playerObject)
            {
                active.Add(profile);
            }
            */
        }
        return active.ToArray();
    }


    //Custom steamworks methods
    #region SteamWorks_Ach

    /// <summary>
    /// Increment counter for int stat
    /// </summary>
    /// <param name="ach">Achievement tag string</param>
    /// <param name="stat">Stat tag string</param>
    /// <param name="target">Target value to give achievement</param>    
    /*
    public void ACH_Count(string ach, string stat, int target)
    {
        //If cheating or demo, break
        if (Cheater == true)
        {
            Debug.Log("Achievement denied, you cheater!");
            return;
        }

        int value = 0;
        SteamUserStats.GetStat(stat, out value);
        value++;
        SteamUserStats.SetStat(stat, value);
        Debug.Log(stat + " count: " + value.ToString());
        if (value>= target)
        {
            bool achieved=false;
            SteamUserStats.GetAchievement(ach, out achieved);
            if (achieved == false)
            {
                audio.sfx_play(Audio.SFX.SFX_ACH);
            }
            SteamUserStats.SetAchievement(ach);
            SteamUserStats.StoreStats();
            Debug.Log(ach + " awarded!");
            ACH_AllEarned();
        }
        SteamUserStats.StoreStats();
    }

    /// <summary>
    /// Set achievment stat to boolean true bit (1)
    /// </summary>
    /// <param name="ach">Achievment tag string</param>
    /// <param name="stat">Stat tag string</param>    
    public void ACH_Bool(string ach, string stat)
    {
        //If cheating but NOT ACH_LevelPW or ACH_CheatPW or ACH_Achiever, skip
        //if ((Cheater == true)&&(ach!="ACH_LevelPW")&&(ach!="ACH_CheatPW")&&(ach!="ACH_Achiever"))
        if (Cheater == true)
        {
            Debug.Log("Achievement denied, you cheater!");
            return;
        }

        bool achieved = false;
        SteamUserStats.GetAchievement(ach, out achieved);
        if (achieved == false)
        {
            audio.sfx_play(Audio.SFX.SFX_ACH);
        }
        //SteamUserStats.SetStat(stat, 1);
        SteamUserStats.SetAchievement(ach);
        SteamUserStats.StoreStats();
        Debug.Log(ach + " awarded!");
        ACH_AllEarned();
    }
    */

    /// <summary>
    /// Set achievment stat to int value, give achievment on target value
    /// </summary>
    /// <param name="ach">Achievment tag string</param>
    /// <param name="stat">Stat tag string</param>
    /*
    public void ACH_SetInt(string ach, string stat, int value, int target)
    {
        //If cheating, break
        if (Cheater == true)
        {
            Debug.Log("Achievement denied, you cheater!");
            return;
        }

        Debug.Log(stat + " test value:" + value.ToString());
        if (value >= target)
        {
            bool achieved = false;
            SteamUserStats.GetAchievement(ach, out achieved);
            if (achieved == false)
            {
                audio.sfx_play(Audio.SFX.SFX_ACH);
            }
            ACH_Bool(ach, stat);
            ACH_AllEarned();
        }
    }    

    //!@ This needs refactored for StarEagle's achievement stats/names! Dead Coffee Code
    /// <summary>
    /// Checks if all achievments are earned. If so, awards achievment
    /// </summary>    
    public void ACH_AllEarned()
    {
        const byte max = 47;
        string ach = "ACH_Achiever";
        string stat = "STA_Achiever";

        string[] ACH_names =
        {
            "ACH_CompGame_Ash",
            "ACH_CompGame_Nick",
            "ACH_CompGame_Coop",
            "ACH_CompStage1",
            "ACH_CompStage2",
            "ACH_CompStage3",
            "ACH_CompStage4",
            "ACH_CompStage5",
            "ACH_CompStage6",
            "ACH_CompStage7",
            "ACH_CompStage8",
            "ACH_CompStage9",
            "ACH_CompStage10",
            "ACH_BeatBoss0",
            "ACH_BeatBoss2",
            "ACH_BeatBoss3",
            "ACH_KillAliens",
            "ACH_KillEnemies",
            "ACH_BonusCups",
            "ACH_BonusGame",
            "ACH_Vandal",
            "ACH_LifePnts",
            "ACH_AlienChow",
            "ACH_CompGame_3",
            "ACH_BeatBossFull",
            "ACH_LevelPW",
            "ACH_CheatPW",
            "ACH_PowLives",
            "ACH_PowInvin",
            "ACH_PowTurbo",
            "ACH_CompGame_Metal",
            "ACH_CompGame_DMetal",
            "ACH_CompGame_XMetal",
            "ACH_CompGame_YMetal",

            "ACH_CompStage4_2",
            "ACH_CompStage8_2",
            "ACH_CompStage11",
            "ACH_CompStage11_2",
            "ACH_CompStage12",
            "ACH_CompStage13",
            "ACH_CompStage13_2",
            "ACH_CompStage14",
            "ACH_CompStage14_2",
            "ACH_Song",
            "ACH_WepGuitar",
            "ACH_WepAlienArm",
            "ACH_KillMayo",
        };

        bool earned = true;
        bool state=false;

        //If overachiever is already awarded, skip the checks.
        //Prevent weird softlock
        SteamUserStats.GetAchievement(ach, out state);
        if (state == true)
        {
            return;
        }

        byte i=0;
        for (i = 0; i < max; i++)
        {
            SteamUserStats.GetAchievement(ACH_names[i], out state);
            earned = earned & state;

            if (earned == false)
            {
                //An achievment was locked; skip checking, you fail it
                return;
            }
        }
        Debug.Log("All achievments earned!");
        ACH_Bool(ach, stat);
    }

    /// <summary>
    /// Toggle bit in byte for achievment stat, give achievment on target set
    /// </summary>
    /// <param name="ach">Achievment tag string</param>
    /// <param name="stat">Stat tag string</param>
    /// <param name="bit">Bit to change</param>
    /// <param name="target">Target value</param>    
    public void ACH_Bitfield(string ach, string stat, byte bit, byte target)
    {
        //If cheating, break
        if (Cheater == true)
        {
            Debug.Log("Achievement denied, you cheater!");
            return;
        }

        int value=0;
        SteamUserStats.GetStat(stat, out value);                            //Fetch the value at server
        byte byt = (byte)(value);
        BitArray bits = new BitArray (new byte[] {byt});
        bits.Set(bit, true);                                                //Set specified bit
        value = ConvertToByte(bits);                                        //Convert modified bit array back to byte        
        SteamUserStats.SetStat(stat, value);                                //Save the value back onto server
        Debug.Log(stat + " bitfield: " + ToBitString(bits));

        //Does new value meet target?
        if (value == target)
        {
            bool achieved = false;
            SteamUserStats.GetAchievement(ach, out achieved);
            if (achieved == false)
            {
                audio.sfx_play(Audio.SFX.SFX_ACH);
            }
            SteamUserStats.SetAchievement(ach);
            SteamUserStats.StoreStats();
            Debug.Log(ach + " awarded!");
            ACH_AllEarned();
        }
        SteamUserStats.StoreStats();    //Resync stats
    }   
    */

    // http://stackoverflow.com/a/560158
    /// <summary>
    /// Converts a BitArray into a byte
    /// </summary>
    /// <param name="bits">Array of bits</param>
    /// <returns>Byte</returns>
    public static byte ConvertToByte(BitArray bits)
    {
        Debug.Log("Bits: " + bits.Count.ToString());
        if (bits.Count > 8)
            //Duh, Einstein.
            throw new ArgumentException("ConvertToByte can only work with a BitArray containing a maximum of 8 values");

        byte result = 0;

        for (byte i = 0; i < bits.Count; i++)
        {
            if (bits[i])
                result |= (byte)(1 << i);
        }

        return result;
    }

    //http://stackoverflow.com/a/8991834
    /// <summary>
    /// Converts a BitArray into a string
    /// </summary>
    /// <param name="bits">BitArray</param>
    /// <returns>String</returns>
    public string ToBitString(BitArray bits)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < bits.Count; i++)
        {
            char c = bits[i] ? '1' : '0';
            sb.Append(c);
        }
        return sb.ToString();
    }
    #endregion

    /// <summary>
    /// Creates the default controls for the player
    /// </summary>
    public void CreateDefaultControls()
    {        
        currentControls = new ControlsHolder(ControlsSetup.DefaultPlayer1Setup());
        //SaveControls();
    }

    /*
    /// <summary>
    /// Saves the Controls to the CCS file
    /// </summary>
    public void SaveControls()
    {   
        //ClassSerializer.Save(Application.persistentDataPath, CONTROLFILE, GameManager.currentControls);
    }
    */

    /// <summary>
    /// Sets a random seed 
    /// </summary>
    public void RandomSeed()
    {
        int value = UnityEngine.Time.frameCount;
        UnityEngine.Random.InitState(value);
    }
}

/*
//Dead Coffee Code
public enum GameModes
{
    SOLO,
    COOP,
}
*/

/// <summary>
/// Character to play as
/// </summary>
public enum Characters
{
    EAGLE,      //Sammy the Eagle
    BUNNY,      //Slippery the Snowhare
    OWL,        //Gregorio the Barn Howl
    DOG,        //Thomas the Prairie Dog
    MAX = DOG   //MAX used for counting
};

public enum PlayerType
{
    PLAYER,
    CPU,
    MAX=CPU
};

/// <summary>
/// Difficulties
/// </summary>
public enum Difficulties
{
    EASY,
    MEDIUM,
    HARD,
    MAX = HARD  //MAX used for counting
}