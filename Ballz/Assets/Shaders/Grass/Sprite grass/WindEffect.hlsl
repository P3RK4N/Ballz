
void WindEffect_float(in float swayFactor, in float distanceFactor, in float3 WorldPosition,in float positionFactor, in bool player, out float3 displacement)
{
   if(player)
   {
      float2 distanceVector2 = normalize(WorldPosition.xz - _PlayerPosition.xz);
      float3 distanceVector = float3(distanceVector2.x, 0.0, distanceVector2.y);
      float dis = distance(WorldPosition, _PlayerPosition);
      displacement = lerp(distanceVector * swayFactor * positionFactor, float3(0,0,0), clamp(dis * distanceFactor, 0, 1));
   }
   else displacement = float3(0,0,0);
}