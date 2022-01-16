Shader "Unlit/Coconut"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            fixed4 _Color;

            fixed4 _LightColor0;

            fixed4 lightningEffect(float3 normalVector, float3 lightVector)
            {
                float NdotL = dot(normalize(normalVector),normalize(lightVector));
                NdotL = NdotL/2. + 0.5;
                return max(0.1,NdotL);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color * _LightColor0 * lightningEffect(i.normal, _WorldSpaceLightPos0.xyz) * unity_AmbientSky * 20;
            }
            ENDCG
        }
    }
}
