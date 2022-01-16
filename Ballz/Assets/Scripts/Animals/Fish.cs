using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    Transform tf;
    Transform player;
    Transform cam;

    [SerializeField] float distance = 7f;
    [SerializeField] float maxDistance = 15f;
    [SerializeField] float maxSpeed = 0.3f;
    //[SerializeField] float deltaSpeed = 0.05f;
    [SerializeField] float deltaRotate = 0.03f;
    [SerializeField] float lowerHeight = -0.4f;
    [SerializeField] float upperHeight = -0.2f;
    [SerializeField] float collisionRadius = 1f;
    [SerializeField] float collisionSpawnRadius = 0.4f;
    [SerializeField] float spawnDistance = 2f;
    [SerializeField] float respawnRadius = 3f;

    bool canReroute = true;
    [SerializeField] bool big = false;
    float speed;

    // Start is called before the first frame update
    void Awake()
    {
        tf = GetComponent<Transform>();
        player = FindObjectOfType<MovePlayer>().transform;
        cam = FindObjectOfType<cam>().transform;
        speed = maxSpeed;
        StartCoroutine(Respawn());
    }


    // Update is called once per frame
    void Update()
    {
        tf.position += tf.forward * speed * Time.deltaTime;
    }

    private void FixedUpdate() {
        if(canReroute) checkCollision();
    }

    void checkCollision()
    {
        Collider[] colliders = Physics.OverlapSphere(tf.position, collisionRadius);
        if(colliders.Length < 2) return;
        int n = 0;
        Vector3 pos = new Vector3(0,0,0);
        foreach (Collider col in colliders)
        {
            if(col.GetComponent<MovePlayer>() == null && col.GetComponent<Fish>() == null)
            {
                canReroute = false;
                pos += col.transform.position;
                n++;
            }
        }
        if(n == 0) return;
        pos /= n;
        StartCoroutine(smoothRotate(pos));
    }

    IEnumerator smoothRotate(Vector3 position)
    {
        float i = 0;
        Quaternion newRotation = Quaternion.LookRotation(- position + tf.position);
        newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);
        while(i < 1f)
        {
            float yAngle = Mathf.LerpAngle(tf.rotation.eulerAngles.y, newRotation.eulerAngles.y, i);
            tf.eulerAngles = new Vector3(0, yAngle, 0);
            i += deltaRotate;  
            yield return new WaitForFixedUpdate();
        } 
        yield return new WaitForSeconds(2f);
        canReroute = true;
    }

    IEnumerator smoothRandomRotate(Vector3 position)
    {
        float i = 0;
        Quaternion newRotation = Quaternion.LookRotation(position - tf.position);
        newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y + UnityEngine.Random.Range(-10,10), 0);
        canReroute = false;
        while(i < 1f)
        {
            float yAngle = Mathf.LerpAngle(tf.rotation.eulerAngles.y, newRotation.eulerAngles.y, i);
            tf.eulerAngles = new Vector3(0, yAngle, 0);
            i += deltaRotate;  
            yield return new WaitForFixedUpdate();
        } 
        canReroute = true;
    }

 

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f,2f));
        while(true)
        {

            float currentDistance = Vector3.Distance(tf.position, player.position);
            
            //Ako je predaleko, respawn
            if(currentDistance > maxDistance)
            {
                Vector3 camDir = cam.forward - new Vector3(0, cam.forward.y, 0);
                Vector2 pos = UnityEngine.Random.insideUnitCircle * 2f;
                Vector3 respawnPos = new Vector3(pos.x + cam.position.x, UnityEngine.Random.Range(lowerHeight, upperHeight), pos.y + cam.position.z) - camDir * spawnDistance;
                if(!Physics.CheckSphere(respawnPos, collisionSpawnRadius))
                    tf.position = respawnPos;
                else 
                {
                    yield return new WaitForSeconds(1f);
                    continue;   
                }
                Vector2 pos2 = UnityEngine.Random.insideUnitCircle * respawnRadius;
                tf.LookAt(player.position + new Vector3(pos2.x, 0, pos2.y));
                tf.rotation = Quaternion.Euler(0, tf.eulerAngles.y + UnityEngine.Random.Range(-30,30), 0);
                yield return new WaitForSeconds(4f);
            }
            else if(!canReroute) yield return new WaitForSeconds(2f);
            //Ako je srednje udaljenosti, a ne ide put playera, neka krene put playera
            else if(currentDistance > distance)
            {
                Quaternion newRotation = Quaternion.LookRotation(player.position - tf.position - new Vector3(0, player.position.y - tf.position.y, 0));
                if(Mathf.Abs(newRotation.eulerAngles.y - tf.rotation.eulerAngles.y) > 30f)
                {
                    Vector2 pos2 = UnityEngine.Random.insideUnitCircle * 3f;
                    StartCoroutine(smoothRandomRotate(player.position + new Vector3(pos2.x, 0, pos2.y)));
                }
            }
            yield return new WaitForSeconds(4f);
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.GetComponent<Fish>() != null && big) return;
        Vector3 dir = other.transform.position - tf.position - new Vector3(0, other.transform.position.y - tf.position.y, 0);
        dir.Normalize();
        tf.position -= dir * 0.01f;
    }
}
