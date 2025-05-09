using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class DestroyUFOSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var endSimulationSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);

            Entities.ForEach((Entity e, in IsColliding isColliding, in UFO ufo) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<ShipBullet>(otherEntity))
                {
                    ecb.DestroyEntity(e);
                    ecb.DestroyEntity(otherEntity);
                }
            }).WithoutBurst().Run();
        }
    }
}