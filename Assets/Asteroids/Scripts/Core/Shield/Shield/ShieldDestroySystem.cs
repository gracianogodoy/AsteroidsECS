using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class ShieldDestroySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var endSimulationSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);;

            Entities.WithAll<Shield>().ForEach((Entity entity, IsColliding isColliding) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<Meteor>(otherEntity) || HasComponent<UFO>(otherEntity) || HasComponent<UFOBullet>(otherEntity))
                {
                    ecb.DestroyEntity(otherEntity);
                    ecb.DestroyEntity(entity);
                }
            }).WithoutBurst().Run();
        }
    }
}