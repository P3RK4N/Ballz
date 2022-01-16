using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnFloor : MonoBehaviour
{
    [SerializeField] GameObject tile;
    [SerializeField] Color[] colors;
    [SerializeField] [Range(1,9999)] public int stage = 1;
    [SerializeField] public int fullLength = 14;
    [SerializeField] public int fullWidth = 10;

    int length;
    int width;
    float changeTime;
    int colorCount;
    Color[] colorList;


    const float SPACING = 0.1f;
    bool pathMade = false;
    Vector2Int[] directions = new Vector2Int[8]
    {
        new Vector2Int(1,0),
        new Vector2Int(1,1),
        new Vector2Int(0,1),
        new Vector2Int(-1,1),
        new Vector2Int(-1,0),
        new Vector2Int(-1,-1),
        new Vector2Int(0,-1),
        new Vector2Int(1, -1),
    };

    public List<GameObject> usedTiles;
    
    public GameObject[,] tiles;
    [SerializeField] List<GameObject> pathTiles;

    private void Start() {
        calculateDifficulty();
        chooseColors();
        tiles = new GameObject[fullLength,fullWidth];
        spawn(fullLength,fullWidth);
        findPath();
        checkPath();
        StartCoroutine(colorPath());
    }

    private void chooseColors()
    {
        shuffle(colors);
        colorList = new Color[colorCount];
        for(int i = 0; i < colorCount; i++)
        {
            colorList[i] = colors[i];
        }
    }

    private void calculateDifficulty()
        {
            float difficulty = Mathf.Min(stage/25f, 1f);
            length = (int) Mathf.Lerp(7f, 14.5f, difficulty);
            width = (int) Mathf.Lerp(5f,10.5f, difficulty);
            changeTime = Mathf.Lerp(3f, 1f, difficulty);
            colorCount = (int) Mathf.Lerp(3f, 6.5f, difficulty);
        }

    private void checkPath()
    {
        if(pathTiles.Count < fullLength * 0.15f * fullWidth)
        {
            usedTiles.Clear();
            findPath();
        }
    }

    private IEnumerator colorPath()
    {
        for(int i = 0; i < pathTiles.Count; i++)
        {
            pathTiles[i].GetComponentInChildren<TileInfo>().id = i + 1;
        }
        yield return new WaitForEndOfFrame();

        //Pattern condition
            int condition = UnityEngine.Random.Range(2,colorCount);

        foreach (GameObject tile in tiles)
        {
            if(tile == null) continue;
            tile.GetComponentInChildren<TileInfo>().colors = colorList;
            tile.GetComponentInChildren<TileInfo>().changeTime = changeTime;
            tile.GetComponentInChildren<TileInfo>().colorTile(colorCount);
            tile.GetComponentInChildren<TileInfo>().conditionNumber = condition;

        }
        Debug.Log(colorCount);
    }

    public void spawn(int fullLength, int fullWidth)
    {
        float totalWidth = fullWidth + SPACING * (fullWidth - 1);
        float posY = totalWidth/2f - 0.5f;
        float posX = 0.5f + SPACING;

        float xNoise, yNoise, zNoise;

        for (int i = 0; i < fullLength; i++)
        {
            if(i < fullLength - 1 - this.length)
            {
                for (int j = 0; j < fullWidth; j++)
                {
                    xNoise = UnityEngine.Random.Range(-0.05f, 0.05f);
                    yNoise = UnityEngine.Random.Range(-0.05f, 0.05f);
                    zNoise = UnityEngine.Random.Range(-0.05f, 0.05f);

                    Vector3 pos = new Vector3(posX + (i * SPACING + i) + xNoise, yNoise, posY - (j * SPACING + j) + zNoise);
                    GameObject current = Instantiate(tile, transform);
                    current.GetComponentInChildren<TileInfo>().id = -1;
                    current.transform.localPosition = pos;
                    tiles[i,j] = current;
                }
            }
            else
            {
                int k = 0;
                for (float j = (fullWidth/2f - width/2f); k < width; k++, j++)
                {
                    xNoise = UnityEngine.Random.Range(-0.05f, 0.05f);
                    yNoise = UnityEngine.Random.Range(-0.05f, 0.05f);
                    zNoise = UnityEngine.Random.Range(-0.05f, 0.05f);

                    Vector3 pos = new Vector3(posX + (i * SPACING + i) + xNoise, yNoise, posY - (j * SPACING + j) + zNoise);
                    GameObject current = Instantiate(tile, transform);
                    current.transform.localPosition = pos;
                    tiles[i,(int)j] = current;
                }
            }
        }
    }

    struct Tile
    {
        public GameObject tile;
        public Vector2Int coordinates;
        public Queue<GameObject> path;
        public Tile(GameObject tile, Vector2Int coordinates, Queue<GameObject> path)
        {
            this.tile = tile;
            this.coordinates = coordinates;
            this.path = path;
            path.Enqueue(tile);
        }
    }

    private void findPath()
    {
        int pos = (int) (fullWidth/2f - width/2f);
        // int first = UnityEngine.Random.Range(0,fullWidth);
        int first = UnityEngine.Random.Range(pos,pos+width);
        Tile firstTile = new Tile(tiles[0,first], new Vector2Int(0, first), new Queue<GameObject>());
        usedTiles.Add(firstTile.tile);
        generatePath(firstTile);
    }

    private void generatePath(Tile previousTile)
    {
        int k = UnityEngine.Random.Range(0,8);
        int pos = (int) (fullWidth/2f - width/2f);
        int counter = 8;
        while(counter > 0)
        {
            counter--;
            Vector2Int direction = directions[k];
            k = (k+1) % 8;
            Vector2Int nextCoords = previousTile.coordinates + direction;
            if(nextCoords.x >= fullLength || nextCoords.x < fullLength-length || nextCoords.y < pos || nextCoords.y >= pos+width || (nextCoords.x == (length/2 + fullLength - length) && (nextCoords.y == pos || nextCoords.y == pos+width-1))) continue;
            GameObject nextTile = tiles[nextCoords.x, nextCoords.y];
            if(usedTiles.Contains(nextTile)) continue;
            else
            {
                usedTiles.Add(nextTile);
                Tile tile = new Tile(nextTile, nextCoords, previousTile.path);
                if(nextCoords.x == fullLength - 1) 
                {
                    pathMade = true;
                    pathTiles.Clear();
                    if(pathTiles.Count < 1)
                    {
                        GameObject pathTile;
                        while(tile.path.Count > 0)
                        {
                            pathTile = tile.path.Dequeue();
                            pathTiles.Add(pathTile);
                        } 
                    }
                    break;
                }
                if(!pathMade) generatePath(tile);
            }
        }   
    }

    static void shuffle<T>(T[] items)
    {
        for (int i = 0; i < items.Length - 1; i++)
        {
            int j = UnityEngine.Random.Range(i, items.Length);
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }
}
