using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutSway : MonoBehaviour
{
    Vector3 swayDir;
    [SerializeField] float factor;
    [SerializeField] [Range(0,1)] float coconutFac = 0.95f;
    Transform tf;
    MeshRenderer trunkMR;
    Transform trunkTf;

    float freq;
    float fac;
    Vector3 startPos;
    
    // Start is called before the first frame update
    void Start()
    {
        tf = gameObject.transform;
        startPos = tf.localPosition;
        swayDir = gameObject.transform.parent.GetComponentInChildren<PalmSway>().swayDir;
        setupMaterial();
    }


    // Update is called once per frame
    void Update()
    {
        updateMaterial();
    }

    void updateMaterial()
    {
        tf.localPosition = startPos + coconutFac * factor * Mathf.Sin(Time.time * freq + trunkTf.position.x + trunkTf.position.z + 3.14f) * swayDir.normalized * fac;
    }

    void setupMaterial()
    {
        trunkMR = tf.parent.GetComponentInChildren<Trunk>().gameObject.GetComponent<MeshRenderer>();
        trunkTf = tf.parent.GetComponentInChildren<Trunk>().transform;
        fac = trunkMR.material.GetFloat("_fac");
        freq = trunkMR.material.GetFloat("_freq");
    }
}
