//Object position
//Player position
//Distance factor

//Offset

void RadialPlayerSway_float(in bool Player, in float3 worldVertexPos, in float distFac, in float offsetFac, out float offset)
{
   if(Player)
   {
      float dist = distance(_PlayerPosition, worldVertexPos);
      float maxOffset = -offsetFac;
      offset = lerp(maxOffset, 0, saturate(dist * distFac));
   }
   else
   {
      offset = 0;
   }
}