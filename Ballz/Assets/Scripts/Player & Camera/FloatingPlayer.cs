using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlayer : MonoBehaviour
{
    [SerializeField] float force = 1f;

    Transform tf;
    Rigidbody rb;

    float radius;
    public float surfaceLevel = 0f;
    float g = Mathf.Abs(Physics.gravity.y);

    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        radius = tf.localScale.x * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(tf.position.y < surfaceLevel + radius/3f) upForce();
    }

    private void upForce()
     {
        float lerpVal = (surfaceLevel - tf.position.y + radius/3f) / (radius * 1.33f);
        float currentForce = Mathf.Lerp(0, force, lerpVal);
        rb.AddForce(new Vector3(0,currentForce * Time.deltaTime,0));
    }
}
