using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Camera controls")]
    [SerializeField] float followingSpeed = 1f;
    [SerializeField] float YrotatingSpeed = 1f;
    [SerializeField] float XrotatingSpeed = 1f;
    //[SerializeField] float zoomSpeed= 1f;
    //[SerializeField] [Range(0f,1f)] float visinaKamere = 0.5f;

    Transform player;
    Transform cameraChild;

    //Inicijalizacija (rotateAroundPlayer)
    float yRange = 15;
        float upperLimit = 35f;
        float lowerLimit = -35f;

    float xValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<MovePlayer>().transform;
        cameraChild = transform.GetChild(0);
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(smoothFollowing());
        rotateAroundPlayer();
        adaptFOVtoSpeed();
    	
    }

   private void adaptFOVtoSpeed()
   {
       
   }


   //Rotacija oko playera
   private void rotateAroundPlayer()
   {
        //Rotacija camera parent
        //Y ima range i pocetni kut, a X netriba
        yRange = Mathf.Clamp(yRange + YrotatingSpeed * Input.GetAxis("Mouse Y"),-upperLimit,-lowerLimit);
        xValue += XrotatingSpeed * Input.GetAxis("Mouse X");

        transform.rotation = Quaternion.Euler(-yRange, xValue, transform.rotation.z);
   }

    //Glatki transform.position prijelaz 
    IEnumerator smoothFollowing(){
        //cameraChild.LookAt(player.position + new Vector3(0,visinaKamere,0));

        Vector3 endPosition = player.position;
        Vector3 startPosition = transform.position;
        //Vektor smjera gledanja
        float progress = 0f;
        while (progress<1f)
        {
            progress += Time.deltaTime * followingSpeed;
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            yield return new WaitForEndOfFrame();
        }
    }
}
