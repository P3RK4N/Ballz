using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePlank : MonoBehaviour
{
    private void OnCollisionStay(Collision other) {
        if(other.gameObject.GetComponent<MovePlayer>() != null)
        {
            other.rigidbody.AddForce(new Vector3(0,3f,0));
        }
    }
}
