Shader "Unlit/Bridge Color"
{
    Properties
    {
        _Noise ("Noise", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Offset ("Offset", Range(-100,100)) = 0.
        _Limit3 ("Limit 3", Range(0,1)) = 0.7
        _Position("Position", Vector) = (0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}
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
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _Noise;
            float4 _Noise_ST;

            fixed4 _Color;
            
            //effect offset
            float _Offset;

            //vector of starting world position
            float3 _Position;

            fixed4 _LightColor0;
            const float LIMIT = 0.657;


            fixed4 lightningEffect(float3 normalVector, float3 lightVector)
            {
                float NdotL = dot(normalize(normalVector),normalize(lightVector));
                NdotL = NdotL/2. + 0.5;
                return max(0.2,NdotL);
            }

            v2f vert (appdata v)
            {
                v2f o;
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Noise);
                o.worldPos = worldPos.xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half alpha = tex2D(_Noise, i.uv).r;
                float2 xzWorld = _Position.xz - i.worldPos.xz;
                float value = alpha + _Offset + length(xzWorld);
                fixed4 col = _Color * lightningEffect(i.normal,_WorldSpaceLightPos0.xyz);
                col.a = 1;
                return value < LIMIT ? fixed4(0,0,0,0) : col * _LightColor0;
            }
            ENDCG
        }
    }
}
