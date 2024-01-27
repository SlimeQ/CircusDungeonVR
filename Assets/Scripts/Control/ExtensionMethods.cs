using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;
//using Rewired;

/// <summary>
/// Class of various extension methods
/// </summary>
public static class ExtensionMethods
{
    // https://forum.unity3d.com/threads/change-gameobject-layer-at-run-time-wont-apply-to-child.10091/
    /// <summary>
    /// Sets the layer for an object and all of its children, except those with the name of exception string
    /// </summary>
    /// <param name="obj">this object</param>
    /// <param name="layer">layer to change to</param>
    /// <param name="exception">Child name to ignore</param>
    public static void SetLayerRecursively(this GameObject obj, int layer, string exception)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            if (child.name != exception)
            {
                child.gameObject.SetLayerRecursively(layer, exception);
            }
        }
    }

    /// <summary>
    /// Normalizes any float to an arbitrary range 
    /// by assuming the range wraps around when going below min or above max 
    /// https://stackoverflow.com/a/2021986
    /// </summary>
    /// <param name="value">this float value</param>
    /// <param name="start">Start float range</param>
    /// <param name="end">End float range</param>
    /// <returns>Normlized float value</returns>
    public static float normalise(this float value, float start, float end ) 
    {
        
        float width = end - start   ;   // 
        float offsetValue = value - start ;   // value relative to 0
        float val = ( offsetValue - ( Mathf.Floor( offsetValue / width ) * width ) ) + start;         
        return val;
        // + start to reset back to start of original range
    }

    /// <summary>
    /// Replaces a character at a pos within string
    /// </summary>
    /// <param name="input">This string to modify </param>
    /// <param name="index">Index of char</param>
    /// <param name="newChar">New char at Index</param>
    /// <returns>New, modified string</returns>
    public static string ReplaceAt(this string input, int index, char newChar)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input");
        }
        char[] chars = input.ToCharArray();
        chars[index] = newChar;
        return new string(chars);
    }

    /// <summary>
    /// Check if this animator has a particular param byName
    /// http://answers.unity3d.com/questions/571414/is-there-a-way-to-check-if-an-animatorcontroller-h.html
    /// </summary>
    /// <param name="animator">this Animator</param>
    /// <param name="paramName">Param name to check</param>
    /// <returns>HasParam?</returns>
    public static bool HasParameter(this Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Copies properties from a shader for a particular material to a new material
    /// </summary>
    /// <param name="mat">This dest material</param>
    /// <param name="newMat">Src material</param>
    public static void CopyPropertiesShaderFromMaterial(this Material mat, Material newMat)
    {
        mat.shader = newMat.shader;
        mat.CopyPropertiesFromMaterial(newMat);
    }

    //http://answers.unity.com/answers/1080062/view.html
    /*
    public static void AddResource(this ICollection<Material> materials, string resource)
        {
            Material material = Resources.Load(resource, typeof(Material)) as Material;
            if (material)
                materials.Add(material);
            else
                Debug.LogWarning("Material Resource '" + resource + "' could not be loaded.");
        }
     */

    /*
    /// <summary>
    /// Extension function to lerp a slow/fast speed value on a Rewired.ComponentControls.Effects.RotateAroundAxis component
    /// </summary>
    /// <param name="RAA">this Rewired.ComponentControls.Effects.RotateAroundAxis</param>
    /// <param name="param">Lerp params. param.x & param.y = min/max speedLerp value, .z integer = speed type to modify (part: negative = slow, positive = fast, 0 = currentSpeed being played, .z float part = time division factor for UnityEngine.Time.deltaTime lerpT value</param>
    public static void LerpSpeed(this Rewired.ComponentControls.Effects.RotateAroundAxis RAA, Vector3 param)
    {
        float z = param.z;                          //full z component
        float zInt = (int)(param.z);                //Int part of z
        float zFrac = (Mathf.Abs(z) - zInt);        //Frac part of z
        bool good = float.TryParse(Mathf.Abs(zFrac).ToString().Replace("0.", ""),out zFrac);   //Conv zFrac to string, remove 0. to get string portion, get the new value
        //Debug.Log("zInt, zFrac: " + zInt.ToString() + ", " + zFrac.ToString());
        if (good)
        {
            Vector4 v4param = new Vector4(param.x, param.y, zInt, zFrac);           //Create new Vector4, set .z and .w params appropriately
            GlobalMonoBehaviour.instance.StartCoroutine(_LerpSpeed(RAA, v4param));  //Run lerpSpeed coroutine with hacky GlobalMonoBehaviour
        }
    }
    */

    /*
    /// <summary>
    /// Actually does the speed lerping for LerpSpeed extension function
    /// </summary>
    /// <param name="RAA">Rewired.ComponentControls.Effects.RotateAroundAxis</param>
    /// <param name="param">Lerp params. param.x & param.y = min/max speedLerp value, .z integer = speed type to modify (part: negative = slow, positive = fast, 0 = currentSpeed being played, .z float part = time division factor for UnityEngine.Time.deltaTime lerpT value</param>
    /// <returns>Speed lerping/yield</returns>
    private static IEnumerator _LerpSpeed(Rewired.ComponentControls.Effects.RotateAroundAxis RAA, Vector4 param)
    {
        Vector2 lerp = new Vector2(param.x, param.y);   //Get min/max lerp values as .x and .y of param
        sbyte spd = (sbyte)(Mathf.Sign(param.z));       //Get speedType to change as Mathf.Sign(param.z)
        float DivFactor = param.w;                      //Get division factor as param.w
        
        float timer = 0f;                               //LerpT timer value
        float speed = 0f;                               //New speed to apply

        //While timer is not maxed out
        while (timer <= 1f)
        {
            timer += (UnityEngine.Time.deltaTime / DivFactor);  //Increment timer by (deltaTime/DivFactor)
            speed = Mathf.Lerp(lerp.x, lerp.y, timer);          //Get new speed from the Lerp

            //Apply the new speed to the appropriate speed type
            if (spd == -1)
            {
                //If negative, apply to slow speed
                RAA.slowRotationSpeed = speed;
            }
            else if (spd == 1)
            {
                //If positive, apply to fast speed
                RAA.fastRotationSpeed = speed;
            }
            else
            {
                //If 0, apply to current speed being played
                if (RAA.speed == Rewired.ComponentControls.Effects.RotateAroundAxis.Speed.Slow)
                {
                    //Apply to slow if slow
                    RAA.slowRotationSpeed = speed;
                }
                else if (RAA.speed == Rewired.ComponentControls.Effects.RotateAroundAxis.Speed.Fast)
                {
                    //Apply to fast if fast
                    RAA.fastRotationSpeed = speed;
                }
            }
            yield return new WaitForSeconds(.1f);   //Safety NOP
        }
        yield break;
    }
    */

    /// <summary>
    /// Determines if a object's renderer is within a camera's view
    /// https://wiki.unity3d.com/index.php?title=IsVisibleFrom
    /// </summary>
    /// <param name="renderer">This obj renderer</param>
    /// <param name="camera">Camera to check against</param>
    /// <returns>Visible?</returns>
    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }    

    /// <summary>
    /// For a bool array, ANDI it by a AND bool mask array, and return ByRef
    /// </summary>
    /// <param name="bvar">this bool array</param>
    /// <param name="ANDI_mask">ANDI Mask bool array</param>
    /// <param name="result">Resultant bool array. Returns null if both arrays are not of equal size</param>
    public static void BoolArr_ANDI_Arr(this bool[] bvar, bool[] ANDI_mask, ref bool[] result)
    {
        byte length_bvar = (byte)(bvar.GetLength(0));       //Get lenght of bvar
        byte length_Mask = (byte)(ANDI_mask.GetLength(0));  //Get length of mask

        //If both don't equal, set result to null and then return
        if (length_bvar != length_Mask)
        {
            result = null;
            return;
        }

        //Redim result to length
        result = new bool[length_bvar];
        byte i = 0; //Iterator
        //Iterate through all bytes, do bvar AND Mask and set into result
        for (i = 0; i < length_Mask; i++)
        {
            result[i] = (bvar[i] & ANDI_mask[i]);
        }
    }

    /// <summary>
    /// For a bool array, ORI it by a OR bool mask array, and return ByRef
    /// </summary>
    /// <param name="bvar">this bool array</param>
    /// <param name="ORI_mask">ORI Mask bool array</param>
    /// <param name="result">Resultant bool array. Returns null if both arrays are not of equal size</param>
    public static void BoolArr_ORI_Arr(this bool[] bvar, bool[] ORI_mask, ref bool[] result)
    {
        byte length_bvar = (byte)(bvar.GetLength(0));       //Get lenght of bvar
        byte length_Mask = (byte)(ORI_mask.GetLength(0));  //Get length of mask

        //If both don't equal, set result to null and then return
        if (length_bvar != length_Mask)
        {
            result = null;
            return;
        }

        //Redim result to length
        result = new bool[length_bvar];
        byte i = 0; //Iterator
        //Iterate through all bytes, do bvar AND Mask and set into result
        for (i = 0; i < length_Mask; i++)
        {
            result[i] = (bvar[i] | ORI_mask[i]);
        }
    }
}