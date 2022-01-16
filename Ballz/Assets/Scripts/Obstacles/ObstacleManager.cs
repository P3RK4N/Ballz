using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] List<GameObject> listaLevela = new List<GameObject>();

    List<int> nedavniLeveli = new List<int>();
    int lastNLevels = 0;

    private void Start() {
        if(lastNLevels >= listaLevela.Count) lastNLevels = 0;
    }
    //rand generacija, stavljanje u listu nedavnih
    public GameObject findObstacle()
    {
        while(true){
            GameObject level = Instantiate(listaLevela[UnityEngine.Random.Range(0,listaLevela.Count)]);
            if(!nedavniLeveli.Contains(level.GetComponent<ObstacleID>().id) && level != null) {
                nedavniLeveli.Add(level.GetComponent<ObstacleID>().id);
                if(nedavniLeveli.Count>lastNLevels) nedavniLeveli.RemoveAt(0);
                return level;
            }
        }
    }
}
