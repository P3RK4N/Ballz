using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] float moveForce = 1f;
    [SerializeField] float maxSpeed = 2f;
    [SerializeField] [Range(0,1)] float subMaximalSpeed = 0.5f;

    Rigidbody rb;
    Transform cameraTransform;

    float totalForce;
    bool onGround = false;
        public void SetOnGround(bool onGround) {this.onGround = onGround;} 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        move();
        controlSpeed();

        if(Input.GetKey(KeyCode.R)) gameObject.transform.position = new Vector3(0,0,0);
    }



   //smanjenje sile ako je u zraku
   private float calculateTotalForce()
    {
            if(onGround || true)
                return moveForce * Time.deltaTime * 10;
            else 
                return moveForce * Time.deltaTime * 4;    
    }

    //Otpor zraka za maksimalnu brzinu kretanja (iskljucivo xz ploha)
    private void controlSpeed()
    {
        Vector3 move = rb.velocity - new Vector3(0,rb.velocity.y,0);
        if(move.magnitude>maxSpeed*subMaximalSpeed) 
        {
            //otpor zraka
            //b*maxspeed3=moveForce*delta*10
            float resistanceFactor = moveForce*Time.deltaTime*10/Mathf.Pow(maxSpeed,2);
            // rb.AddForce(-move.normalized * resistanceFactor * move.magnitude);
            rb.AddForce(-resistanceFactor*move.normalized*move.magnitude*move.magnitude);
        }
    }

    //Kretanje u odnosu na vektor kamera-player
    private void move()
    {
            Vector3 straight = cameraTransform.position - transform.position;
            Vector2 perpendicular2D = Vector2.Perpendicular(new Vector2(straight.x,straight.z));
            Vector3 perpendicular = new Vector3(perpendicular2D.x,0,perpendicular2D.y);

            straight -= new Vector3(0,straight.y,0);
            straight.Normalize();
            perpendicular.Normalize();

            Vector3 movement = new Vector3(0,0,0);

            // if(Input.GetKey(KeyCode.W)) rb.AddForce(movement - straight);
            // if(Input.GetKey(KeyCode.A)) rb.AddForce(movement - perpendicular);
            // if(Input.GetKey(KeyCode.S)) rb.AddForce(movement + straight);
            // if(Input.GetKey(KeyCode.D)) rb.AddForce(movement + perpendicular);

            if(Input.GetKey(KeyCode.W)) movement = movement - straight;
            if(Input.GetKey(KeyCode.A)) movement = movement - perpendicular;
            if(Input.GetKey(KeyCode.S)) movement = movement + straight;
            if(Input.GetKey(KeyCode.D)) movement = movement + perpendicular;

            movement.Normalize();

            rb.AddForce(movement * calculateTotalForce());
    }
}
