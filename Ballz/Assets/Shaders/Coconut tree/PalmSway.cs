using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmSway : MonoBehaviour
{
    [SerializeField] public Vector3 swayDir;
    [SerializeField] float factor;
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
        setupMaterial();
    }


    // Update is called once per frame
    void Update()
    {
        updateMaterial();
    }

    void updateMaterial()
    {
        trunkMR.material.SetFloat("_time", Time.time);
        tf.localPosition = startPos + factor * Mathf.Sin(Time.time * freq + trunkTf.position.x + trunkTf.position.z + 3.14f) * swayDir.normalized * fac;
    }

    void setupMaterial()
    {
        trunkMR = tf.parent.GetComponentInChildren<Trunk>().gameObject.GetComponent<MeshRenderer>();
        trunkTf = tf.parent.GetComponentInChildren<Trunk>().transform;
        trunkMR.material.SetVector("_dir", swayDir.normalized);
        fac = trunkMR.material.GetFloat("_fac");
        freq = trunkMR.material.GetFloat("_freq");
        GetComponent<MeshRenderer>().material.SetFloat("_offsetSway", UnityEngine.Random.Range(0f, 6.28f));
    }
}
