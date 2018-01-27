using UnityEngine;

[RequireComponent(typeof(Controller2D))] //Autimaticamente adiciona o Controller2D ao objeto e não permite que retire
public class Player : MonoBehaviour {

    float gravity = -20;
    float moveSpeed = 6;

    Vector3 velocity;

    Controller2D controller;

	void Start () {
        controller = GetComponent<Controller2D>();		
	}

    void Update () {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity.x = input.x * moveSpeed;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
	}
}
