using UnityEngine;

/*
 * ref: Toda alteração feita em uma variável com ref, vai alterar seu valor nas demais funções, não apenas localmente
 */
public class Controller2D : RaycastController {

    float maxClimbAngle = 80;
    float maxDescendAngle = 75;
   
    public CollisionsInfo collisions;
    [HideInInspector]
    public Vector2 playerInput;

    public override void Start() //Chama o método Start do RaycastController
    {
        base.Start();
        collisions.faceDir = 1;
    }

    public void Move(Vector2 moveAmout, bool standingOnPlatform)
    {
        Move(moveAmout, Vector2.zero, standingOnPlatform);
    }
    //Função que controla o movimento
    public void Move(Vector2 moveAmout, Vector2 input, bool stadingOnPlatform = false) 
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.moveAmoutOld = moveAmout;
        playerInput = input;

        if (moveAmout.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(moveAmout.x);
        }
        if(moveAmout.y < 0) //Descending Slope
        {
            DescendSlope(ref moveAmout);
        }

        HorizontalCollisions(ref moveAmout); //Não verifica se moveAmout.x != 0 por causa do Wall Sliding
        if (moveAmout.y != 0)
        {
            VerticalCollisions(ref moveAmout);
        }

        transform.Translate(moveAmout);
        if (stadingOnPlatform)
        {
            collisions.below = true;
        }
    }

    // Trata as colisões verticais
    void VerticalCollisions(ref Vector2 moveAmout) 
    {
        float directionY = Mathf.Sign(moveAmout.y); //Pega o sinal da direção em Y (-1: baixo, 1: cima)
        float rayLength = Mathf.Abs(moveAmout.y) + skinWidth; 
        
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmout.x);
            //LayerMask: Com quais layers ele vai colidir
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
          
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                if(hit.collider.tag == "Through")
                {
                    if(directionY == 1 || hit.distance == 0) //Se tiver subindo uma plataforma ou parar no meio dela, não colide
                    {
                        continue;
                    }
                    if (collisions.fallingThroughPlatform) //Se estiver caindo da plataforma
                    {
                        continue;
                    }
                    if(playerInput.y == -1) //Se estiver numa plataforma e apertar para baixo, ignora colisão
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .5f); //depois de meio segundo, reseta
                        continue;
                    }
                }
                moveAmout.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    moveAmout.x = moveAmout.y / Mathf.Tan(collisions.slopeAngle * Mathf.Rad2Deg) * Mathf.Sign(moveAmout.x);
                }
                //Determina para qual direção colide
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(moveAmout.x);
            rayLength = Mathf.Abs(moveAmout.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? 
                raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmout.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != collisions.slopeAngle)
                {
                    moveAmout.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }

        }
    }

    //Trata as colisões horizontais
    void HorizontalCollisions(ref Vector2 moveAmout)
    {
        float directionX = collisions.faceDir; //Pega o sinal da direção em X (-1: esquerda, 1: direita)
        float rayLength = Mathf.Abs(moveAmout.x) + skinWidth;

        if(Mathf.Abs(moveAmout.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            //LayerMask: Com quais layers ele vai colidir
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                if(hit.distance == 0)
                {
                    continue;
                }
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up); //Determina o ângulo de inclinação do chão

                if (i == 0 && slopeAngle <= maxClimbAngle) //Subida
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        moveAmout = collisions.moveAmoutOld;
                    }

                    float distanceToSlopeStart = 0;
                    if(slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmout.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmout, slopeAngle);
                    moveAmout.x += distanceToSlopeStart * directionX;
                }

                if(!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    moveAmout.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        moveAmout.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmout.x);
                    }

                    //Determina para qual direção colide
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void ClimbSlope(ref Vector2 moveAmout, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(moveAmout.x);
        float climbMoveAmoutY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if(moveAmout.y <= climbMoveAmoutY)
        { 
            moveAmout.y = climbMoveAmoutY;
            moveAmout.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmout.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector2 moveAmout)
    {
        float directionX = Mathf.Sign(moveAmout.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) //Superfície plana
            {
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle*Mathf.Deg2Rad) * Mathf.Abs(moveAmout.x))//O quão loje está da inclinação
                    {
                        float moveDistance = Mathf.Abs(moveAmout.x);
                        float descendMoveAmoutY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmout.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmout.x);
                        moveAmout.y -= descendMoveAmoutY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionsInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector2 moveAmoutOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
