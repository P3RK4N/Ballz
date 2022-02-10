using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tag : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text tm;

    private void Start() 
    {
    }

    void Update()
    {
        transformTag();
        updateTag();
    }

    private void updateTag()
    {
        float mass = transform.parent.GetComponent<PlayerStats>().mass;
        tm.text = "Mass: " + $"{mass:0.00}";
    }

   private void transformTag()
    {
        transform.position = transform.parent.GetComponentInChildren<MultiplayerMovement>().transform.position + new Vector3(0,0.4f,0);
        transform.LookAt(FindObjectOfType<Camera>().transform.position);
    }
}
