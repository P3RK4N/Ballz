Shader "Unlit/Rock"
{
    Properties
    {
        _Color1("Color 1", Color) = (1,1,1,1)
        _Color2("Color 2", Color) = (1,1,1,1)
        _Noise("Moss noise", 2D) = "white" {}
        _Offset("Offset", Range(0,1)) = 0.5
        _Offset2("Offset2", Range(0,1)) = 0.5

        
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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 worldPos : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD1;
            };

            fixed4 _Color1;
            fixed4 _Color2;
            float _Offset;
            float _Offset2;
            sampler2D _Noise;

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
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldPos = worldPos.xyz;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col1 = _Color1 * lightningEffect(i.normal, _WorldSpaceLightPos0.xyz);
                fixed4 col2 = tex2D(_Noise, i.uv).g < _Offset  ? col1 : _Color2;
                
                float c = 1- dot(i.normal,float3(0,1,0));

                fixed4 color = (c<_Offset2) ? col1 : col2;
                color.a = 1;


                fixed4 light = _LightColor0;
                light.x = light.x < 0.2 ? 0.2 : light.x;
                light.y = light.y < 0.2 ? 0.2 : light.y;
                light.z = light.z < 0.2 ? 0.2 : light.z;
                
                return  color * light;
            }
            ENDCG
        }
    }
}
