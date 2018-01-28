using UnityEngine;

[RequireComponent(typeof(Controller2D))] //Autimaticamente adiciona o Controller2D ao objeto e não permite que retire
public class Player : MonoBehaviour {

    public float maxJumpHeight = 4; //Altura máxima que o player pode pular
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f; //Tempo que leva para alcançar a altura máxima
    float accelerationTimeAirborne = .2f; //Aceleração no Ar
    float accelerationTimeGrounded = .1f; //Aceleração no chão
    float gravity;
<<<<<<< HEAD:Assets/Scripts 2/Player.cs
	SpriteRenderer rend;
    float jumpVelocity;
=======
    float maxJumpVelocity;
    float minJumpVelocity;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;
>>>>>>> master:Assets/Scripts/Player.cs

    float moveSpeed = 6;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

	void Start () {
        controller = GetComponent<Controller2D>();
<<<<<<< HEAD:Assets/Scripts 2/Player.cs
		rend = GetComponent<SpriteRenderer>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2); //S = S0 + V0*t + (a * t^2)/2
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex; // V = Vo + a*t;
=======
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2); //S = S0 + V0*t + (a * t^2)/2
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex; // V = Vo + a*t;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight); // V² = Vo² + 2*a*(S-So)
        print("Gravit: " + gravity + " Jump Velocity " + maxJumpVelocity);
>>>>>>> master:Assets/Scripts/Player.cs
    }

    void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);
        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void onJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope) //Se tentar pular quando a inclinação for maior que o máximo
            {
                if(directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) // Não está pulando contra a inclinação
                {
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            }
            else {
                velocity.y = maxJumpVelocity;
            }
        }
    }

    public void onJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
<<<<<<< HEAD:Assets/Scripts 2/Player.cs
		
		rend.flipX = input.x > 0;
=======
    }

    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below & velocity.y < 0) //Wall sliding
        {
            wallSliding = true;
            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }
            if (timeToWallUnstick > 0) //Não permite que o player saia da parede por um determinado tempo
            {
                velocityXSmoothing = 0;
                velocity.x = 0;
>>>>>>> master:Assets/Scripts/Player.cs

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }
    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        /* Suaviza o movimento na direção X
         * Se o player estiver no chão, o tempo para acelerar é menor
         */
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }
}
