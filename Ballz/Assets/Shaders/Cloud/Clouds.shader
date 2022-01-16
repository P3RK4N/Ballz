Shader "Unlit/Clouds"
{
    Properties
    {
        _Noise ("Noise", 2D) = "white" {}
        _Strength ("Strength", Range(0,5)) = 1.
        _UpperLimit ("Upper Husk Limit", Range(0,1)) =0.5
        _LowerLimit ("Lower Husk Limit", Range(0,1)) =0
        _DisplacementOffset("Displacement Offset", Range(-5,5)) = 0.5
        _DisplacementSpeed("Displacement Speed",Range(0,1)) = 1
        _FlowAngle("Flow angle",Range(0,360)) = 0
        _Offset("O",float) = 0
        _Yoffset("Y offset", float) = 0
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _Noise;
            float4 _Noise_ST;
            float _Strength;
            float _UpperLimit;
            float _LowerLimit;
            float _DisplacementOffset;
            float _DisplacementSpeed;
            float _FlowAngle;
            float _Offset;
            float _Yoffset;
            

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.uv = v.uv;
                v.vertex.xyz += normalize(v.normal) * _DisplacementOffset + v.normal * pow(tex2Dlod(_Noise,half4(v.uv + _Time[1] * _DisplacementSpeed,0,0)),0.3) * _Strength;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldPos = worldPos.xyz;
                return o;
            }
    
            fixed4 frag (v2f i) : SV_Target
            {
                float2 vec = float2(cos(_FlowAngle/180. * 3.14),sin(_FlowAngle/180. * 3.14));
                fixed4 col = tex2D(_Noise, i.uv + float2(0,_Yoffset) + vec * _Time[1] * _DisplacementSpeed);

                float3 noise = tex2D(_Noise, i.uv + float2(0,_Yoffset) + vec * _Time[1] * _DisplacementSpeed);
                col.a =  (noise < _UpperLimit && noise > _LowerLimit) ? 0 : 1;
                return col;
            }
            ENDCG
        }
    }
}
