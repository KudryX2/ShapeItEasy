using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    float movementSpeed = 80;
    float clampAngle = 80.0f;
    float sensitivity = 250.0f;

    float mouseX, mouseY;
    float rotationX = 0.0f;
    float rotationY = 0.0f;

    Vector3 movement;
    Rigidbody rigidbody;            

    float horizontalInput, verticalInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;               // Lock the cursor
        Cursor.visible = false;                                 // Hide el cursor

        rotationX = transform.localRotation.eulerAngles.x;      // Initial camera rotation 
        rotationY = transform.localRotation.eulerAngles.y;

        rigidbody = GetComponent <Rigidbody>();                 // Get the rigidbody component       

    }

    // Update is called once per frame
    void Update()
    {
        calculateCameraRotation();
        calculateCameraMovement();
    }

    private void calculateCameraRotation(){
        mouseX = Input.GetAxis("Mouse X");                      // Mouse input
        mouseY = Input.GetAxis("Mouse Y");

        rotationX += mouseX * sensitivity * Time.deltaTime;
        rotationY -= mouseY * sensitivity * Time.deltaTime;

        rotationY = Mathf.Clamp(rotationY, -clampAngle, clampAngle);            // Narrow down the camera y angle 
    
        rigidbody.MoveRotation(Quaternion.Euler(rotationY, rotationX, 0.0f));   // Rotate
    }

    private void calculateCameraMovement(){
        horizontalInput = Input.GetAxisRaw("Horizontal");       // Keyboard input
        verticalInput = Input.GetAxisRaw("Vertical");

        movement.Set(0,0,0);   

        if(verticalInput > 0)                                   // W A S D 
            movement += rigidbody.transform.forward;

        if(verticalInput < 0)
            movement -= rigidbody.transform.forward;

        if (horizontalInput > 0)
            movement += rigidbody.transform.right;

        if (horizontalInput < 0)
            movement -= rigidbody.transform.right;
        

        movement.y = 0;                                         // Reset the y axis movement 

        if(Input.GetKey(KeyCode.LeftShift))                     // Down (LeftShift)
            movement += rigidbody.transform.up * -1.0f;

        if(Input.GetKey(KeyCode.Space))                         // Up (Space)
            movement += rigidbody.transform.up;


        movement = (movement.normalized * movementSpeed) * Time.deltaTime;
        
        rigidbody.MovePosition(transform.position + movement);  // Move 
    }

}
