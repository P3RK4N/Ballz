using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GrassScript : MonoBehaviour
{
    Transform tf;
    Transform playerTransform;
    MeshRenderer mr;
    Material mat;

    bool sway = false;

    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        mr = GetComponent<MeshRenderer>();
        if(Application.isPlaying)
            mat = mr.material;
        else
            mat = mr.sharedMaterial;


        pos = tf.position;

        mat.SetVector("_Position", pos);
        mat.SetFloat("_Height", 0.442f * tf.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(sway && mat != null)
        {
            playerTransform = FindObjectOfType<MovePlayer>().transform;

            mat.SetFloat("_Distance",Vector2.Distance(new Vector2(tf.position.x,tf.position.z),new Vector2(playerTransform.position.x,playerTransform.position.z)));
            mat.SetVector("_PlayerVector", Vector3.Normalize(new Vector2(playerTransform.position.x,playerTransform.position.z) - new Vector2(tf.position.x,tf.position.z) ));
            //mat.SetVector("_PlayerVector", Vector3.Normalize(new Vector2(tf.position.x,tf.position.z) - new Vector2(playerTransform.position.x,playerTransform.position.z)));
        }
    }


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<MovePlayer>() != null) sway = true;
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.GetComponent<MovePlayer>() != null) sway = false;
    }
} 
