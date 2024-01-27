using UnityEngine;
using UnityEngine.UI;
//using Steamworks;

using System.Collections;

/*
/// <summary>
/// Handles the end-of-level score screen in StarEagle, and handling the scores.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private const ushort MAXSCORE = 999;        //16-bit max for level score
    private Audio audio;

    [HideInInspector]
    public int levelScores = 0;
    public Text totalScore;
    private bool Ready = false;

    void Start()
    {
        instance = this;
        audio = this.gameObject.GetComponent<Audio>();
    }
  
    public void UpdateUI()
    {
        totalScore.text = GetTotalScore(true).ToString("D5");
        //!@ Handle cutscene/blurb shit here?
        LevelComp();
    }

    //!@ Steamworks
    private void LevelComp()
    {     
        //!@ Adjust me for level completion achievements later!
        //if (SteamManager.Initialized)
        //{
        //    const string stage = "Stage ";
        //    string scenename = Application.loadedLevelName;
        //    Debug.Log("Scenename: " + scenename);
        //    string num = scenename.Replace(stage, "");
        //    string ach = "ACH_CompStage" + num;
        //    string sta = "STA_CompStage" + num;
        //    Debug.Log("Stage #: " + num);
        //    GameManager.instance.ACH_Bool(ach, sta);
        //}
    }

    public void AddPoints(int points)
    {
        //Add and cap scores
        levelScores += points;
        levelScores = Mathf.Clamp(levelScores, 0, MAXSCORE);
        LivesCount.instance.UpdateScore(levelScores);
    }

    public int GetTotalScore(bool ingame)
    {
        int total = 0;        
        byte i = 0; byte max = GameManager.max_mission_slots;
        byte curIndex = GameManager.instance.mission_index;
        for (i = 0; i < max; i++)
        {
            int score = 0;
            if (ingame)
            {
                if (i != curIndex)
                {
                    score = GameManager.instance.Missions[i].score;
                }
                else
                {
                    score = levelScores;
                }
            }
            else
            {
                score = GameManager.instance.Missions[i].score;
            }
            total += score;
        }
        return total;
    }

    public void TogglePath(int newPath)
    {
        byte b = (byte)(newPath);
        GameManager.instance.currentPath = b;
    }

    public void StartEndOfLevel()
    {
        MainMenuController MMC = MainMenuController.instance;
        if (MMC)
        {
            Audio.instance.music_play(Audio.BGM.GAME_VICTORY);
            GameManager.instance.levelCompleted = true;
            foreach (PlayerProfile p in GameManager.instance.players)
            {
                if (p!=null)
                {
                    if (p.playerObject != null)
                    {
                        p.playerObject.isDead = true;
                        p.playerObject.Stop();
                    }
                }
            }

            IngameCameraController IGC = IngameCameraController.instance;
            if (IGC)
            {
                IGC.ToggleCockpit(false);
            }

            byte ID = (byte)(MainMenuController.GameMenus.RESULTS_MISSCOMP);
            MMC.OpenScreen(ID);
        }
    }
}
*/