using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int id = 0;
    int idCounter = 0;
    float riseSpeed = 5f;
    bool lowered = false;
    public int conditionNumber;
    Renderer r;

    public Color[] colors;
    public float changeTime;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponentInChildren<Renderer>();
        transform.localPosition -= new Vector3(0, 1f, 0);
    }

    public void colorTile(int colorCount)
    {
        if(id < 0) return;
        if(id == 0) 
        {
            int pos = UnityEngine.Random.Range(0, colors.Length);
            id = pos;
        }
        else id--;
        id = id % colorCount;
        GetComponent<Renderer>().material.color = colors[id];
    }

    public IEnumerator rise()
    {
        lowered = false;
        float i = 0f;
        float h;
        while(i<1)
        {
            i += Time.deltaTime * riseSpeed;
            h = Mathf.Lerp(-1f, 0f, i);
            transform.localPosition = new Vector3(0, h, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator lower()
    {
        lowered = true;
        float i = 0f;
        float h;
        while(i<1)
        {
            i += Time.deltaTime * riseSpeed;
            h = Mathf.Lerp(0f, -1f, i);
            transform.localPosition = new Vector3(0, h, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator periodicRising()
    {
        int colorCount = colors.Length;
        while(id >= 0)
        {
            bool condition = colorCount < 5 ? 
                (idCounter % colorCount == id) || ((idCounter + 1) % colorCount == id) :
                (idCounter % colorCount == id) || ((idCounter + 1) % colorCount == id) || ((idCounter + conditionNumber) % colorCount == id);
            if(condition && lowered) StartCoroutine(rise());
            else if(!condition && !lowered) StartCoroutine(lower());
            idCounter++;
            yield return new WaitForSeconds(changeTime);
        }
    }
}
