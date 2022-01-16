Shader "Unlit/MyGrass"
{
    Properties
    {
        [Header(Color)]
        _Color1("Color 1", Color) = (0,0,0,1)
        _Color2("Color 2", Color) = (0,0,0,1)
        _ColorOffset("Color Offset",Range(-1,1)) = 0

        [Space]
        [Header(Movement)]
        _SwayFactor("Sway Factor", Range(1,10)) = 1
        _Angle("Angle",Range(0,360)) = 0
        _SwayDistance("Sway starting Distance", Range(0,1)) = 1
        _CollisionFactor("Collision Factor",Range(0,10)) = 1

        [Space]
        [Header(Data)]
        _Position("Position", Vector) = (0,0,0)
        _Height("Height", float) = 0
        _Distance("Character Distance", float) = 0
        _PlayerVector("Player Vector", Vector) = (0,0,0)
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
                float3 tangent : TANGENT;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 normal : NORMAL;
            };

            fixed4 _Color1;
            fixed4 _Color2;


            float3 _Position;
            float _Height;
            float _Distance;
            float2 _PlayerVector;
            float _SwayDistance;

            float _SwayFactor;
            float _CollisionFactor;
            float _Angle;
            float _ColorOffset;

            fixed4 _LightColor0;

            fixed4 lightningEffect(float3 normalVector, float3 lightVector)
            {
                float NdotL = dot(normalize(normalVector),normalize(lightVector));
                NdotL = NdotL/2.+0.5;
                return max(0.2,NdotL);
            }

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f,o);

                float angle = _Angle / 180 * 3.14;
                float2 vec = float2(cos(angle),sin(angle));

                        float3 startPos = v.vertex.xyz;
                        float3 posPlusTan = startPos + 0.01 * v.tangent;
                        float3 biTangent = cross(v.normal,v.tangent);
                        float3 posPlusBiTan = startPos + 0.01 * biTangent;

                //Vertex wind Displacement
                v.vertex.xz = v.vertex.xz +  vec * sin(_Time[1] + _Position.x * _Position.z) * v.vertex.y/_Height/_SwayFactor * 1.5;

                //normal
                posPlusTan.xz = posPlusTan.xz + vec * sin(_Time[1] + _Position.x * _Position.z) * posPlusTan.y/_Height/_SwayFactor * 1.5;
                posPlusBiTan.xz = posPlusBiTan.xz + vec * sin(_Time[1] + _Position.x * _Position.z) * posPlusBiTan.y/_Height/_SwayFactor * 1.5;


                //+Vertex collision Displacement 
                if(_Distance<_SwayDistance)
                    {
                        v.vertex.xz = lerp(v.vertex.xz,v.vertex.xz +_PlayerVector * _SwayDistance * v.vertex.y/_Height/_CollisionFactor,(_SwayDistance-_Distance)/_SwayDistance);
                        v.vertex.y = v.vertex.y - (_SwayDistance-_Distance) * v.vertex.y/_Height/_CollisionFactor;

                        //normal
                        posPlusTan.xz = lerp(posPlusTan.xz,posPlusTan.xz +_PlayerVector * _SwayDistance * posPlusTan.y/_Height/_CollisionFactor,(_SwayDistance-_Distance)/_SwayDistance);
                        posPlusTan.y = posPlusTan.y - (_SwayDistance-_Distance) * posPlusTan.y/_Height/_CollisionFactor;

                        posPlusBiTan.xz = lerp(posPlusBiTan.xz,posPlusBiTan.xz +_PlayerVector * _SwayDistance * posPlusBiTan.y/_Height/_CollisionFactor,(_SwayDistance-_Distance)/_SwayDistance);
                        posPlusBiTan.y = posPlusBiTan.y - (_SwayDistance-_Distance) * posPlusBiTan.y/_Height/_CollisionFactor;
                    }

                //new normal
                float3 modifiedTangent = posPlusTan - v.vertex.xyz;
                float3 modifiedBitangent = posPlusBiTan - v.vertex.xyz;
                float3 newNormal = cross(modifiedTangent,modifiedBitangent);    

                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(normalize(newNormal));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 color = lerp(_Color1,_Color2,(i.worldPos.y-_Position.y)/_Height + _ColorOffset);
                return fixed4(color,1) * lightningEffect(i.normal,_WorldSpaceLightPos0.xyz) * _LightColor0 * unity_AmbientSky * 8;
            }
            ENDCG
        }
    }
}
