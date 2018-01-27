using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] //Autimaticamente adiciona o BoxCollider2D ao objeto e não permite que retire
public class Controller2D : MonoBehaviour {

    public LayerMask collisionMask; 

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;
    public CollisionsInfo collisions;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    //Função que controla o movimento
    public void Move(Vector3 velocity) 
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if(velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    /*  Trata as colisões verticais
     *  ref: Toda alteração feita no vetor velocity dentro dessa função, 
     * vai alterar seu valor nas demais funções;
     */
    void VerticalCollisions(ref Vector3 velocity) 
    {
        float directionY = Mathf.Sign(velocity.y); //Pega o sinal da direção em Y (-1: baixo, 1: cima)
        float rayLenght = Mathf.Abs(velocity.y) + skinWidth; 
        
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            //LayerMask: Com quais layers ele vai colidir
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, collisionMask);
          
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLenght, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLenght = hit.distance;


                //Determina para qual direção colide
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    //Trata as colisões horizontais
    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x); //Pega o sinal da direção em X (-1: esquerda, 1: direita)
        float rayLenght = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            //LayerMask: Com quais layers ele vai colidir
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLenght, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLenght = hit.distance;

                //Determina para qual direção colide
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void UpdateRaycastOrigins() //A origem do raycast no objeto
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2); //coloca para dentro de uma "pele"

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    void CalculateRaySpacing() //Distância entre os raycasts
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RaycastOrigins //Corners do objeto
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionsInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}
