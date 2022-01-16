Shader "Unlit/Island - small"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Color2("Color2", Color) = (1,1,1,1)
        _fac("Factor", float) = 1
        _facDepth("FactorDepth", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float3 worldPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            fixed4 _Color;
            fixed4 _Color2;
            fixed4 _LightColor0;
            float _fac;
            float _facDepth;

            fixed4 lightningEffect(float3 normalVector, float3 lightVector)
            {
                float NdotL = dot(normalize(normalVector),normalize(lightVector));
                NdotL = NdotL/2.+0.5;
                return max(0.2,NdotL);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = lerp(_Color, _Color2, saturate(-i.worldPos.y / _facDepth));
                return col * _LightColor0 * lightningEffect(i.normal, _WorldSpaceLightPos0.xyz) * _fac;
            }
            ENDCG
        }
    }
}
