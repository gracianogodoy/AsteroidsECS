using Unity.Entities;

namespace Asteroids.Core
{
    public partial class MoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            Entities.ForEach((ref Position position,
                in Velocity velocity) =>
            {
                position.Value += velocity.Value * deltaTime;
            }).ScheduleParallel();
        }
    }
}