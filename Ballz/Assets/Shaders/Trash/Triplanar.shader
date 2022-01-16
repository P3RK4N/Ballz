Shader "Unlit/Triplanar"
{
    Properties
    {
        _Noise("Noise", 2D) = "white" {}
        _Sharpness("Sharpness", Range(1,10)) = 1.
        _FlowSpeed("Flow Speed", Range(0,10)) = 0.5
        _MovingTopWorldFactor("Moving top world factor", Range(0,9.5)) = 5.
        _MovingTopTimeFactor("Moving top time factor",Range(0,10)) = 1.
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
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float3 worldPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            sampler2D _Noise;
            float4 _Noise_ST;
            float _Sharpness;
            float _FlowSpeed;
            float _MovingTopWorldFactor;
            float _MovingTopTimeFactor;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldPos = worldPos;
                float3 worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
                o.normal = normalize(worldNormal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv_front = TRANSFORM_TEX(i.worldPos.xy, _Noise);
                float2 uv_side = TRANSFORM_TEX(i.worldPos.zy, _Noise);
                float2 uv_top = TRANSFORM_TEX(i.worldPos.xz + float2(i.worldPos.y/(10-_MovingTopWorldFactor),i.worldPos.y/(10-_MovingTopWorldFactor)), _Noise);

                float3 weights = abs(i.normal);
                weights /= weights.x+weights.y+weights.z;
                weights = pow(weights,_Sharpness);

                fixed4 col_front = weights.z * tex2D(_Noise, uv_front + float2(0,_Time[1]) * _FlowSpeed);
                fixed4 col_side = weights.x * tex2D(_Noise, uv_side + float2(0,_Time[1]) * _FlowSpeed);
                fixed4 col_top = weights.y * tex2D(_Noise, uv_top + _Time[1] * _MovingTopTimeFactor);

                fixed4 col = (col_front+col_side+col_top);
                return col;
            }
            ENDCG
        }
    }
}
