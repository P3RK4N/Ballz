using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBuilder : MonoBehaviour
{
    //Dimenzije:
    //Standardni: od -0.125 do 0.125 (sa konopom 0.05 do 0.025)
    //Pocetni: od -0.125 do 0.125 (sa konopom 0.05 do 0.025 i -0.25 do -0.5)

    [SerializeField] GameObject normalChunk;
    [SerializeField] GameObject startChunk;

    [SerializeField] [Range(0.5f,10f)] float vfxSpeed = 1f;
    [Range(-100,100)] float offset = 0;

    List<GameObject> chunks;
    GameObject bridgeEmpty;

    Vector3 startPoint;
    Vector3 endPoint;

    bool bridgeFinished = false;
    int nPlanks;

    private void offsetBridge()
    {
        //FOR EVERY PREFAB
        for(int i = 0; i<nPlanks ; i++)
        //AND EVERY SUB-PREFAB
        for(int j = 0; j < chunks[i].transform.childCount; j++)
                {
                    //OFFSET THEM FOR VFX
                    chunks[i].transform.GetChild(j).GetComponent<MeshRenderer>().materials[0].SetFloat("_Offset",offset);
                    chunks[i].transform.GetChild(j).GetComponent<MeshRenderer>().materials[1].SetFloat("_Offset",offset);
                }
    }

    //INSERT START,END POSITION , ROTATE TOWARDS END, COUNT PLANKS
    public void calculateBridge(Vector3 start, Vector3 end)
    {
            bridgeEmpty = new GameObject("Bridge");
            bridgeEmpty.transform.parent = FindObjectOfType<LevelGenerator>().currentLevel.transform;
            // Transform start = startPos.transform;
            // Transform end = endPos.transform;

            startPoint = start;
            endPoint = end;

            bridgeEmpty.transform.position = startPoint;
            bridgeEmpty.transform.LookAt(endPoint);

            float distance = Vector3.Distance(startPoint,endPoint);
            //SECURE RIGHT OFFSET
            offset = -1f-distance;
            offsetBridge();
            nPlanks = (int) (distance/0.3f) + 1;

            BuildBridge(nPlanks);

    }

    public void BuildBridge(int n)
    {  
        Debug.Log(n);
        chunks = new List<GameObject>();

        //INSTANTIATE AND TRANSFORM PLANK PREFABS
        for(int i = 0;i<n;i++)
        {
            if(i==0) chunks.Add(Instantiate(startChunk, bridgeEmpty.transform.position + bridgeEmpty.transform.forward * 0.3f * i,bridgeEmpty.transform.rotation,bridgeEmpty.transform));
            else chunks.Add(Instantiate(normalChunk, bridgeEmpty.transform.position + bridgeEmpty.transform.forward * 0.3f * i,bridgeEmpty.transform.rotation,bridgeEmpty.transform));
        }

        for(int i = 0; i<n; i++)
        {
            GameObject[] ropes = new GameObject[2];
            ropes[0] = chunks[i].transform.Find("RopeL").gameObject;
            ropes[1] = chunks[i].transform.Find("RopeR").gameObject;
            
            //SET STARTING POSITION TO EVERY GAMEOBJECT INSIDE BRIDGE (FOR VFX)
            for(int j = 0; j < chunks[i].transform.childCount; j++)
            {
                chunks[i].transform.GetChild(j).GetComponent<MeshRenderer>().materials[1].SetVector("_Position",endPoint);
                chunks[i].transform.GetChild(j).GetComponent<MeshRenderer>().materials[0].SetVector("_Position",endPoint);
            }

            //TRANSFORM ANCHOR POINTS ON EVERY PAIR OF ROPES
            foreach (GameObject rope in ropes)
            {
                if(i!=n-1) rope.GetComponents<HingeJoint>()[1].connectedBody = chunks[i+1].transform.Find("Plank").GetComponent<Rigidbody>();
                rope.GetComponents<HingeJoint>()[0].anchor -= Vector3.forward * 0.5f;
                rope.GetComponents<HingeJoint>()[1].anchor += Vector3.forward * 0.5f;
            }

            //MAKE LAST ROPE STAND IN PLACE
            if(i==n-1)
            {
                foreach (GameObject rope in ropes)
                {
                    rope.GetComponent<Rigidbody>().useGravity = false;
                    rope.GetComponents<HingeJoint>()[1].useLimits = false;
                }
            } 
            bridgeFinished = true;
        }        
    }

    private void Update() {
        //OFFSET BRIDGE VFX
        if(bridgeFinished && offset<0.7f) 
        {
            offset += Time.deltaTime * vfxSpeed;
            offsetBridge();
        }
    }
}
