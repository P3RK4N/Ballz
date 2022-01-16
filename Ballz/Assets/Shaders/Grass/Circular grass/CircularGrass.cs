using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularGrass : MonoBehaviour
{
    Material middleMat;
    Material grassMat;

    void Start()
    {
        middleMat = transform.GetChild(1).GetComponent<MeshRenderer>().material;
        grassMat = transform.GetChild(2).GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.GetComponent<MovePlayer>() != null) 
        {
            middleMat.SetInt("_Player", 1);
            grassMat.SetInt("_Player", 1);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.GetComponent<MovePlayer>() != null) 
        {
            middleMat.SetInt("_Player", 0);
            grassMat.SetInt("_Player", 0);
        }
    }
}
