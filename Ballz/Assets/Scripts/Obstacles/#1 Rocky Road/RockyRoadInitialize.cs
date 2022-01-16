using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyRoadInitialize : MonoBehaviour
{
    [SerializeField] GameObject island;
    float minDistance = 20f;
    float maxDistance = 50f;
    float distance = 20f;
    int current;

    private void Awake() {
        current = FindObjectOfType<LevelStats>().current;
        float distance = Mathf.Clamp(20 + current * 1.2f, minDistance, maxDistance);
        spawnEndIsland();
    }

    private void spawnEndIsland()
    {
        Transform tf = GetComponent<Transform>().GetChild(0).GetComponentInChildren<AwakePath>().transform;
        Vector3 pos = tf.position + tf.forward * distance;
        Quaternion rot = Quaternion.Euler(new Vector3(0, 180, 0) + tf.eulerAngles);
        GameObject endIsland = Instantiate(island, pos, rot, transform);
        GetComponentInChildren<RockyRoad>().end = endIsland;
        GetComponentInChildren<RockyRoad>().stage = FindObjectOfType<LevelStats>().current;
        GetComponentInChildren<RockyRoad>().enabled = true;
    }
}
