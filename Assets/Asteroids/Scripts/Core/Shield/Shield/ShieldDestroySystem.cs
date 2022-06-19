using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class ShieldDestroySystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();

            Entities.WithAll<Shield>().ForEach((Entity entity, IsColliding isColliding) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<Meteor>(otherEntity) || HasComponent<UFO>(otherEntity) || HasComponent<UFOBullet>(otherEntity))
                {
                    commandBuffer.DestroyEntity(otherEntity);
                    commandBuffer.DestroyEntity(entity);
                }
            }).WithoutBurst().Run();
        }
    }
}