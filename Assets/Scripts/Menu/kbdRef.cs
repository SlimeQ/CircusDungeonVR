using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//!@ Dead Coffee Code.
/// <summary>
/// Used for updating the keyboard reference graphic, to highlight which keys have been currently mapped to something
/// </summary>
public class kbdRef : MonoBehaviour
{
    /*
    public static kbdRef instance;                      //Singleton ref
    private const byte maxKeys = 33;                    //Max amount of keys to process
    public string[] keys = new string[maxKeys];         //The key text strings for each key
    public GameObject[] btn = new GameObject[maxKeys];  //Their corresponding GameObject on the keyboard graphic

    private IEnumerator Start()
    {
        //Set the singleton ref to this
        instance=this;
        ToggleAllKeys(true);
        yield return new WaitForSeconds(.25f);
        ToggleAllKeys(false);
    }

    /// <summary>
    /// Toggles the state of all Keys.
    /// Used for temporarily turning all on and then off,
    /// so they initalize with the same highlight speeds (and stay N'Sync yo)
    /// </summary>
    /// <param name="state"></param>
    public void ToggleAllKeys(bool state)
    {
        foreach(string k in keys)
        {
            ToggleKey(k,state);
        }
    }

    /// <summary>
    /// Toggles the highlight state of a particular key
    /// </summary>
    /// <param name="key">Key text string</param>
    /// <param name="state">State</param>
    public void ToggleKey(string key, bool state)
    {
        //Generic iterator
        byte i=0;

        //Iterate through all keys in the arrary
        foreach (string k in keys)
        {
            //If the input key matches one of those keys
            if (key == k)
            {
                //Get that key's highlight child
                GameObject hi = btn[i].transform.Find("Highlight").gameObject;
                if (hi)
                {
                    //Changes its state
                    hi.SetActive(state);
                }
            }

            //Increment index
            i++;
        }
    }
    */
}