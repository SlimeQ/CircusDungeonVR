

using UnityEngine;
using System.Collections;

public class LittlePathTween : TweenBase
{
    //!@ Get original version from Vadym! This script was heavily modified for Coffee usage
    /*
    public float xScale = 1f;
    public float yScale = 1f;
    public bool isPlayerFling = false;   //If set to true (for player's flingTween), tweak invuln flag
    public bool isEnemyFling = false;
    public bool isOtherTween = false;
    public bool runScaleTween = false;

    public float xv=0f;
    private CharacterControllerCoffee C3;   //C3 component of player (isPlayerFlign==true), if any
    public GameObject Shadow;
    private flingBugFix FBX;
    private Vector3 origin;
    private Vector3 originShad;
    private Vector3 originShadSc;
    private Rigidbody2D rigidbody2d;
    //private bool runBack = false;
    //private ScaleTween ST;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        if (Shadow)
        {
            if((isEnemyFling == true)||(isPlayerFling))
            {
                //Get enemy's or player's FBX
                FBX = Shadow.transform.GetComponentInChildren<flingBugFix>();
            }
        }
    }

    void Update()
    {        
        if (!isPlaying || IngameCameraControls.instance.levelComplete)
        {
            xv = 0f;
            return;
        }
           
        float y = 0f;
        float vMin = .5f;
        float vMax = 1f;

        if((isEnemyFling)&&(!isOtherTween))
        {
            vMin=.75f;
            vMax=1.5f;
        }

        switch (playbackDirection)
        {            
            case PlaybackDirection.FORWARD:
                value += Time.deltaTime / playbackTime;

                if (FBX)
                {
                    if (FBX.collision == false)
                    {
                        xv = value * xScale * multFact;
                    }
                }
                else
                {
                    xv = value * xScale * multFact;
                }
                y=curve.Evaluate(value) * yScale;

                //if ((isPlayerFling) || (isEnemyFling))
                //{                    
                //    if (value <= vMin)
                //    {
                            
                //        if ((ST)&&(runScaleTween==true))
                //        {
                //            if (runBack == false)
                //            {
                //                runBack = true;
                //                ST.PlayBackward();
                //            }
                //        }                            
                //    }                    
                //}

                if (value < vMax)
                {
                    if (isPlayerFling == false)
                    {
                        if (rigidbody2d)
                            rigidbody2d.MovePosition(origin + new Vector3(xv, y));
                        else
                            transform.localPosition = origin + new Vector3(xv, y);
                    }
                    else
                    {
                        transform.localPosition = origin + new Vector3(xv, y);
                    }

                    if ((isPlayerFling) || (isEnemyFling))
                    {
                            Vector2 newpos = new Vector2(Shadow.transform.localPosition.x, originShad.y - y);
                            Shadow.transform.localPosition = newpos;
                    }
                }
                else
                {
                    isPlaying = false;                    

                    if(isPlayerFling==false)
                    {
                        if (rigidbody2d)
                            rigidbody2d.MovePosition(origin + new Vector3(xv, y));
                        else
                            transform.localPosition = origin + new Vector3(xv, y);
                    }
                    else
                    {
                        transform.localPosition = origin + new Vector3(xv, y);
                    }

                    if ((isPlayerFling) || (isEnemyFling))
                    {
                            Vector2 newpos = new Vector2(Shadow.transform.localPosition.x, originShad.y - y);
                            Shadow.transform.localPosition = newpos;
                    }

                    StopCoroutine(TogglePlayer(false));
                    StartCoroutine(TogglePlayer(false));  //Remove movelock if applicable
                    StopCoroutine(ToggleEnemy(true));
                    StartCoroutine(ToggleEnemy(true));    //Re-enable pickup box if appllicable
                }
                break;

            case PlaybackDirection.BACKWARD:
                value += Time.deltaTime / playbackTime;
                if (FBX)
                {
                    if (FBX.collision == false)
                    {
                        xv = -value * xScale * multFact;
                    }
                }
                else
                {
                    xv = -value * xScale * multFact;
                }
                y = curve.Evaluate(value) * yScale;

                //if ((isPlayerFling) || (isEnemyFling))
                //{
                //        if (value <= vMin)
                //        {
                //            if ((ST)&&(runScaleTween==true))
                //            {
                //                if (runBack == false)
                //                {
                //                    runBack = true;
                //                    ST.PlayBackward();
                //                }
                //            }
                //        }
                //}

                if (value < vMax)
                {                    

                    if(isPlayerFling==false)
                    {
                        if (rigidbody2d)
                            rigidbody2d.MovePosition(origin + new Vector3(xv, y));
                        else
                            transform.localPosition = origin + new Vector3(xv, y);
                    }
                    else
                    {
                        transform.localPosition = origin + new Vector3(xv, y);
                    }

                    if ((isPlayerFling) || (isEnemyFling))
                    {
                            Vector2 newpos = new Vector2(Shadow.transform.localPosition.x, originShad.y - y);
                            Shadow.transform.localPosition = newpos;
                    }
                }
                else
                {
                    isPlaying = false;

                    if (isPlayerFling == false)
                    {
                        if (rigidbody2d)
                            rigidbody2d.MovePosition(origin + new Vector3(xv, y));
                        else
                            transform.localPosition = origin + new Vector3(xv, y);
                    }
                    else
                    {
                        transform.localPosition = origin + new Vector3(xv, y);
                    }

                    if ((isPlayerFling) || (isEnemyFling))
                    {
                            Vector2 newpos = new Vector2(Shadow.transform.localPosition.x, originShad.y - y);
                            Shadow.transform.localPosition = newpos;
                    }

                    StopCoroutine(TogglePlayer(false));  //Remove movelock if applicable
                    StartCoroutine(TogglePlayer(false));  //Remove movelock if applicable
                    StopCoroutine(ToggleEnemy(true));
                    StartCoroutine(ToggleEnemy(true));    //Re-enable pickup box if appllicable
                }
                break;
        }
    }

    public override void PlayForward()
    {
        StopCoroutine(TogglePlayer(true));
        StartCoroutine(TogglePlayer(true));   //Set movelock if applicable
        StopCoroutine(ToggleEnemy(false));
        StartCoroutine(ToggleEnemy(false));   //Disable pickup box if applicable

        if (rigidbody2d)
        {
            origin = transform.position;            
        }
        else
        {
            origin = transform.localPosition;
        }
        base.PlayForward();
    }

    public override void PlayBackward()
    {
        StopCoroutine(TogglePlayer(true));   //Set movelock if applicable
        StartCoroutine(TogglePlayer(true));   //Set movelock if applicable
        StopCoroutine(ToggleEnemy(false));   //Disable pickup box if applicable
        StartCoroutine(ToggleEnemy(false));   //Disable pickup box if applicable

        if (rigidbody2d)
            origin = transform.position;
        else
            origin = transform.localPosition;

        base.PlayBackward();
    }

    /// <summary>
    /// For flingtweens that have moveLock set (specifically C3 scripts for player), give player invuln and other special processing
    /// </summary>
    /// <param name="state">Toggle</param>
    /// <param name="dir">Movement direction (false=playBackwards, true=PlayForwards)</param>
    /// <returns></returns>
    private IEnumerator TogglePlayer(bool state)
    {
        if (isPlayerFling == true)
        {
            //Get C3 script
            C3 = this.gameObject.GetComponent<CharacterControllerCoffee>();
            if (C3)
            {                
                if (state == true)
                {
                    GameManager.spawnSmoke(C3.transform.position + new Vector3(0.0f, -0.2f, 0.0f));
                    C3.flingLock = true;
                    C3.atkHeld = false;         //Disable attack held+chargeLock to prevent issues
                    C3.isChargeLocked = false;
                    C3.isInvincible = state;    //Change invuln state
                    originShad = Shadow.gameObject.transform.localPosition;
                    originShadSc = Shadow.gameObject.transform.localScale;
                    TogglePlayerWalkCol(state); //Toggle the WalkCollider off
                    C3.renderer2d.color = new Color(1f, 1f, 1f, .5f);   //Manually set semi trans, due to effing isInvuln semi trans code not working(???)
                    //runBack = false;
                    //ST = Shadow.gameObject.GetComponent<ScaleTween>();
                    //if ((ST) && (runScaleTween == true))
                    //{   
                    //    ST.PlayForward();
                    //}
                }
                else
                {
                    C3.flingLock = false;   //Fling is done; reset flingLock
                    Shadow.transform.localPosition = originShad;
                    Shadow.gameObject.transform.localScale = originShadSc;
                    TogglePlayerWalkCol(state);             //Toggle the WalkCollider on
                    GameManager.spawnSmoke(C3.transform.position + new Vector3(0.0f, -0.2f, 0.0f));
                    yield return new WaitForSeconds(.5f);   //Yield .5 seconds on landing before removing invin
                    C3.isInvincible = state;    //Change invuln state
                    C3.renderer2d.color = new Color(1f, 1f, 1f, 1f);   //Manually set semi trans, due to effing isInvuln semi trans code not working(???)
                }                
				
            }
        }
    }

    /// <summary>
    /// Toggles the player's WalkCollider
    /// </summary>
    /// <param name="state">state of TogglePlayer</param>
    private void TogglePlayerWalkCol(bool state)
    {
        GameObject walk = Shadow.gameObject.transform.GetChild(0).gameObject;   //Get the WalkCollider gameobject
        if (walk)
        {
            CapsuleCollider2D cap = walk.gameObject.GetComponent<CapsuleCollider2D>();  //Get its capsuleCollider
            if (cap)
            {
                //Toggle it as necessary
                cap.enabled = (!state);
            }
        }
    }

    private IEnumerator ToggleEnemy(bool state)
    {
        if (isEnemyFling==true)
        {            
            Enemy en = this.gameObject.GetComponent<Enemy>();            
            if (en)
            {
                SpriteRenderer shadSR = Shadow.gameObject.GetComponent<SpriteRenderer>();
                bool hasFling = en.anim.HasParameter("Flung");
                if (isOtherTween == false)
                {                    
                    if (hasFling)
                    {
                        en.anim.SetBool("Flung", !state);
                    }
                }

                //en.TogglePickupTrig(state);
                if (state == false)
                {
                    //If the enemy has an animation to play after getting flung N times (esp Skinny), do stuff
                    if (en.flingCounterAnim != "")
                    {
                        //Get the enemy's EA component
                        EnemyAttack EA = en.GetComponent<EnemyAttack>();
                        if (EA)
                        {
                            //Increment the flingCounter
                            StopCoroutine(EA.handleflingCounter(-1));
                            StartCoroutine(EA.handleflingCounter(-1));
                        }
                    }
                        originShad = Shadow.gameObject.transform.localPosition;
                        originShadSc = Shadow.gameObject.transform.localScale;

                        en.SR.sortingOrder = 0;
                        if (shadSR)
                        {
                            shadSR.sortingOrder = 0;
                        }

                        //ST = Shadow.gameObject.GetComponent<ScaleTween>();
                        //runBack = false;
                        //if ((ST) && (runScaleTween == true))
                        //{
                        //    ST.PlayForward();
                        //}
                        GameManager.spawnSmoke(en.shadowTransform.position + new Vector3(0.0f, 0.2f, 0.0f));
                }
                else
                {
                    if (isOtherTween == false)
                    {
                        if (hasFling)
                        {
                            bool isGuitar = false;
                            bool isGuitar2 = false;
                            GameManager.spawnSmoke(en.shadowTransform.position + new Vector3(0.0f, 0.2f, 0.0f));    //Spawn smoke
                            isPlaying = false;
                            byte i = 0;
                            for (i = 0; i < 23; i++)
                            {
                                en.moveLock = true;                                                                     //Lock enemy movement

                                if (en.DmgCollider)
                                {
                                    isGuitar=false;
                                    isGuitar2 = false;
                                    switch (en.DmgCollider.type)
                                    {
                                        case(Damage.enemy_t.ET_BOSS_AOS):
                                        case(Damage.enemy_t.ET_BOSS_MHEAD):
                                            isGuitar = true;
                                            break;
                                        case(Damage.enemy_t.ET_BOSS_COUNTRY):
                                            isGuitar2 = true;
                                            break;
                                    }

                                    if((isGuitar)||(isGuitar2))
                                    {
                                        //If Skinny and flung, stop all movement during this NOP loop. Bugfix
                                        en.rigidbody2d.velocity = Vector2.zero;
                                    }
                                }
                                yield return new WaitForSeconds(.1f);                                                  //Stall
                                if (en.die == false)
                                {
                                    //!@ ChangeShadow anim call in Getup anim isn't working for flings, so it's functionality is hardcoded here
                                    if (i < 11)
                                    {                                        
                                        if((shadSR)&&(isGuitar2==false))
                                        {
                                            shadSR.sortingOrder = -10;
                                            en.SR.sortingOrder = -10;
                                        }
                                        Shadow.transform.localPosition = en.shadDeathpos;
                                        Shadow.gameObject.transform.localScale = en.shadDeathscale;
                                        en.walkCollider.gameObject.transform.localScale = new Vector3(en.BoxScaleOrigin.x / en.shadDeathscale.x, en.BoxScaleOrigin.y / en.shadDeathscale.y, 1f);
                                    }
                                    else
                                    {                                        
                                        if ((shadSR) && (isGuitar2 == false))
                                        {
                                            en.SR.sortingOrder = 0;
                                            shadSR.sortingOrder = 0;
                                        }
                                        try
                                        {
                                            Shadow.transform.localPosition = originShad;
                                            Shadow.gameObject.transform.localScale = originShadSc;
                                            en.walkCollider.gameObject.transform.localScale = en.BoxScaleOrigin;
                                        }
                                        catch
                                        {
                                            //IF NRE, catch
                                            Debug.Log("Shadow NRE");
                                        }
                                    }
                                }
                            }

                            if (en.die == false)
                            {
                                en.ResetInterrupts();               //Before unsetting pickedup state, reset all interrupts. Bugfix to prevent interrupts post-getup
                                en.anim.SetBool("PickedUp", false); //Getup
                            }
                            en.moveLock = false;   //Unlock enemy movement
                        }
                    }
                    else
                    {
                        Shadow.transform.localPosition = originShad;
                        Shadow.gameObject.transform.localScale = originShadSc;
                    }

                    //If the enemy has an animation to play after getting flung N times (esp Skinny), do stuff
                    if (en.flingCounterAnim != "")
                    {
                        EnemyAttack EA = en.GetComponent<EnemyAttack>();
                        if (EA)
                        {
                            //Check for fling target met
                            StopCoroutine(EA.handleflingCounter(1));
                            StartCoroutine(EA.handleflingCounter(1));
                        }
                    }
                }				
				//Debug.Log ("SPAWNING SMOKE (FLING)");
            }
            yield break;
        }
    }
    */
}
