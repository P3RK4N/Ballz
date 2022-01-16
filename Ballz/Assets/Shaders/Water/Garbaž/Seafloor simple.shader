Shader "Unlit/Seafloor simple"
{
    Properties
    {
        _ColorDefault ("ColorDefault", Color) = (1,1,1,1)
        _ColorLight ("ColorLight", Color) = (1,1,1,1)
        _ColorDark ("ColorDark", Color) = (1,1,1,1)

        _upperLerp("Upper Lerp", Range(0,10)) = 0
        _lowerLerp("Lower Lerp", Range(0,10)) = 0

        _a1 ("A1", float) = 0
        _a2 ("A2", float) = 0
        _a3 ("A3", float) = 0
        _f1 ("F1", float) = 0
        _f2 ("F2", float) = 0
        _f3 ("F3", float) = 0
        _o2 ("O2", float) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPath" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR0;
            };

            fixed4 _ColorDefault;
            fixed4 _ColorDark;
            fixed4 _ColorLight;

            float _upperLerp;
            float _lowerLerp;

            float _a1;
            float _a2;
            float _a3;
            float _f3;
            float _f2;
            float _o2;
            float _f1;


            v2f vert (appdata v)
            {
                v2f o;
                float baseHeight = v.vertex.y; 
                v.vertex.y += _a1 * sin(v.vertex.x/3 * _f1) + _a2 * sin((v.vertex.x / 5 + v.vertex.z / 4) * _f2 + _o2) + _a3 * sin((v.vertex.x / 2 + v.vertex.y) * _f3);
                v.color = v.vertex.y > baseHeight ? lerp(_ColorDefault,_ColorLight,v.vertex.y-baseHeight / _upperLerp) : lerp(_ColorDefault,_ColorDark, baseHeight - v.vertex.y / _lowerLerp);
                o.vertex = UnityObjectToClipPos(v.vertex); 
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                return i.color;
            }
            ENDCG
        }
    }
}
