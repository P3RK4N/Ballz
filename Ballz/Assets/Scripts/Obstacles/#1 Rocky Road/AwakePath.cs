using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakePath : MonoBehaviour
{

   public IEnumerator activateParts()
   {
        foreach (Transform part in transform)
        {
            if(part.tag == "Rock" && part.childCount == 3)  part.GetChild(1).gameObject.SetActive(true);
            else part.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }
   }

   public IEnumerator activateHammers()
   {
       foreach (Transform part in transform)
      {
          if(part.tag == "Rock" && part.childCount == 3)
          {
              part.GetChild(0).gameObject.SetActive(true);
              yield return new WaitForSeconds(0.2f);
          }
          yield return new WaitForSeconds(0.01f);
      }
   }


   public void initialize() {
       {
            StartCoroutine(activateParts());     
            StartCoroutine(activateHammers());
       }
   }
}
