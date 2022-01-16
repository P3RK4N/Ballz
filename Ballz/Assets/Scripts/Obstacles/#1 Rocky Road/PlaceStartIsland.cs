using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceStartIsland : MonoBehaviour
{
    Transform end;
    Transform start;


    private const float HEIGHT = 2.221f;
    private const float DISTANCE = 5.259f;

    // Start is called before the first frame update
    void Start()
    {
        end = GetComponent<Transform>().GetChild(4);
        start = GameObject.Find("StartRR").GetComponent<Transform>();
        
        //placeIsland();
    }

    private void placeIsland()
    {
        start.rotation = end.rotation;
        start.position = end.position + end.forward * DISTANCE;
    }
}
