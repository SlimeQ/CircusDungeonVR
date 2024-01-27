using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
/// <summary>
/// Scales a child text component with its parent. Add this script to text obj
/// Example: Button -> text element. Scale button, text scales appropriately in editor/during runtime
/// </summary>
public class ScaleTextWithParent : MonoBehaviour
{
    public bool ExeInEditMode = false;              //Run this in edit mode
    private RectTransform T;                        //RectTransform of this text comp
    private bool ready = false;                     //Ready?
    private Vector2 oldSizeDelta = Vector2.zero;    //Previous T sizeDelta, to detect delta changes
    
    // Start is called before the first frame update
    public void Start()
    {        
        //Get the RT from this gameobject, then we are ready
        T = this.gameObject.GetComponent<RectTransform>();
        if (T)
        {
            ready = true;
        }
    }

    private void LateUpdate()
    {
        //If not ready, return
        if (!ready)
        {
            return;
        }

        //if in editor and editMode is disabled, return
        #if UNITY_EDITOR
        if (ExeInEditMode == false)
        {
            return;
        }
        #endif

        //If T.sizeDelta != previous value, then a change occurred
        bool changed = (T.sizeDelta != oldSizeDelta);
        if(changed)
        {        
            //If a changed occurred

            //Get the parent of this gameObject
            Transform parent = this.gameObject.transform.parent;
            if (parent)
            {
                //Get the parent's RT
                RectTransform RT = parent.gameObject.GetComponent<RectTransform>();
                if (RT)
                {
                    //Apply this sizeDelta to the parent's RT
                    T.sizeDelta = RT.sizeDelta;
                }
            }
        }

        //Cache old sizeDelta for further comparisons
        oldSizeDelta = T.sizeDelta;
    }
}