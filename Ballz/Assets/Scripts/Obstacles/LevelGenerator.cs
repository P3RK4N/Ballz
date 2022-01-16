using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private const float HEIGHT = 2.221f;
    private const float DISTANCE = 5.259f;

    [SerializeField] public GameObject previousLevel;
    [SerializeField] public GameObject currentLevel;
    [SerializeField] public GameObject newLevel;
    public void generateNewLevel()
    {
        //trenutni level
        Transform tf = currentLevel.GetComponentInChildren<LevelEnd>().transform;
        //pozicija kraja mosta
        Vector3 pos = tf.position + tf.forward * DISTANCE;
        pos -= new Vector3(0,pos.y,0);
        //rotacija jednaka kao smjer kraja levela
        Quaternion rot = tf.rotation;
        //traženje sljedeceg levela
        newLevel = GetComponent<ObstacleManager>().findObstacle();
        newLevel.transform.position = pos;
        newLevel.transform.rotation = rot;
        //generiranje mosta između
        FindObjectOfType<BridgeBuilder>().calculateBridge(tf.position, pos + new Vector3(0, HEIGHT, 0));
        previousLevel = currentLevel;
        currentLevel = newLevel;
        
        FindObjectOfType<LevelStats>().increaseLevel(newLevel.GetComponent<ObstacleID>().id, newLevel.name);
    }
}
