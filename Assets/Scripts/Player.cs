using UnityEngine;

[RequireComponent(typeof(Controller2D))] //Autimaticamente adiciona o Controller2D ao objeto e não permite que retire
public class Player : MonoBehaviour {

    Controller2D controller;

	void Start () {
        controller = GetComponent<Controller2D>();		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
