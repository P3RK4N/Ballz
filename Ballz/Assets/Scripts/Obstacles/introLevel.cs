using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<MovePlayer>() != null)
        {
            LevelGenerator intro = FindObjectOfType<LevelGenerator>();
            intro.currentLevel.GetComponentInChildren<AwakePath>().initialize();
            Destroy(intro.previousLevel);
        }
    }
}
