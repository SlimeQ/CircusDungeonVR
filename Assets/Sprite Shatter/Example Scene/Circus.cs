using UnityEngine;
using System.Collections;
using SpriteShatter;

public class Circus : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//If the user clicks the left mouse button, explode the circus!
		if (Input.GetMouseButtonDown(0))
			GetComponent<Shatter>().shatter();

		//If the user clicks the right mouse button, reset the circus!
		else if (Input.GetMouseButtonDown(1))
			GetComponent<Shatter>().reset();
	}
}
