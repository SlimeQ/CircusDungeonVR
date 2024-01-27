using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Handles changing an SR material in animation events (by toggling this script on/off, like with 2DXFX shader scripts)
/// </summary>
public class AnimEventShader : MonoBehaviour
{
    public SpriteRenderer SR;                   //This SR
    public Material mat;                        //New material to apply
    public Material defMat;                     //Default material
    public bool preventDisable = false;         //Prevent disabling new material OnDisable?

	void Update()
	{
        //When enabled, use newmat all the time
        SR.material = mat;
	}
	
    /// <summary>
    /// Restore defMat onDestroy
    /// </summary>
	void OnDestroy()
	{
        SR.material = defMat;
	}
	
    /// <summary>
    /// If possible, restore default material on disable
    /// </summary>
    void OnDisable()
	{
        if (preventDisable == false)
        {
            SR.material = defMat;
        }
	}
	
    /// <summary>
    /// Use new mat onEnable
    /// </summary>
	void OnEnable()
	{
        SR.material = mat;
	}
}