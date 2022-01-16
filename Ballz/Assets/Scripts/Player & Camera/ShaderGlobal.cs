using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ShaderGlobal : MonoBehaviour
{  
    void Update()
    {
        setShaderPos();
    }

    private void setShaderPos()
    {
        Shader.SetGlobalVector("_PlayerPosition", transform.position);
    }
}
