using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<MovePlayer>() != null)
        {
            FindObjectOfType<LevelGenerator>().generateNewLevel();
            Destroy(gameObject);
        }
    }
}
