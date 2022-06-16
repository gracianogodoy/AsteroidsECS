using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class RotateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var bufferFromEntity = GetBufferFromEntity<Points>();
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((Entity entity,
                ref Rotation rotation,
                in RotateSpeed rotateSpeed) =>
            {
                var pointsBuffer = bufferFromEntity[entity];

                var angle = math.radians(rotateSpeed.Value) * deltaTime;
                rotation.Value += angle;

                for (int i = 0; i < pointsBuffer.Length; i++)
                {
                    var point = pointsBuffer[i];
                    var x = point.Value.x;
                    var y = point.Value.y;

                    var ca = math.cos(angle);
                    var sa = math.sin(angle);

                    var newValue = new float2(x * ca - y * sa, x * sa + y * ca);

                    pointsBuffer.ElementAt(i).Value = newValue;
                }
            }).Schedule();

            this.CompleteDependency();
        }
    }
}