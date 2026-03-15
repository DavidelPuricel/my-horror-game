using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 3.5f;
    public Transform orientation;
    public float gravity = -9.81f;
    
    float horizontalInput;
    float verticalInput;
    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    horizontalInput = Input.GetAxis("Horizontal");
    verticalInput = Input.GetAxis("Vertical");
    Vector3 moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    controller.Move(moveDirection.normalized * speed * Time.deltaTime);
    if(controller.isGrounded && velocity.y<0){
    	velocity.y = -2f;
    }
    velocity.y+=gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
        
    }
}
