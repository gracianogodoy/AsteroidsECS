using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class DestroyShipSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var endSimulationSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);

            Entities.WithAll<Ship>().ForEach((Entity e, in IsColliding isColliding) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<Meteor>(otherEntity) || HasComponent<UFOBullet>(otherEntity) || HasComponent<UFO>(otherEntity))
                {
                    ecb.DestroyEntity(e);
                    ecb.DestroyEntity(otherEntity);

                    var doResetEntity = ecb.CreateEntity();
                    ecb.AddComponent(doResetEntity, new DoReset());
                }
            }).WithoutBurst().Run();
        }
    }
}