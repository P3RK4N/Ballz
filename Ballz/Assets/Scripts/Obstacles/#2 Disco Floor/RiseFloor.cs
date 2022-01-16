using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseFloor : MonoBehaviour
{
    [SerializeField] float rowRiseTime = 0.5f;

    SpawnFloor tiles;
    bool risen = false;

    // Start is called before the first frame update
    void Start()
    {
       tiles = GetComponentInParent<SpawnFloor>();
    }

    private void OnTriggerEnter(Collider other) {
        if(!risen && other.gameObject.GetComponent<MovePlayer>() != null)
        {
            risen = true;
            StartCoroutine(riseTiles());
        }
    }

    IEnumerator riseTiles()
    {
        for (int i = 0; i < tiles.fullLength; i++)
        {
            for (int j = 0; j < tiles.fullWidth; j++)
            {
                if(tiles.tiles[i,j]) 
                {
                    StartCoroutine(tiles.tiles[i,j].GetComponentInChildren<TileInfo>().rise());
                }
            }
            yield return new WaitForSeconds(rowRiseTime);
        }
        startPeriodicRising();
    }

    private void startPeriodicRising()
    {
        foreach (GameObject tile in tiles.tiles)
        {
            if(tile) StartCoroutine(tile.GetComponentInChildren<TileInfo>().periodicRising());
        };
    }
}
