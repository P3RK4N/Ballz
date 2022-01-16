Shader "Unlit/Bridge"
{
    Properties
    {
        _Noise ("Noise", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _DissolveColor1 ("Dissolve Color 1", Color) = (1,1,1,1)
        _DissolveColor2 ("Dissolve Color 2", Color) = (1,1,1,1)
        _DissolveColor3 ("Dissolve Color 3", Color) = (1,1,1,1)
        _Offset ("Offset", Range(-100,100)) = 0.
        _Limit ("Limit 1", Range(0,1)) = 0.3
        _Limit2 ("Limit 2", Range(0,1)) = 0.5
        _Limit3 ("Limit 3", Range(0,1)) = 0.7
        _DissolveColor12Offset("Dissolve Color 12 Offset", Range(-1,1)) = 0
        _DissolveColor23Offset("Dissolve Color 23 Offset", Range(-1,1)) = 0

        _Displacement("Displacement",Range(0,10)) = 1

        _Position("Position", Vector) = (0,0,0)
        //_Vector("Vector", Vector) = (0,0,0)
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
            fixed4 _DissolveColor1;
            fixed4 _DissolveColor2;
            fixed4 _DissolveColor3;
            float _Limit1;
            float _Limit2;
            float _Limit3;
            float _DissolveColor12Offset;
            float _DissolveColor23Offset;

            float _Offset;
            float _Displacement;

            float3 _Position;
            //float3 _Vector;

            fixed4 _LightColor0;

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
                // half alpha = tex2Dlod(_Noise, float4(v.uv,0,0)).r;
                // float2 xzWorld = _Position.xz - worldPos.xz;
                // float value = alpha + _Offset + length(xzWorld);

                // if(value < _Limit3 && value >= 0) v.vertex.xyz += v.normal * _Displacement * (_Limit3 - value);

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
                fixed4 col = fixed4(1,1,1,1);
                fixed4 _DissolveColor3Light = _DissolveColor3 * lightningEffect(i.normal,_WorldSpaceLightPos0) * _LightColor0;
                _DissolveColor3Light.a = 1;
                //fire colors transition
                col = value < 0 ? fixed4(0,0,0,0) : value < _Limit1 ? _DissolveColor1 : value < _Limit2 ? lerp(_DissolveColor1,_DissolveColor2, (value-_Limit1 + _DissolveColor12Offset)/(_Limit2-_Limit1)) : value < _Limit3 ? lerp(_DissolveColor2,_DissolveColor3Light,(value-_Limit2 + _DissolveColor23Offset)/(_Limit3-_Limit2)) : fixed4(0,0,0,0);
                return col;
            }
            ENDCG
        }
    }
}
