﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))] //Autimaticamente adiciona o BoxCollider2D ao objeto e não permite que retire
public class RaycastController : MonoBehaviour {

    public LayerMask collisionMask;

    public const float skinWidth = .015f;
    const float dstBetweenRays = .25f;

    [HideInInspector]
    public int horizontalRayCount;
    [HideInInspector]
    public int verticalRayCount;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public BoxCollider2D collider;
    public RaycastOrigins raycastOrigins;

    public virtual void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins() //A origem do raycast no objeto
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2); //coloca para dentro de uma "pele"

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    public void CalculateRaySpacing() //Distância entre os raycasts
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins //Corners do objeto
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
