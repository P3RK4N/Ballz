struct Attributes
{
   float4 positionOS : POSITION;
   float3 normalOS : NORMAL;
   float4 tangentOS : TANGENT;
   float2 uv : TEXCOORD0;
   float2 uvLM : TEXCOORD1;
   float4 color : COLOR;
   UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
   float3 normalOS : NORMAL;
   float2 uv                     : TEXCOORD0;
   float2 uvLM                   : TEXCOORD1;
   float4 positionWSAndFogFactor : TEXCOORD2;
   half3 normalWS                : TEXCOORD3;
   half3 tangentWS               : TEXCOORD4;
   float4 positionOS             : TEXCOORD5;
   float4 color : COLOR;
   #if _NORMALMAP
      half3 bitangentWS          : TEXCOORD5;
   #endif
   #ifdef _MAIN_LIGHT_SHADOWS
      float4 shadowCoord         : TEXCOORD6;
   #endif
   float4 positionCS : SV_POSITION;
};

//Properties
float _Height;
float _Base;
float4 _Color1;
float4 _Color2;
float _LightPower;
float _TPower;
float _AlphaCutoff;
float _ShadowPower;
float _WindStrength;
float3 _WindFrequency;
sampler2D _MainTex;
float4 _MainTex_ST;
sampler2D _WindDistortionMap;


//Vertex pass
Varyings LitPassVertex(Attributes input)
{
   Varyings output = (Varyings)0;
   
   output.color = input.color;

   VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS);
   VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
   float fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
   output.uv = TRANSFORM_TEX(input.uv, _MainTex);
   output.uvLM = input.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;
   output.positionWSAndFogFactor = float4(vertexInput.positionWS, fogFactor);
   // output.positionCS = vertexInput.positionCS;
   output.positionOS = input.positionOS;
   output.normalOS = input.normalOS;
   output.normalWS = vertexNormalInput.normalWS;
   output.tangentWS = vertexNormalInput.tangentWS;
   #ifdef _NORMAL_MAP
      output.bitangentWS = vertexNormalInput.bitangentWS;
   #endif
   #ifdef _MAIN_LIGHT_SHADOWS
      output.shadowCoord = GetShadowCoord(vertexInput);
   #endif
   return output;
}

float3x3 RotY(float angle)
{
   return float3x3
   (
      cos(angle), 0, sin(angle),
      0, 1, 0,
      -sin(angle), 0, cos(angle)
   );
}

float3x3 RotX(float angle)
{
   return float3x3
   (
      1,0,0,
      0,cos(angle),-sin(angle),
      0,sin(angle),cos(angle)
   );
}

float3x3 RotZ(float angle)
{
   return float3x3
   (
      cos(angle),-sin(angle),0,
      sin(angle),cos(angle),0,
      0,0,1
   );
}

float rand(float3 co)
{
    return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
}

[maxvertexcount(6)]
void LitPassGeom(triangle Varyings input[3], inout TriangleStream<Varyings> stream)
{
   float3 basePos = (input[0].positionWSAndFogFactor.xyz + input[1].positionWSAndFogFactor.xyz + input[2].positionWSAndFogFactor.xyz) / 3;
   
   float2 uv = (input[0].positionOS.xy * _Time.xy * _WindFrequency);
   float4 windSample = tex2Dlod(_WindDistortionMap, float4(uv,0,0)) * _WindStrength;

   float3 rotatedTangent = normalize(mul(input[0].tangentWS, RotY(rand(basePos) * 900)));
   float3 rotatedNormalZ = mul(input[0].normalWS, RotZ(windSample.x));
   float3 rotatedNormal = mul(rotatedNormalZ, RotX(windSample.y));
   
   Varyings o1 =  input[0];
   float3 o1Pos = (basePos - rotatedTangent * _Base);
   o1.positionCS = TransformWorldToHClip(o1Pos);
   
   Varyings o2 =  input[0];
   float3 o2Pos = (basePos + rotatedTangent * _Base);
   o2.positionCS = TransformWorldToHClip(o2Pos);
   
   Varyings o3 =  input[0];
   float3 o3Pos = (basePos + rotatedTangent * _Base + rotatedNormal * _Height);
   o3.positionCS = TransformWorldToHClip(o3Pos);
   
   Varyings o4 =  input[0];
   float3 o4Pos = (basePos - rotatedTangent * _Base + rotatedNormal * _Height);
   o4.positionCS = TransformWorldToHClip(o4Pos);

   float3 newNormal = mul(rotatedTangent, RotY(PI/2));
   o4.uv = TRANSFORM_TEX(float2(0,1), _MainTex);
   o3.uv = TRANSFORM_TEX(float2(1,1), _MainTex);
   o2.uv = TRANSFORM_TEX(float2(1,0), _MainTex);
   o1.uv = TRANSFORM_TEX(float2(0,0), _MainTex);

   o4.normalWS = newNormal;
   o3.normalWS = newNormal;
   o2.normalWS = newNormal;
   o1.normalWS = newNormal;

   stream.Append(o4);
   stream.Append(o3);
   stream.Append(o1);

   stream.RestartStrip();

   stream.Append(o3);
   stream.Append(o2);
   stream.Append(o1);

}

float4 TransformWorldToShadowCoords(float3 positionWS)
{
   half cascadeIndex =ComputeCascadeIndex(positionWS);
   return mul(_MainLightWorldToShadow[cascadeIndex], float4(positionWS, 1.0));
}

half4 LitPassFragment (Varyings input, bool vf : SV_ISFRONTFACE) : SV_TARGET
{
   half3 normalWS = input.normalWS;
   normalWS = normalize(normalWS);
   if(vf == false)
   {
      normalWS = -normalWS;
   }

   float3 positionWS = input.positionWSAndFogFactor.xyz;
   half4 color = (0, 0, 0, 1);
   Light mainLight;
   float4 shadowCoord = TransformWorldToShadowCoords(positionWS);
   mainLight = GetMainLight(shadowCoord);
   float3 normalLight = LightingLambert(mainLight.color, mainLight.direction, normalWS) * _LightPower;
   float3 inverseNormalLight = LightingLambert(mainLight.color, mainLight.direction, -normalWS) * _TPower;
   half4 light = half4(normalLight+inverseNormalLight, 1.);

   color = _Color1 + light;
   color = lerp(_Color2, color, input.uv.y);
   color = lerp(_Color2, color, clamp(mainLight.shadowAttenuation + _ShadowPower, 0, 1));
   float fogFactor = input.positionWSAndFogFactor.w;
   color = half4(MixFog(color, fogFactor),1);

   float alpha2 = tex2D(_MainTex, input.uv).a;
   clip(alpha2 - _AlphaCutoff);
   return color;
}