using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles status player images, wrt 3D beaks and 2D images w/o beaks
/// </summary>
public class StatusScreen : MonoBehaviour
{
    //Image icons for each char
    [NamedArrayAttribute(typeof(Characters))]
    public Image[] Icons = new Image[(byte)(Characters.MAX) + 0x01];

    //Sprite to use for the icons (2D unbeaked)
    [NamedArrayAttribute(typeof(Characters))]
    public Sprite[] UBeak2D = new Sprite[(byte)(Characters.MAX)+0x01];

    //Sprite to use for the icons (2D beaked)
    [NamedArrayAttribute(typeof(Characters))]
    public Sprite[] Beak2D = new Sprite[(byte)(Characters.MAX) + 0x01];

    //Gameobjects to use for 3D beaks
    [NamedArrayAttribute(typeof(Characters))]
    public GameObject[] Beak3D = new GameObject[(byte)(Characters.MAX) + 0x01];
    
    /// <summary>
    /// Toggles the images/gameobjects appropriate based on the Canvas mode
    /// </summary>
    /// <param name="c">Canvas to test</param>
    public void ToggleMode(Canvas c)
    {
        bool camera = (c.renderMode == RenderMode.ScreenSpaceCamera);
        foreach (GameObject g in Beak3D)
        {
            if (g != null)
            {
                g.SetActive(camera);
            }
        }
        ToggleImages(!camera);
    }

    /// <summary>
    /// Changes the type of sprites for character icons
    /// </summary>
    /// <param name="beaked">Beaked sprites?</param>
    private void ToggleImages(bool beaked)
    {
        byte i = 0;
        foreach (Image I in Icons)
        {
            if (I != null)
            {
                if (beaked)
                {
                    I.sprite = Beak2D[i];
                }
                else
                {
                    I.sprite = UBeak2D[i];
                }
            }
            i++;
        }
    }
}