void DirDis_float(in bool player, in float3 object,in float distFac, in float pressFac, out float3 offset)
{ 
   if(player)
   {
      float dist = distance(_PlayerPosition, object);
      float3 direction = normalize(_PlayerPosition-object);

      float offsetWeight = saturate(dist * distFac);
      float3 maxOffset = (-pressFac) * float3(direction.x, 0, direction.z);
      offset = lerp(maxOffset, float3(0, 0, 0), offsetWeight);
   }
   else 
   {
      offset = float3(0,0,0);
   }
}