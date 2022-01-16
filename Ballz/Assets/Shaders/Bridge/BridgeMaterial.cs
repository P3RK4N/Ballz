using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeMaterial : MonoBehaviour
{
    [SerializeField] [Range(-2,5)] float offset = 0f;
    [SerializeField] [Range(0,1)] float limit3 = 0.657f;

    Material[] mats;

    private void OnValidate() {
        changeMaterialProperties();
    }

    private void changeMaterialProperties()
    {
        if(mats != null)
        foreach (Material mat in mats)
        {
            mat.SetFloat("_Limit3", limit3);
            mat.SetFloat("_Offset", offset);
        }   
    }

   void Start()
    {
        mats = GetComponent<MeshRenderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
