using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaFloorManager : MonoBehaviour
{
    [SerializeField] GameObject floorTile;
    
    int numberOfTiles = 9;
    int tileSize = 20;
    bool start = true;

    Transform player;
    GameObject[] instancesOfTiles;   
    List<Vector2Int> surroundingCoords;
    List<Vector2Int> tilesPositions;

    // Inicijalizacija i instanciranje N tileova (GameObjekata)
    void Awake()
    {
        player = FindObjectOfType<MovePlayer>().transform;

        instancesOfTiles = new GameObject[numberOfTiles];
        for(int i=0;i<numberOfTiles;i++)
        {
            instancesOfTiles[i] = Instantiate(floorTile,transform);
            //instancesOfTiles[i].SetActive(false);
        }
    }

    //pokretanje trazenja bliskih KOORDINATA u tipu Vector2Int
    private void Start() {
        InvokeRepeating("getSurroundingTiles", 0f, 0.5f);
    }

    //trazenje 3x3 kocke oko playera i stavljanje u ArrayList u obliku Vector2Int
    private void getSurroundingTiles()
    {
        //trazimo srediste srednjeg tilea
        Vector2Int startPos = new Vector2Int(Mathf.RoundToInt(player.position.x / 20) * 20, Mathf.RoundToInt(player.position.z / 20) * 20);

        //inicijalizacija surroundingCoords
        surroundingCoords = new List<Vector2Int>();

        //Stavljanje 9 lokacija susjednih polja u surroundingCoords
        for(int i = -1;i<=1;i++) for(int j = -1;j<=1;j++)
        {
            surroundingCoords.Add(new Vector2Int(Mathf.RoundToInt(startPos.x + tileSize * i),Mathf.RoundToInt(startPos.y + tileSize * j)));
        }
        
        manageTiles2();
        
        //provjeravanje svih child objekata s metodom koja deaktivira daleke tileove (GameObjecte)  , ako je startup onda preskace provjeru
        // if(startup)
        //     checkTiles(surroundingCoords);
        // else 
        // {
        //     startup = true;
        //     filterNewCoords(surroundingCoords);
        // }
    }
    
    private void manageTiles2()
    {   
        //pocetno postavljanje tileova
        if(start) 
        {
            for(int i = 0; i < numberOfTiles; i++)
            {
                instancesOfTiles[i].transform.position = new Vector3Int(surroundingCoords[i].x,0,surroundingCoords[i].y);
            }
            start = false;
        }
        else
        {
            //all positions of tiles
            recalculateTilesPositions();

            //trazenje bliskog mjesta na kojem nije nista postavljeno i premjestanje tilea
            foreach (GameObject tile in instancesOfTiles)
            {
                Vector2Int coord = new Vector2Int(Mathf.RoundToInt(tile.transform.position.x),Mathf.RoundToInt(tile.transform.position.z));
                if(!surroundingCoords.Contains(coord))
                {
                    foreach (Vector2Int unusedCoord in surroundingCoords)
                    {
                        if(!tilesPositions.Contains(unusedCoord))
                        {
                            tile.transform.position = new Vector3Int(unusedCoord.x,0,unusedCoord.y);
                            recalculateTilesPositions();
                            break;
                        }
                    }
                }
            }
        }
    }

   private void recalculateTilesPositions()
   {
        tilesPositions = new List<Vector2Int>();
        foreach (Transform tile in transform)
            {
                tilesPositions.Add(new Vector2Int(Mathf.RoundToInt(tile.position.x),Mathf.RoundToInt(tile.position.z)));
            }
   }

   // private void manageTiles()
   // {   
   //     //deaktivacija i micanje s current liste dalekih tileova

   //     List<GameObject> trashList = new List<GameObject>();
   //     foreach (GameObject placedTile in currentTiles)
   //     {
   //         Vector2Int currentTilePosition = new Vector2Int(Mathf.RoundToInt(placedTile.transform.position.x),Mathf.RoundToInt(placedTile.transform.position.z));

   //         if(!surroundingCoords.Contains(currentTilePosition))
   //         {
   //             trashList.Add(placedTile);
   //         }
   //     }

   //     foreach (GameObject trash in trashList)
   //     {
   //         currentTiles.Remove(trash);
   //             trash.SetActive(false);
   //     }

   //     //popunjavanje current liste i aktiviranje tileova
   //     while(currentTiles.Count < numberOfTiles)
   //     {
   //         GameObject deaktiviraniTile = null;

   //         //trazenje prvog deaktiviranog
   //         foreach (Transform child in transform)
   //         {
   //             if(!child.gameObject.activeInHierarchy)
   //             {
   //                 deaktiviraniTile = child.gameObject;
   //                 break;
   //             }
   //         }

   //         List<Vector2Int> takenCoords = new List<Vector2Int>();

   //         //trazenje svih current tiles
   //         foreach (GameObject placedTile in currentTiles)
   //         {
   //             Vector2Int currentTilePosition = new Vector2Int(Mathf.RoundToInt(placedTile.transform.position.x),Mathf.RoundToInt(placedTile.transform.position.z));
   //             takenCoords.Add(currentTilePosition);
   //         }

   //         //trazenje surroundingCoord koji nije u currentTiles i postavljanje novog current tilea
   //         foreach (Vector2Int surroundingCoord in surroundingCoords)
   //         {
   //             if(!takenCoords.Contains(surroundingCoord))
   //             {
   //                 deaktiviraniTile.transform.position = new Vector3Int(surroundingCoord.x,0,surroundingCoord.y);
   //                 deaktiviraniTile.SetActive(true);
   //                 currentTiles.Add(deaktiviraniTile);
   //                 break;
   //             }
   //         }
   //     }
   // }



   // public void filterNewCoords(ArrayList availableCoords)
   // {
   //     if (currentTiles.Count > 0) foreach (GameObject tile in currentTiles)
   //     {
   //         Vector2Int temp = new Vector2Int(Mathf.RoundToInt(tile.transform.position.x),Mathf.RoundToInt(tile.transform.position.z));
   //         if(availableCoords.Contains(temp)); 
   //             availableCoords.Remove(temp);
   //     }

   //     addNewTiles(availableCoords);
   // }

   // private void addNewTiles(ArrayList availableCoords)
   // {
   //     if(availableCoords != null)
   //         for(int i=0;i<transform.childCount;i++)
   //         {
   //             if(!transform.GetChild(i).gameObject.activeInHierarchy)
   //             {
   //                 transform.GetChild(i).position = new Vector3Int(((Vector2Int)availableCoords[0]).x,0,((Vector2Int)availableCoords[0]).y);
   //                 transform.GetChild(i).gameObject.SetActive(true);
   //                 currentTiles.Add(transform.GetChild(i).gameObject);
   //             }
   //         }
   // }

   //slanje signala child tileovima skupa sa pretrazenim bliskim koordinatama u obliku Vector2Int arraylista
   // private void checkTiles(ArrayList coords)
   // {
   //     BroadcastMessage("checkCoordinates",coords);
   // }
}
