using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public partial class RotateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var bufferFromEntity = SystemAPI.GetBufferLookup<Points>();
            var deltaTime = SystemAPI.Time.DeltaTime;

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
                    var newValue = VectorHelper.Rotate(angle, point.Value);

                    pointsBuffer.ElementAt(i).Value = newValue;
                }
            }).Schedule();

            this.CompleteDependency();
        }
    }
}