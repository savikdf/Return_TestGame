using UnityEngine;
using System.Collections;

[RequireComponent(typeof(P_Controller3D))]

public class P_Main : MonoBehaviour
{
    P_Controller3D controller;

    float gravity = -20;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<P_Controller3D>();
    }

    void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
	
}
