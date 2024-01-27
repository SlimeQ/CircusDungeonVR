using UnityEngine;
using UnityEngine.UI;

public class RedTween : TweenBase
{
    public float from;
    public float to;

    //!@
    private MeshRenderer MR;                        //Mesh Renderer to change (if any)
    public Material redTween_mat;                   //Material ref to use for redTweening (should be EnemyBullet red palcycle)
    public Material[] mats;                         //A cache of mats to apply as the redTween_mat
    private Material[] oldMat = new Material[2];    //The previous material before redTweening
    private bool doRedTween_MR_runonce = false;     //A runonce to init the MR material swap
    private bool repeat = false;                    //Repeat the tween (MeshRenderer param)

    public Color clr_start = Color.white;           //Start color
    public Color clr_end = Color.red;               //End color
    private SpriteRenderer SR;                      //SpriteRenderer to change (if any)
    private MaskableGraphic image;                  //Image component
    private CanvasGroup group;                      //Canvas group component

    void Start()
    {
        //Get all components if exist
        group = GetComponent<CanvasGroup>();
        image = GetComponent<MaskableGraphic>();
        SR = GetComponent<SpriteRenderer>();
        MR = GetComponent<MeshRenderer>();

        //If MR
        if (MR)
        {
            //Get array length of materials, create oldMat with array length, cache current MR mats into oldMat array
            byte max = (byte)(MR.materials.Length);
            oldMat = new Material[max];
            oldMat = MR.materials;
        }
    }

    void FixedUpdate()
    {
        Color c = Color.black;  //Generic color reg
        float p = 0f;           //Lerped value P
        
        //Return if not playing
        if (!isPlaying)
            return;

        switch (playbackDirection)
        {
            case PlaybackDirection.FORWARD:
                value += Time.unscaledDeltaTime / playbackTime;

                if (value < 1f)
                {                   
                    p = Mathf.Lerp(from, to, curve.Evaluate(value));    //Get lerped value p
                    c = Color.Lerp(clr_start,clr_end,p);                //Lerp between start and end colors by p

                    //Process the coloring based on available colorable component
                    if (group)
                        group.alpha = Mathf.Lerp(from, to, curve.Evaluate(value));
                    else
                    {
                        if (SR)
                        {                            
                            SR.color = c;
                        }
                        else if (image)
                        {
                            image.color = c;
                        }
                        else if (MR)
                        {
                            //Special routien for handling MR
                            HandleRedTween_MR(true);
                        }
                    }
                }
                else
                {
                    //Special processing for MR
                    if (MR)
                    {
                        if (!repeat)
                        {
                            //If no repeat, stop playing tween
                            isPlaying = false;
                            StopTween(true);
                        }
                        else
                        {
                            //If repeating, yousa isPlaying, and reset value back to 0f for repeat
                            isPlaying = true;
                            value = 0f;                            
                        }
                        break;
                    }
                    else
                    {
                        isPlaying = false;
                    }

                    //Get new color via lerp
                    p = Mathf.Lerp(from, to, curve.Evaluate(1f));
                    c = Color.Lerp(clr_start, clr_end, p);

                    //Process the coloring based on available colorable component
                    if (group)
                    {
                        group.alpha = Mathf.Lerp(from, to, curve.Evaluate(1f));
                    }
                    else
                    {
                        if (SR)
                        {
                            SR.color = c;
                        }
                        else if (image)
                        {
                            image.color = c;
                        }
                    }
                    OnReset();  //Reset stuff
                }
                break;

            case PlaybackDirection.BACKWARD:
                value += Time.unscaledDeltaTime / playbackTime;

                if (value < 1f)
                {                                        
                    p = Mathf.Lerp(from, to, curve.Evaluate(value));    //Get lerped value p
                    c = Color.Lerp(clr_start, clr_end, p);              //Lerp between start and end colors by p

                    //Process the coloring based on available colorable component
                    if (group)
                    {
                        group.alpha = Mathf.Lerp(to, from, curve.Evaluate(value));
                    }
                    else
                    {
                        if (SR)
                        {
                            SR.color = c;
                        }
                        else if (image)
                        {
                            image.color = c;
                        }
                        else if (MR)
                        {
                            HandleRedTween_MR(true);
                        }
                    }
                }
                else
                {
                    if (MR)
                    {
                        if (!repeat)
                        {
                            //If no repeat, stop playing tween
                            isPlaying = false;
                            StopTween(true);
                        }
                        else
                        {
                            //If repeating, yousa isPlaying, and reset value back to 0f for repeat
                            isPlaying = true;
                            value = 0f;
                        }
                        break;
                    }
                    else
                    {
                        isPlaying = false;
                    }

                    p = Mathf.Lerp(from, to, curve.Evaluate(1f));
                    c = Color.Lerp(clr_start, clr_end, p);

                    if (group)
                    {
                        group.alpha = Mathf.Lerp(to, from, curve.Evaluate(1f));
                    }
                    else
                    {
                        if (SR)
                        {
                            SR.color = c;
                        }
                        else if (image)
                        {
                            image.color = c;
                        }
                    }
                    OnReset();  //Do reset stuff
                }
                break;
        }
    }

    /// <summary>
    /// Play the Tween forward (MeshRenderer overload).
    /// </summary>
    /// <param name="index">Index from material array to use for the matFX color tween</param>
    /// <param name="time">playback time of the tween. Negative values will enable repeat</param>
    public void PlayForward(byte index, float time)
    {
        StopTween(true);    //Stop any lingering tweens

        if (time < 0f)
        {
            //If the time <=0f, set playback time and repeat flag
            playbackTime = Mathf.Abs(time);
            repeat = true;
        }
        else
        {
            playbackTime = time;
            repeat = false;
        }
        redTween_mat = mats[index]; //Set the matFX material to use
        base.PlayForward();         //Play tween forward
    }

    public override void PlayForward()
    {
        base.PlayForward();
        return;
    }
    public override void PlayBackward()
    {
        base.PlayBackward();
        return;
    }

    /// <summary>
    /// Play the Tween backward (MeshRenderer overload).
    /// </summary>
    /// <param name="index">Index from material array to use for the matFX color tween</param>
    /// <param name="time">playback time of the tween. Negative values will enable repeat</param>
    public void PlayBackward(byte index, float time)
    {
        StopTween(true);
        if (time < 0f)
        {
            playbackTime = 1f;
            repeat = true;
        }
        else
        {
            playbackTime = time;
            repeat = false;
        }
        redTween_mat = mats[index];
        base.PlayBackward();
    }

    public override void StopTween(bool reset)
    {
        if (MR)
        {
            //Stop the red tween for MeshRenderer
            HandleRedTween_MR(false);
        }
        else
        {
            base.StopTween(reset);
        }
    }

    public override void OnReset()
    {
        if (SR)
        {
            SR.color = Color.white;
        }
        else if (image)
        {
            image.color = Color.white;
        }
        else if (MR)
        {
            //Stop the red tween for MeshRenderer
            HandleRedTween_MR(false);
        }
    }

    /// <summary>
    /// Handles the RedTween for the MeshRenderer
    /// </summary>
    /// <param name="action">Action to take. False = reset/restore oldMat, true=handle normal tweening</param>
    private void HandleRedTween_MR(bool action)
    {
        byte i = 0;                                 //Generic iterator

        if (action == false)
        {
            //If false flag, reset runonce and restore oldMat
            doRedTween_MR_runonce = false;
            MR.materials = oldMat;
        }
        else
        {
            //If true flag
            if (doRedTween_MR_runonce == false)
            {                
                //If the runonce flag, set flag
                doRedTween_MR_runonce = true;
                
                byte max = (byte)(MR.materials.Length); //Max amount of materials
                Material[] ma = new Material[max];      //Dim new material array with amount of materials

                //Iterate through all materials
                for (i = 0; i < max; i++)
                {                    
                    ma[i] = redTween_mat;   //Set all mats to the redTween_mat
                }
                MR.materials = ma;          //Set the MeshRenderer's material array to the new one.
            }
        }
    }
}