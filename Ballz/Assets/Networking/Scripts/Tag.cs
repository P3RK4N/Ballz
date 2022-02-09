using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class Tag : MonoBehaviour
{
    MultiplayerMovement ball;
    [SerializeField] TMPro.TMP_Text tm;

    private void Start() 
    {
        ball = transform.parent.GetComponentInChildren<MultiplayerMovement>();  
    }

    void Update()
    {
        transformTag();
        updateTag();
    }

    private void updateTag()
    {
        float mass = ball.GetComponent<Rigidbody>().mass;
        tm.text = "Mass: " + (int)(mass * 100);
    }

   private void transformTag()
    {
        transform.position = ball.transform.position + new Vector3(0,0.4f,0);
        transform.LookAt(FindObjectOfType<Camera>().transform.position);
    }
}
