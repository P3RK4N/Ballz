using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeafloorDecorator : MonoBehaviour
{
    [SerializeField] GameObject[] decorations;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(reDecorate());
    }

    IEnumerator reDecorate()
    {
        yield return new WaitForFixedUpdate();
        foreach (GameObject decoration in decorations)
        {
            //NEED TO DO
        }
    }

   // Update is called once per frame
   void Update()
    {
        
    }
}
