using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SphereCloud : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Transform tf;

    [SerializeField] float factor = 1f;

    private void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
        tf = GetComponent<Transform>();
    }

    private void Update() {
        meshRenderer.sharedMaterial.SetFloat("_Yoffset", tf.position.y * factor);
    }
} 
