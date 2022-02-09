using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSkills : MonoBehaviour
{
    Rigidbody rb;

    //JUMP
    [Header("#1 - Jump skill")]
    [SerializeField] float jumpTimer = 1.5f;
    float currentJumpTime = 0f;
    [SerializeField] float jumpForce = 200f;
    [SerializeField] KeyCode jumpButton = KeyCode.Space;

    [Space(5f)]

    //Thrust
    [Header("#2 - Thrust skill")]
    [SerializeField] float thrustTimer = 3f;
    float currentThrustTime = 0f;
    [SerializeField] float thrustForce = 100f;
    [SerializeField] KeyCode thrustButton = KeyCode.LeftAlt;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        jump();
        thrust();
    }

   private void thrust()
   {
       if(Input.GetKey(thrustButton) && currentThrustTime < 0)
       {
            Vector3 direction = new Vector3(GetComponent<MultiplayerMovement>().currentDirection.x, 0, GetComponent<MultiplayerMovement>().currentDirection.z).normalized;
            rb.AddForce(direction * thrustForce);
            currentThrustTime = thrustTimer;
       }
       else 
       {
           currentThrustTime -= Time.deltaTime;
       }
   }

   private void jump()
    {
        if(currentJumpTime > 0) currentJumpTime -= Time.deltaTime;
        else if(Input.GetKey(jumpButton) && GetComponent<MultiplayerMovement>().onGround) 
        {
            rb.AddForce(new Vector3(0, jumpForce, 0));
            currentJumpTime = jumpTimer;
        }
    }
}
