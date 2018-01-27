using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] //Autimaticamente adiciona o BoxCollider2D ao objeto e não permite que retire
public class Controller2D : MonoBehaviour {

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;


    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        UpdateRaycastOrigins();
        CalculateRaySpacing();
        for (int i = 0; i < verticalRayCount; i++)
        {
            Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.red);
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
}
