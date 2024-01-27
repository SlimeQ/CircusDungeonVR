using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class defines a CutScenePanel object
/// </summary>
public class CutScenePanel : MonoBehaviour
{
    public Image byIcon;            //Icon image
    public Text byName;             //Name text
    public Text expressionLabel;    //Expression text
    public GameObject nextMarker;   //Next gameobject indicator

    [NamedArrayAttribute(typeof(Characters))]
    public GameObject[] CutSceneHealthBars = new GameObject[(byte)(Characters.MAX)+1];
}
