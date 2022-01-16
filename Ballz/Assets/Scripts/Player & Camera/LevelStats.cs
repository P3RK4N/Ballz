using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    [SerializeField] int obstacleID;
    [SerializeField] string obstacleName;
    
    public int current;

    private void Start() {
        current = 0;
    }

    public void increaseLevel(int id, string name)
    {
        obstacleID = id;
        obstacleName = name;
        current++;
    }
}
