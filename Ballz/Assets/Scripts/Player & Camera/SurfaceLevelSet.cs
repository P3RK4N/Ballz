using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceLevelSet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<MovePlayer>() != null)
        {
            other.gameObject.GetComponent<FloatingPlayer>().surfaceLevel = transform.position.y;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.GetComponent<MovePlayer>() != null)
        {
            other.gameObject.GetComponent<FloatingPlayer>().surfaceLevel = 0f;
        }
    }
}
