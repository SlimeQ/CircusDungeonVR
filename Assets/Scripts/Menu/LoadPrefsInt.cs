using System;
using System.Collections;
using UnityEngine;
using ByteSheep.Events;

/// <summary>
/// Loads integer PlayerPrefs
/// </summary>
public class LoadPrefsInt : MonoBehaviour
{
    public static byte kbdCounter = 1;      //Static counter for amount of Logitech G19 playerprefs with keys loaded. See IncrementKbdCounter() func
    public AdvancedIntEvent onValueLoaded;  //What to do onValueLoaded(int)
    public string key;                      //The playerprefs key string (see constants in GameManager script)

    void Start()
    {
        int value;                                  //Value of the key

        //Does the key exist?
        bool exists = PlayerPrefs.HasKey(key);      
        if (exists)
        {
            //If so, get value to use
            value = PlayerPrefs.GetInt(key);
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
    private int KeyNREError(string key)
    {        
        int value = 0;  //Value to return
        //Dummy VectorVision struct instance to fetch default values from constructor
        //!@
        //GameManager.VectorVision dummy = new GameManager.VectorVision();
        //GLCD.LGKBD dummy2 = new GLCD.LGKBD();
        //GameManager.RetroGFX_Settings dummyGfx = new GameManager.RetroGFX_Settings();

        //Return default value for key type
        switch (key)
        {                        
            case GameManager.mvolKey:
                value = 10;
                break;
            case GameManager.svolKey:
                value = 10;
                break;
            case GameManager.difficultyKey:
                value = (int)(Difficulties.MEDIUM);
                break;
            case GameManager.langKey:
                value = (int)(GameManager.Language.LANG_ENGLISH);
                break;
            case GameManager.pitchKey:
                value = 1;
                break;
            /*
            case GameManager.yawRollKey:
                value = 1;
                break;
            */

            /*
            case GameManager.VV_MonType_key:
                value = (int)(dummy.MonitorType);
                break;
            case GameManager.VV_Mode_key:
                value = (int)(dummy.RenderMode);
                break;
            case GameManager.VV_EdgeFilter_key:
                value = (int)(dummy.EdgeFilter);
                break;
            case GameManager.VV_ScaleX_key:
                value = (int)(dummy.Scale.x);
                break;
            case GameManager.VV_ScaleY_key:
                value = (int)(dummy.Scale.y);
                break;
            case GameManager.VV_FPS_key:
                value = GameManager.VV_FPS_defInd;
                break;
            case GameManager.VV_T1_key:
                value = (int)(dummy.t1);
                break;
            case GameManager.VV_T2_key:
                value = (int)(dummy.t2);
                break;
            case GameManager.G19_flash_key:
                value = BoolToBit(dummy2.flash);
                break;
            case GameManager.G19_video_key:
                value = BoolToBit(dummy2.video);
                break;

            case GameManager.PAL_palType_key:
                value = (int)(dummyGfx.palette.paletteType);
                break;
            case GameManager.PAL_dither_key:
                value = BoolToBit(dummyGfx.palette.dither);
                break;
            case GameManager.PAL_ditherTex_key:
                value = 0;
                break;

            case GameManager.PSX_useFog_key:
                value = BoolToBit(dummyGfx.psx_shader.useFog);
                break;
            case GameManager.PSX_fogColor_r_key:
                value = dummyGfx.psx_shader.fogColor.r;
                break;
            case GameManager.PSX_fogColor_g_key:
                value = dummyGfx.psx_shader.fogColor.g;
                break;
            case GameManager.PSX_fogColor_b_key:
                value = dummyGfx.psx_shader.fogColor.b;
                break;
            case GameManager.PSX_fogColor_a_key:
                value = dummyGfx.psx_shader.fogColor.a;
                break;
            case GameManager.PSX_texDist_key:
                value = dummyGfx.psx_shader.tex_distortion;
                break;
            */
        }
        return value;
    }

    #region MainOptions
    /// <summary>
    /// Sets the MVol value from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetMVol(int id)
    {
        GameManager.instance.SetMVol(id);
    }
    /// <summary>
    /// Sets the SVol value from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetSVol(int id)
    {
        GameManager.instance.SetSVol(id);
    }

    /*
    /// <summary>
    /// Sets the Difficulty value from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetDifficulty(int id)
    {
        GameManager.instance.SetDifficulty(id);
    }
    */

    /// <summary>
    /// Sets the language value from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetLang(int id)
    {
        GameManager.instance.SetLang(id);
    }

    /*
    /// <summary>
    /// Sets the invert pitch value from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetPitch(int id)
    {
        GameManager.instance.SetPitch(id);
    }
    */

    /*
    /// <summary>
    /// Swaps yaw/roll axises for flightstick from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetYawRoll(int id)
    {
        GameManager.instance.SetYawRoll(id);
    }
    */
    #endregion

    //Versions of VV to save keys
    /*
    #region VectorVisionSave
    /// <summary>
    /// Sets the VV Mon Type value from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_MonType(int id)
    {
        GameManager.instance.SetVV_MonType(id,true);
    }
    /// <summary>
    /// Sets the VV Mode value from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_Mode(int id)
    {
        GameManager.instance.SetVV_Mode(id, true);
    }
    /// <summary>
    /// Sets the VV Edge Filter from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_EdgeFilter(int id)
    {
        GameManager.instance.SetVV_EdgeFilter(id, true);
    }
    /// <summary>
    /// Sets the VV ScaleX from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_ScaleX(int id)
    {
        GameManager.instance.SetVV_ScaleX(id, true);
    }
    /// <summary>
    /// Sets the VV ScaleY from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_ScaleY(int id)
    {
        GameManager.instance.SetVV_ScaleY(id, true);
    }
    /// <summary>
    /// Sets the VV FPS from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_FPS(int id)
    {
        GameManager.instance.SetVV_FPS(id, true);
    }
    /// <summary>
    /// Sets the VV T1 param from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_T1(int id)
    {
        GameManager.instance.SetVV_T1(id, true);
    }
    /// <summary>
    /// Sets the VV T2 param from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_T2(int id)
    {
        GameManager.instance.SetVV_T2(id, true);
    }
    #endregion

    //Versions of VV to not save keys
    #region VectorVisionNoSave
    /// <summary>
    /// Same as SetVV_MonType without save
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_MonType2(int id)
    {
        GameManager.instance.SetVV_MonType(id, false);
    }
    /// <summary>
    /// Same as SetVV_Mode without save
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_Mode2(int id)
    {
        GameManager.instance.SetVV_Mode(id, false);
    }
    /// <summary>
    /// Same as SetVV_EdgeFilter without save
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_EdgeFilter2(int id)
    {
        GameManager.instance.SetVV_EdgeFilter(id, false);
    }
    /// <summary>
    /// Same as SetVV_ScaleX without save
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_ScaleX2(int id)
    {
        GameManager.instance.SetVV_ScaleX(id, false);
    }
    /// <summary>
    /// Same as SetVV_ScaleY without save
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_ScaleY2(int id)
    {
        GameManager.instance.SetVV_ScaleY(id, false);
    }
    /// <summary>
    /// Same as SetVV_FPS without save
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_FPS2(int id)
    {
        GameManager.instance.SetVV_FPS(id, false);
    }
    /// <summary>
    /// Same as SetVV_T1 without save
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_T1_2(int id)
    {
        GameManager.instance.SetVV_T1(id, false);
    }
    /// <summary>
    /// Same as SetVV_T2 without save
    /// </summary>
    /// <param name="id">Value</param>
    public void SetVV_T2_2(int id)
    {
        GameManager.instance.SetVV_T2(id, false);
    }
    #endregion

    #region LG19
    /// <summary>
    /// Sets the Logitech G19 flash from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetG19_Flash(int id)
    {
        GameManager.instance.SetG19_Flash(id);
        IncrementKbdCounter();
    }
    /// <summary>
    /// Sets the Logitech G19 Video from PPkey to RAM in GM and then saves PP
    /// </summary>
    /// <param name="id">Value</param>
    public void SetG19_Video(int id)
    {
        GameManager.instance.SetG19_Video(id);
        IncrementKbdCounter();
    }
    #endregion

    #region VideoPal
    public void SetPAL_palType(int id)
    {
        GameManager.instance.SetPAL_palType(id);
    }
    public void SetPAL_dither(int id)
    {
        GameManager.instance.SetPAL_dither(id);
    }
    public void SetPAL_ditherTex(int id)
    {
        GameManager.instance.SetPAL_ditherTex(id);
    }
    #endregion

    #region VideoPSX
    public void SetPSX_useFog(int state)
    {
        GameManager.instance.SetPSX_useFog(state);
    }
    public void SetPSX_fogColor_r(int val)
    {
        GameManager.instance.SetPSX_fogColor(val, 0);
    }
    public void SetPSX_fogColor_g(int val)
    {
        GameManager.instance.SetPSX_fogColor(val, 1);
    }
    public void SetPSX_fogColor_b(int val)
    {
        GameManager.instance.SetPSX_fogColor(val, 2);
    }
    public void SetPSX_fogColor_a(int val)
    {
        GameManager.instance.SetPSX_fogColor(val, 3);
    }
    public void SetPSX_textDist(int val)
    {
        GameManager.instance.SetPSX_textDist(val);
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

    /// <summary>
    /// Converts bool to bit
    /// </summary>
    /// <param name="state">Bool</param>
    /// <returns>Bit</returns>
    private int BoolToBit(bool state)
    {
        int result = 1;
        if (state == false)
        {
            result = 0;
        }
        return result;
    }

    /*
    /// <summary>
    /// Increments the kbdCount.
    /// When Logitech G19 userSettings count is reached, init the kbd settings with the loaded key values.
    /// </summary>
    private void IncrementKbdCounter()
    {
        kbdCounter++;
        if (kbdCounter >= GLCD.LGKBD.userSettings)
        {
            GameManager.instance.InitLCD();
        }
    }
    */
}