using UnityEngine;

[RequireComponent(typeof(Controller2D))] //Autimaticamente adiciona o Controller2D ao objeto e não permite que retire
public class Player : MonoBehaviour {

    public float maxJumpHeight = 4; //Altura máxima que o player pode pular
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f; //Tempo que leva para alcançar a altura máxima
    float accelerationTimeAirborne = .2f; //Aceleração no Ar
    float accelerationTimeGrounded = .1f; //Aceleração no chão
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float moveSpeed = 6;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

	void Start () {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2); //S = S0 + V0*t + (a * t^2)/2
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex; // V = Vo + a*t;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight); // V² = Vo² + 2*a*(S-So)
        print("Gravit: " + gravity + " Jump Velocity " + maxJumpVelocity);
    }

    void Update () {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        /* Suaviza o movimento na direção X
         * Se o player estiver no chão, o tempo para acelerar é menor
         */
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below & velocity.y < 0) //Wall sliding
        {
            wallSliding = true;
            if(velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }
            if(timeToWallUnstick > 0) //Não permite que o player saia da parede por um determinado tempo
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if(input.x != wallDirX && input.x != 0)
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
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallSliding)
            {
                if(wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if(input.x == 0)
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
                velocity.y = maxJumpVelocity;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if(velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }
}
