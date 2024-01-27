using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Script to move credits GUItext upwards
/// </summary>
public class Credits : MonoBehaviour
{
    public Localization LZ;                 //Localization script
    public LoadScene LS;                    //LoadScene script
    public AlphaTween[] AT;                 //Alpha Tween scrips to tween in/out stuff
    public float SpeedY = 40f;              //Current Vert movement speed
    private const float SpeedYMax = 100f;   //Max abs(vert) movement speed
    public float minY = -40f;               //Min y for credits. !@ Needs modified
    public float maxY = 2350f;              //Max y for credits. !@ Needs modified
    private bool ExitRunonce = false;       //A runonce to prevent spamming of credits exit
    GameObject obj;                         //GameObject script is attached to
    Vector2 pos;                            //Position of the Text script holding this credits text

    private void Start()
    {
        //Localize the text
        Text T = this.gameObject.GetComponent<Text>();
        if ((T) && (LZ))
        {
            T.text = LZ._Credits();
        }
        obj = this.gameObject;                                                  //Get this Gameobject
        pos = obj.GetComponent<RectTransform>().anchoredPosition;               //Get this object's pos
        PlayerControls controls = gameObject.AddComponent<PlayerControls>();    //Add a PlayerControls script
        //controls.LoadPlayer1Setup(true);
        controls.LoadPlayer1Setup();                                            //LoadPlayer 1 controls
        
        //sign up for events
        controls.PitchYawAxis += ChangeSpeed;                                    //Change vert speed on controlaxis change
        controls.onStart += ExitCredits;                                        //Exit credits on Start
    }

    //Move the GUIText
    private void Update()
    {
        //Debug.Log(pos.y.ToString());
        //Debug.Log("posy: "+pos.y.ToString());

        float newval = pos.y + SpeedY * Time.deltaTime;                         //New ypos
        if (newval > minY && newval < maxY)
        {
            //If ypos is withing range

            //Update the ypos
            pos.y = newval;
            Vector2 newpos = new Vector2(pos.x, pos.y);
            obj.GetComponent<RectTransform>().anchoredPosition = newpos;
        }
        else if (newval >= maxY)
        {
            //If exceeed max Y, exit credits
            ExitCredits();
        }
        //If below minY, do nothing
    }

    /// <summary>
    /// Changes the speed
    /// </summary>
    /// <param name="axis">InControl DirStick axis from cawback</param>
    public void ChangeSpeed(Vector2 axis)
    {
        if (axis.y < -0.1f)
        {
            //If down, decrease speed
            SpeedDec();
        }
        else if (axis.y > 0.1f)
        {
            //Else up increase speed
            SpeedInc();
        }
        //No movement do nothing
    }

    /// <summary>
    /// Exit the Credits scene
    /// </summary>
    public void ExitCredits()
    {
        //Don't exit if runonce ran
        if (ExitRunonce == true)
        {
            return;
        }
        ExitRunonce = true;             //Set the runonce

        //Alpha tween out everything
        foreach (AlphaTween a in AT)
        {
            a.PlayBackward();
        }
        //Load the new scene (Main Menu/whatever)
        LS.LoadSceneDelayed(1.0f);
    }

    /// <summary>
    /// Increase the scrolling speed
    /// </summary>
    public void SpeedInc()
    {
        Debug.Log("Increasing credits speed");
        SpeedY += 5f;
        SpeedY=Mathf.Clamp(SpeedY, -SpeedYMax, SpeedYMax);
    }

    /// <summary>
    /// Decrease the scrolling speed
    /// </summary>
    public void SpeedDec()
    {
        Debug.Log("Decreasing credits speed");
        SpeedY -= 5f;
        SpeedY = Mathf.Clamp(SpeedY, -SpeedYMax, SpeedYMax);
    }
}