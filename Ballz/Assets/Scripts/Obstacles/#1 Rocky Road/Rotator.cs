using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float force = 1f;
    [SerializeField] float rotationSpeed = 1f;

    Transform tf;
    Rigidbody rb;

    float offset;

    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        offset = UnityEngine.Random.Range(0.75f,1.25f);
        tf.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0f,90f));
    }

    // Update is called once per frame
    void Update()
    {
        rotate();
    }

   private void rotate()
   {
      rb.AddTorque(new Vector3(0,1f,0) * force * Time.deltaTime);
      rb.angularVelocity = new Vector3(0,Mathf.Max(rb.angularVelocity.y, rotationSpeed * offset),0);
   }
}
