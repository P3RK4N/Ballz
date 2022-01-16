using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    Transform tf;
    Rigidbody rb;
    float radius;
    float surfaceLevel = 0f;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        radius = tf.localScale.x * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        surfaceLevel = GetComponent<FloatingPlayer>().surfaceLevel;
        if(tf.position.y < surfaceLevel + 0.4f * radius && !dead) StartCoroutine(death());
    }

    IEnumerator death()
    {
        Debug.Log("Dead");
        dead = true;
        GetComponent<MovePlayer>().enabled = false;
        rb.drag = 3f;
        yield return new WaitForSeconds(2f);
        tf.position = new Vector3(-10.64f,3.12f,6.46f);
        rb.drag = 1f;
        dead = false;
        GetComponent<MovePlayer>().enabled = true;
    }
}
