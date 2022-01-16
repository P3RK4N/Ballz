using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCollider : MonoBehaviour
{
    private void OnTriggerStay(Collider other) {
        GetComponentInParent<MovePlayer>().SetOnGround(true);
    }

    private void OnTriggerExit(Collider other) {
        GetComponentInParent<MovePlayer>().SetOnGround(false);
    }
}
