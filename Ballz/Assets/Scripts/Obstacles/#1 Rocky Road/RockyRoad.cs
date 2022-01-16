using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyRoad : MonoBehaviour
{
    [SerializeField] public GameObject start;
    [SerializeField] public GameObject end;
    [SerializeField] GameObject cube;
    [SerializeField] List<GameObject> rock;
    [SerializeField] List<GameObject> rotator;
    [SerializeField] List<GameObject> islands;
    [SerializeField] GameObject hammer;

    [SerializeField] [Range(1,9999)] public int stage = 1;

    [SerializeField] [Range(0.01f,10f)] float frequency = 1f;
    [SerializeField] [Range(0.05f,5f)] float spacing = 1f;
    [SerializeField] [Range(0f,1f)] float perpendicularPosition = 0.07f;

    Transform startTransform;
    Transform endTransform;
    float distance;
    float amplitude;

    List<GameObject> parts;
    GameObject decorationParent;

    //FUNKCIJA
    float a1,b1,c1,a2,b2,c2,b3,c3;

    void Start()
    {
        startTransform = start.GetComponent<Transform>();
        endTransform = end.GetComponent<Transform>();
        distance = Vector3.Distance(startTransform.position,endTransform.position);
        parts = new List<GameObject>();
        createPath();

        decorationParent = new GameObject("Decorations");
        decorationParent.transform.position = startTransform.position;
        decorationParent.transform.rotation = startTransform.rotation;
        decorationParent.transform.parent = FindObjectOfType<LevelGenerator>().newLevel.transform;
        StartCoroutine(decorate());  
    }


   private void createPath()
    {
        amplitude = 7f;

        //namjestanje pocetka i kraja
        endTransform.LookAt(startTransform);
        startTransform.LookAt(endTransform);

        //potencijalno nepotrebno
        // Vector3 direction = Vector3.Normalize(endTransform.position - startTransform.position);
        // Vector2 direction2D = new Vector2(direction.x,direction.z);
        // Vector3 perpendicularDir2D = Vector2.Perpendicular(direction2D);
        // Vector3 perpendicularDir = new Vector3(perpendicularDir2D.x,0,perpendicularDir2D.y);

        // GameObject currentcube1 = Instantiate(cube, startTransform.position + perpendicularDir * amplitude,Quaternion.identity);
        // GameObject currentcube2 = Instantiate(cube, startTransform.position - perpendicularDir * amplitude,Quaternion.identity);
        // GameObject currentcube3 = Instantiate(cube, endTransform.position + perpendicularDir * amplitude,Quaternion.identity);
        // GameObject currentcube4 = Instantiate(cube, endTransform.position - perpendicularDir * amplitude,Quaternion.identity);

        //sin function y = a1 * sin(a2 * x) + b1 * cos(b2 * x + b3) + c1 * sin(c2 * x + c3)
        //sin function derived y' = a1 * cos(a2 * c) * a2 - b1 * sin(b2 * x + b3) * b2 + c1 * cos(c2 * x + c3) * c2
        //a1+b1+c1 = amplitude

        //FUNKCIJA
        int i = 0;
        float z = 0f;

        //build random function
        //amplitudes
        a1 = UnityEngine.Random.Range(amplitude * 0.2f, amplitude * 0.5f);
        b1 = UnityEngine.Random.Range(0.05f, amplitude * 0.3f);
        c1 = UnityEngine.Random.Range(0f , amplitude - a1 - b1);
        float x = a1+b1+c1;
        //frequencies
        a2 = UnityEngine.Random.Range(frequency * 0.05f * frequency, frequency * 0.2f);
        b2 = UnityEngine.Random.Range(frequency * 0.3f * frequency, frequency * 0.6f);
        c2 = UnityEngine.Random.Range(0f, frequency - a2 - b2);
        //offsets
        b3 = UnityEngine.Random.Range(1f,5.28f);
        c3 = UnityEngine.Random.Range(1f,5.28f);

        while(true && i < 250)
        {
            i++;
            //get direction from function tangent
            float alpha = calculateFunction(z);
            Vector3 nextDirection = new Vector3(Mathf.Sin(alpha),0,Mathf.Cos(alpha));
            Vector3 nextDirectionPerpendicular = new Vector3(Mathf.Cos(alpha),0,-Mathf.Sin(alpha));

            GameObject nextPart;
            nextPart = Instantiate(nextPlatform(), startTransform, false);
            Transform tfNextPart = nextPart.GetComponent<Transform>();

            //parts distance
            float partDistance = 0;
            if(parts.Count>=1) partDistance = spacing + tfNextPart.GetComponent<PlatformInfo>().radius + parts[parts.Count-1].transform.GetComponent<PlatformInfo>().radius;

            //place next part
            if(z<0.01f) tfNextPart.localPosition = nextDirection * spacing;
            else tfNextPart.localPosition = parts[i-2].transform.localPosition + nextDirection * partDistance;

            //Clamp amplitude
            // if(Mathf.Abs(tfNextPart.localPosition.x) > amplitude) 
            // {
            //     Vector3 previousPartPosition = parts[i-2].transform.localPosition;
            //     Vector3 currentPartNewPosition = new Vector3(amplitude * Mathf.Sign(previousPartPosition.x),0 , Mathf.Sqrt(partDistance*partDistance-Mathf.Pow(amplitude * Mathf.Sign(previousPartPosition.x) - previousPartPosition.x,2)) + previousPartPosition.z);

            //     tfNextPart.localPosition = currentPartNewPosition;
            // }
            
            //Perpendicular position noise
            tfNextPart.localPosition += nextDirectionPerpendicular * UnityEngine.Random.Range(-perpendicularPosition,perpendicularPosition);

            //rotate nextRock child randomly
            tfNextPart.GetChild(0).localEulerAngles = new Vector3(-90, tfNextPart.position.x * 1000f + tfNextPart.position.z * 500f, 0);

            //Set next z value, add rock, end if at goal destination
            z = tfNextPart.localPosition.z;
            parts.Add(nextPart);
            nextPart.transform.GetChild(0).gameObject.SetActive(false);
            if(tfNextPart.localPosition.z > distance - 0.3f) {modifyParts(); break;}
            
        }
    }

    //nakon instanciranja svih djelova
    private void modifyParts()
    { 
        rotateRocks();
        placeHammers();  
    }

    //nakon rocks, rotators, valjaka
    private void placeHammers()
    {
        int difficulty = 1 + (int)(stage / 4);
        for(int i = 5; i < parts.Count; i++)
        {
// needs work?
            if(parts[i].tag == "Rock" && !parts[i-1].GetComponent<PlatformInfo>().hasHammer)
            {
                GameObject trap;
                if(UnityEngine.Random.Range(0f,1f) < 0.02f * difficulty)
                {
                    trap = Instantiate(hammer, parts[i].transform, false);
                    trap.transform.SetSiblingIndex(0);
                    trap.SetActive(false);
                    parts[i].GetComponent<PlatformInfo>().hasHammer = true;
                }
            }
        }
    }

    private GameObject nextPlatform()
    {
        GameObject next;
        if(UnityEngine.Random.Range(0f,1f) < 0.9f || parts.Count < 5) next = rock[UnityEngine.Random.Range(0,rock.Count-1)];
        else next = rotator[0];
        return next;
    }

    private void rotateRocks()
    {
        for(int i = 0;i<parts.Count;i++)
        {
            if(i==0)
            {
                parts[i].transform.LookAt(parts[i+1].transform);
            }
            else if(i==parts.Count-1)
            {
                Debug.Log(i);
                parts[i].transform.LookAt(parts[i-1].transform);
                parts[i].transform.localEulerAngles = new Vector3(parts[i].transform.localEulerAngles.x,parts[i].transform.localEulerAngles.y+180,parts[i].transform.localEulerAngles.z);
            }
            else
            {
                Vector3 direction = parts[i-1].transform.position - parts[i+1].transform.position;
                direction.Normalize();
                if(direction.z < 0)
                parts[i].transform.eulerAngles = new Vector3(0, Mathf.Atan(direction.x/direction.z) * Mathf.Rad2Deg, 0);
                else
                parts[i].transform.eulerAngles = new Vector3(0, Mathf.Atan(direction.x/direction.z) * Mathf.Rad2Deg + 180f, 0);

            }
        }
    }

    float calculateFunction(float x)
    {
        float value = a1 * Mathf.Cos(a2 * x) * a2 - b1 * Mathf.Sin(b2 * x + b3) * b2 + c1 * Mathf.Cos(c2 * x + c3) * c2;
        float alpha = Mathf.Atan(value);
        return alpha;
    }

    private IEnumerator decorate()
    {
        int amount = 10;
        int attempts = 0;
        GameObject checkSphere = new GameObject("CheckSphere");
        checkSphere.transform.parent = decorationParent.transform;
        while(attempts<30 && amount > 0)
        {
            yield return new WaitForFixedUpdate();
            attempts++;
            checkSphere.transform.localPosition = new Vector3(UnityEngine.Random.Range(-amplitude*2.5f,amplitude*2.5f),5,UnityEngine.Random.Range(0,distance));
            if(!(Physics.CheckSphere(checkSphere.transform.position, 3f)))
            {
                GameObject newIsland = Instantiate(islands[UnityEngine.Random.Range(0,islands.Count)], decorationParent.transform, false);
                newIsland.transform.position = new Vector3(checkSphere.transform.position.x, 0, checkSphere.transform.position.z);
                newIsland.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0,360f), 0);
                amount--;
            }
        }
        Destroy(checkSphere);
    }
}
