using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clapping : MonoBehaviour
{
    public float currentVelocity;
    Rigidbody rb;
    public static float damage = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag != "Player") return;
        float otherVelocity = other.gameObject.GetComponent<Clapping>().currentVelocity;
        float force = Vector3.Dot(other.GetContact(0).normal,other.relativeVelocity);
        float damageFactor = force / damage;
        Debug.Log(transform.parent.name + ": " + currentVelocity + " " + other.transform.parent.name + ": " + otherVelocity + " " + damageFactor);
    }

    void FixedUpdate() 
    {
        currentVelocity = rb.velocity.magnitude;
    }
}
