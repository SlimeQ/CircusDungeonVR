using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public static PauseScreen instance;
    public ScreenItem Retry;
    public bool paused = false; //Game paused?

    private void Start()
    {
        instance = this;
    }

    /// <summary>
    /// Toggle the pause state
    /// </summary>
    public void TogglePause()
    {
        //Dew it, and proper un/pause code
        paused = !paused;   
        if (paused)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    public void Pause()
    {
        bool RetryEnable = (GameManager.instance.livesLeft > 0);
        UnityEngine.Time.timeScale = 0f;
        paused = true;
        ToggleAudio(paused);
        Audio.instance.sfx_play(Audio.SFX.SFX_PAUSE);

        int val = 0;
        if (RetryEnable){val = 1;}
        Retry.ToggleEnabled(val);
    }

    /// <summary>
    /// Unpauses the game
    /// </summary>
    public void UnPause()
    {
        UnityEngine.Time.timeScale = 1f;
        paused = false;
        ToggleAudio(paused);
        Audio.instance.sfx_play(Audio.SFX.SFX_PAUSE);
    }

    /// <summary>
    /// Toggles un/pausing all sfx audios sources and of the bgm singleton instance
    /// </summary>
    /// <param name="pause">Pause the audio?</param>
    private void ToggleAudio(bool pause)
    {
        Audio bgm = Audio.instance; //The bgm singleton instance
        Audio[] aud;                //Array of Audio scripts

        if (bgm)
        {
            //Due to the fact fadeIns/Outs and jingles are scaledWaitForSeconds couroutine yields,
            //we can safely pause the audio there, and unpausing the music and then unpausing the game (restoring time)
            //will resume normal operation

            if (pause)
            {
                //If pausing, pause the bgm if it is playing                
                if (bgm.audio_bgm.isPlaying)
                {
                    bgm.music_pause();
                }
            }
            else
            {
                //If unpause if not pausing
                bgm.music_unpause();
            }
        }

        //Get all Audio scripts
        aud = GameObject.FindObjectsOfType<Audio>();
        foreach (Audio a in aud)
        {
            if (a.audio_sfx)
            {
                //If pausing, pause playing sfx; else unpause all
                if (pause)
                {
                    if (a.audio_sfx.isPlaying)
                    {
                        a.sfx_pause();
                    }
                }
                else
                {
                    a.sfx_unpause();
                }
            }
        }
    }

    public void ResetTime()
    {
        UnityEngine.Time.timeScale = 1f;
    }
}