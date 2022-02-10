using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMovement : MonoBehaviour
{
    [SerializeField] float moveForce = 1f;
    [SerializeField] float maxSpeed = 2f;
    [SerializeField] [Range(0,1)] float subMaximalSpeed = 0.5f;

    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;

    public Vector3 currentDirection = new Vector3(0f,0f,0f);
    public float resistanceFactor = 5f;
    float damage = 0f;

    Rigidbody rb;
    Transform cameraTransform;

    float totalForce;
    public bool onGround = false;

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
            if(onGround)
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
            float resistance = moveForce*Time.deltaTime*resistanceFactor/Mathf.Pow(maxSpeed,2);
            // rb.AddForce(-move.normalized * resistanceFactor * move.magnitude);
            rb.AddForce(-resistance*move.normalized*move.magnitude*move.magnitude);
        }
    }

    //Kretanje u odnosu na vektor kamera-player
    private void move()
    {
            //Reference system for movement
            // Vector3 straight = cameraTransform.position - transform.position;
            Vector3 straight = -cameraTransform.forward;

            Vector2 perpendicular2D = Vector2.Perpendicular(new Vector2(straight.x,straight.z));
            Vector3 perpendicular = new Vector3(perpendicular2D.x,0,perpendicular2D.y);

            straight -= new Vector3(0,straight.y,0);
            straight.Normalize();
            perpendicular.Normalize();

            Vector3 direction = new Vector3(0,0,0);

            // if(Input.GetKey(KeyCode.W)) rb.AddForce(movement - straight);
            // if(Input.GetKey(KeyCode.A)) rb.AddForce(movement - perpendicular);
            // if(Input.GetKey(KeyCode.S)) rb.AddForce(movement + straight);
            // if(Input.GetKey(KeyCode.D)) rb.AddForce(movement + perpendicular);

            if(Input.GetKey(up)) direction = direction - straight;
            if(Input.GetKey(left)) direction = direction - perpendicular;
            if(Input.GetKey(down)) direction = direction + straight;
            if(Input.GetKey(right)) direction = direction + perpendicular;

            direction.Normalize();
            currentDirection = direction;

            rb.AddForce(direction * calculateTotalForce());
    }

    public void recalculateResistanceFactor()
    {
        resistanceFactor = Mathf.Lerp(5f,3f, (1-rb.mass) / 0.8f);
    }

    public void increaseDamage(float dmg)
    {
        damage += dmg;
        rb.mass = Mathf.Exp(-0.1f*damage);
    }
}
