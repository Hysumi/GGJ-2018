using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetter : MonoBehaviour {

	public int width, height;
	// Use this for initialization
	void Awake () {
		int mult = Screen.currentResolution.height/height;
		
		Screen.SetResolution(width * mult, height * mult, false);
		
	}
	

}
