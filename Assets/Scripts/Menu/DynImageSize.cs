using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Helper script which will always update the RectTransform width/height values for a center-aligned image based on sprite image
/// </summary>
public class DynImageSize : MonoBehaviour
{
    public Image img;               //The image to update size
    private bool ready = false;     //Ready semaphore
    void Start()
    {
        //If no image, get image component
        if (!img)
        {
            img = this.gameObject.GetComponent<Image>();
        }
        //We are ready!
        ready = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            //If image has a sprite, update rect transform based on the sprite's size
            if (img.sprite)
            {
                try
                {
                    float x = img.rectTransform.rect.x;
                    float y = img.rectTransform.rect.y;
                    float w = img.sprite.rect.width;
                    float h = img.sprite.rect.height;
                    Rect r = img.rectTransform.rect;
                    r.width = w;
                    r.height = h;
                    img.rectTransform.rect.Set(x, y, w, h);
                }
                catch { }
            }
        }
    }
}