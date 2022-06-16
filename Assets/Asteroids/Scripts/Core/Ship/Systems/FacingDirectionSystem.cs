using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class ShipFacingDirection : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref FacingDirection facingDirection,
                in Rotation rotation) =>
            {
                var rotationValue = rotation.Value + math.radians(90);

                facingDirection.Value = new float2(math.cos(rotationValue), math.sin(rotationValue));
            }).Run();
        }
    }
}