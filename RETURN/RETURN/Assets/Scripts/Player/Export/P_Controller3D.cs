using UnityEngine;
using System.Collections;

//Will have to swap out the box collider for Capsule
[RequireComponent(typeof(CapsuleCollider))]
public class P_Controller3D : MonoBehaviour
{
    const float skinWidth = .015f;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    CapsuleCollider collider;
    RayCastOrigins raycastOrigins;
	void Start ()
    {
        collider = GetComponent<CapsuleCollider>();
        CalculateRaySpacing();
    }
    

    void Update()
    {

    
    }

    void VerticalCollisions()
    {
        for (int i = 0; i < verticalRayCount; i++)
        {
            Debug.DrawRay(raycastOrigins.bottomLeftFront + (Vector3.right) * verticalRaySpacing * i, Vector3.up * -2, Color.red);
            Debug.DrawRay(raycastOrigins.bottomRightBack + (-Vector3.right) * verticalRaySpacing * i, Vector3.up * -2, Color.red);
        }
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        transform.Translate(velocity);
    }

	void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeftFront = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        raycastOrigins.bottomRightFront = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        raycastOrigins.topLeftFront = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        raycastOrigins.topRightFront = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);

        raycastOrigins.bottomLeftBack = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        raycastOrigins.bottomRightBack = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        raycastOrigins.topLeftBack = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        raycastOrigins.topRightBack = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RayCastOrigins
    {
        public Vector3 topLeftFront, topRightFront, topLeftBack, topRightBack;
        public Vector3 bottomLeftFront, bottomRightFront, bottomLeftBack, bottomRightBack;
    }
}
