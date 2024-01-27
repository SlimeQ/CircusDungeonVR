
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//!@ Dead Coffee Code?
/// <summary>
/// Handles in-game text for FTZ messages
/// </summary>
public class PlayerInfoScreen : MonoBehaviour
{
    /*
    public static PlayerInfoScreen instance;
    public Text infoText;
    public Font altFont;                            //Alt font to use for foreign langs
    public AlphaTween AT;                           //Alpha tween for text
    public PositionTween PT;                        //Position tween for ticker
    private List<string> msg = new List<string>();  //List of messages to process
    [HideInInspector]public const float scrollTime = 3f;

    void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        if (GameManager.instance)
        {
            //If not English, use altfont. This will ensure accented vowels, Chinese scribble, and Russian Cryllic appear properly
            if (GameManager.instance.lang != GameManager.Language.LANG_ENGLISH)
            {
                infoText.font = altFont;
            }
        }

    }

    /// <summary>
    /// Clears the list of messages
    /// </summary>
    public void Reset()
    {
        StopAllCoroutines();
        msg.Clear();
    }

    /// <summary>
    /// Rests, adds "Proceed" text, plays it. Used for end of FTZs
    /// </summary>
    public void PlayText_Pro()
    {
        Reset();
        Localization.LangPack pack = new Localization.LangPack("Proceed!", "¡Prosigue!", "Avancez!", "前进！", "Вперед!");
        string msg = pack.language;
        AddText(msg);
        Play();
    }

    /// <summary>
    /// Adds a message to the list
    /// </summary>
    /// <param name="value"></param>
    public void AddText(string value)
    {
        msg.Add(value);
    }

    /// <summary>
    /// Plays a list of messages
    /// </summary>
    public void Play()
    {
        StartCoroutine(_Play());
    }

    /// <summary>
    /// Plays a list of messages
    /// </summary>
    /// <returns>yield</returns>
    private IEnumerator _Play()
    {
        //Wait for messages to get added to list, then process
        yield return new WaitForSeconds(.1f);   
        AT.PlayForward();                       //Fade in

        //Iterate through all strings in list
        foreach (string s in msg)
        {
            infoText.enabled = false;                       //Disable the text
            yield return new WaitForSeconds(.1f);           //yield
            PT.OnReset();                                   //Reset position
            yield return new WaitForSeconds(.1f);           //yield
            infoText.enabled = true;                        //Enable the text

            infoText.text = s;                              //Set text to message
            PT.playbackTime = scrollTime;
            PT.PlayForward();                               //Scroll it in
            yield return new WaitForSeconds(scrollTime);    //Yield scrollTime seconds while PT plays
        }
        AT.PlayBackward();                                  //Fade out
        Reset();                                            //Clear message list
    }
    */
}
