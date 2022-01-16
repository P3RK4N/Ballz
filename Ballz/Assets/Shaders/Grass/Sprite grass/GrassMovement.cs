using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GrassMovement : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<MovePlayer>() != null)
        {
            GetComponent<Renderer>().material.SetInt("_Player", 1);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.GetComponent<MovePlayer>() != null)
        {
            GetComponent<Renderer>().material.SetInt("_Player", 0);
        }
    }
}
