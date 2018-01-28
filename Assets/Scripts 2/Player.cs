using UnityEngine;

[RequireComponent(typeof(Controller2D))] //Autimaticamente adiciona o Controller2D ao objeto e não permite que retire
public class Player : MonoBehaviour {

    public float jumpHeight = 4; //Altura máxima que o player pode pular
    public float timeToJumpApex = .4f; //Tempo que leva para alcançar a altura máxima
    float accelerationTimeAirborne = .2f; //Aceleração no Ar
    float accelerationTimeGrounded = .1f; //Aceleração no chão
    float gravity;
	SpriteRenderer rend;
    float jumpVelocity;

    float moveSpeed = 6;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

	void Start () {
        controller = GetComponent<Controller2D>();
		rend = GetComponent<SpriteRenderer>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2); //S = S0 + V0*t + (a * t^2)/2
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex; // V = Vo + a*t;
    }

    void Update () {

        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }
		
		rend.flipX = input.x > 0;

        float targetVelocityX = input.x * moveSpeed;
        /* Suaviza o movimento na direção X
         * Se o player estiver no chão, o tempo para acelerar é menor
         */
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, 
            (controller.collisions.below)? accelerationTimeGrounded:accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
	}
}
