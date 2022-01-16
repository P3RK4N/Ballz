using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceHelper : MonoBehaviour
{
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;

    private void Start() {
        Debug.Log(Vector3.Distance(pos1.position, pos2.position));
    }
}
