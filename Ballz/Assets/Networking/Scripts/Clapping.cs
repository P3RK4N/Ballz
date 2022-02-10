using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clapping : MonoBehaviour
{
    public static float damageFactor = 100f;

    Rigidbody rb;

    public Vector3 currentVelocity;

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

        //Getting vector of collision
        Vector3 impactNormal = -other.GetContact(0).normal;

        //Getting relative speed before hit
        float myRelativeSpeed = Mathf.Max(0,Vector3.Dot(currentVelocity, impactNormal));
        float otherRelativeSpeed = Mathf.Max(0,Vector3.Dot(other.transform.GetComponent<Clapping>().currentVelocity, -impactNormal));

        //Calculating damage done to enemy
        float totalRelativeSpeed = myRelativeSpeed + otherRelativeSpeed;
        float dmgPercent = myRelativeSpeed / totalRelativeSpeed;

        float force = other.impulse.magnitude / Time.fixedDeltaTime;
        float enemyDamage = force / damageFactor * dmgPercent;
        other.transform.GetComponentInParent<PlayerStats>().damagePlayer(enemyDamage);
    }

    void FixedUpdate() 
    {
        currentVelocity = rb.velocity;
    }
}
