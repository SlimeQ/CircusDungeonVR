using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Probably !@Dead Coffee Code
/// <summary>
/// A script that will en/disable certain UI elements based on if they meant for 4:3/16:9 screen resolutions
/// </summary>
public class AnchorAdj : MonoBehaviour
{
    public Camera main;                 //Main camera ref
    
    /// <summary>
    /// Screen resolution enums
    /// </summary>
    public enum screenres
    {
        FOURTHREE,                      //4:3
        SIXTEENNINE                     //16:9
    }

    public screenres elementtype;       //Type of element
    public Text FourThreeText;          //Text on the 4:3 version
    private bool ready = false;         //Script ready?

    void Start()
    {
        StartCoroutine(Init());
    }

    /// <summary>
    /// Init script
    /// </summary>
    /// <returns>Yield</returns>
    private IEnumerator Init()
    {
        yield return new WaitForSeconds(.1f);   //Waits .1f seconds
        ready = true;                           //We are ready!
    }
	
	/// <summary>
	/// Toggles this UI element based on the screenres
	/// </summary>
	void Update()
    {
        //If not ready skip
        if (ready == false)
        {
            return;
        }

        float rat;                              //Screenrez as float
        if (!main)
        {
            rat = Camera.main.aspect;           //If not main camera, set rat as main camera aspect
        }
        else
        {
            rat = main.aspect;                  //Otherwise use mainref's aspect
        }
       
        bool state = false;                     //Active state
        screenres res;                          //Screenres enum

        if (rat >= 1.7f)
        {
            //If rat>=1.7f (16/9=1._77_ barnotated), then the res is 16:9
            res=screenres.SIXTEENNINE;
        }
        else
        {
            //Otherwise 4:3
            res=screenres.FOURTHREE;
        }

        state = (elementtype == res);                                   //Get proper state
        this.gameObject.GetComponent<Text>().text = FourThreeText.text; //Set the text to the 4:3 version (especially if 16:9 version)
        this.gameObject.SetActive(state);                               //Toggle the enable state
	}
}
