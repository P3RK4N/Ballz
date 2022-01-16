using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetSea : MonoBehaviour
{
    //[SerializeField] float offset = 0;

    Material mat;
    Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        offsetMat();
    }

    private void offsetMat()
    {
        tf.position = FindObjectOfType<MovePlayer>().transform.position - new Vector3(0,FindObjectOfType<MovePlayer>().transform.position.y,0);
        //mat.SetFloat("_Xoffset", tf.position.z / offset / tf.localScale.x);
        //mat.SetFloat("_Zoffset", tf.position.x / offset / tf.localScale.y );
    }
}
