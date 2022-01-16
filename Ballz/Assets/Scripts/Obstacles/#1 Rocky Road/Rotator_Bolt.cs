using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator_Bolt : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;

    Transform tf;

    float angle = 0f;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();   
    }

    // Update is called once per frame
    void Update()
    {
        rotate();
    }

    private void rotate()
    {
        angle += rotationSpeed * Time.deltaTime;
        if(angle>360f) angle = 0f;

        tf.localEulerAngles = new Vector3(0, 0, angle);
    }
}
