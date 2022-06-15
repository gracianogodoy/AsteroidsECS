using Unity.Entities;

namespace Asteroids.Core
{
    public class MoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((ref Position position,
                in Velocity velocity) =>
            {
                position.Value += velocity.Value * deltaTime;
            }).ScheduleParallel();
        }
    }
}