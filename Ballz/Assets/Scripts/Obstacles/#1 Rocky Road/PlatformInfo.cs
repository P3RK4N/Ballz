using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlatformInfo : MonoBehaviour
{
    [SerializeField] public float radius;
    
    public bool hasHammer = false;
    
    VisualEffect vfxBubble;

    private void Start() {
        vfxBubble = GetComponentInChildren<VisualEffect>();
    }

    public void removeBubble()
    {
        vfxBubble.Stop();
    }
}
