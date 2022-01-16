using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerRising : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float depth = 2f;

    Transform tf;
    Vector3 startPosition;
    Vector3 endPosition;

    float i = 0f;
    bool setup = true;
    
    private void OnEnable() {
        if(setup)
        {
        tf = GetComponentInParent<Transform>();
        endPosition = tf.localPosition;
        startPosition = tf.localPosition - new Vector3(0,depth,0);
        setup = false;
        }
        StartCoroutine(rise());
    }

    private IEnumerator rise()
    {
        while(i<1f)
        {
            i += Time.deltaTime * speed;
            tf.localPosition = Vector3.Lerp(startPosition, endPosition + new Vector3(0, 0.25f, 0), i);
            yield return new WaitForEndOfFrame();
        }
        i = 0;
        while(i<1f)
        {
            i += Time.deltaTime * speed * 5f;
            tf.localPosition = Vector3.Lerp(endPosition + new Vector3(0, 0.25f, 0), endPosition, i);
            yield return new WaitForEndOfFrame();
        }

        tf.GetChild(1).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }
}
