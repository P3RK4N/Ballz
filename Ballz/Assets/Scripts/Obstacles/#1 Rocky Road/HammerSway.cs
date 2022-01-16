using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSway : MonoBehaviour
{
    
    [SerializeField] float speed = 1f;

    Rigidbody rb;
    Transform tf;

    float offset = 0;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0,0,0);
        tf = GetComponent<Transform>();
        offset = UnityEngine.Random.Range(0, 6.28f);
    }

    private void Update() {
        pendulum();     
    }

    private void pendulum()
    {
        float angle = 80f * Mathf.Sin(Time.time * speed + offset);
        transform.localRotation = Quaternion.Euler(angle - 90f, 0, 90f);
    }

    private void OnCollisionEnter(Collision other) {
       foreach (ContactPoint contact in other.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.white);
			if (other.gameObject.GetComponent<MovePlayer>() != null)
			{
				Vector3 hitDir = contact.normal + new Vector3(0,0.1f,0);
				other.gameObject.GetComponent<Rigidbody>().AddForce(-hitDir * 100f);
			}
		}
    }

    private void OnCollisionStay(Collision other) {
       foreach (ContactPoint contact in other.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.white);
			if (other.gameObject.GetComponent<MovePlayer>() != null)
			{
				Vector3 hitDir = contact.normal + new Vector3(0,0.1f,0);
				other.gameObject.GetComponent<Rigidbody>().AddForce(-hitDir * 50f);
			}
		}
    }
}
