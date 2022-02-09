using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag != "Ground") return;
        GetComponentInParent<MultiplayerMovement>().onGround = true;
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag != "Ground") return;
        GetComponentInParent<MultiplayerMovement>().onGround = false;
    }
}
