using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class DestroyShipSystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();

            Entities.WithAll<Ship>().ForEach((Entity e, in IsColliding isColliding) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<Meteor>(otherEntity) || HasComponent<UFOBullet>(otherEntity) || HasComponent<UFO>(otherEntity))
                {
                    commandBuffer.DestroyEntity(e);
                    commandBuffer.DestroyEntity(otherEntity);

                    var doResetEntity = commandBuffer.CreateEntity();
                    commandBuffer.AddComponent(doResetEntity, new DoReset());
                }
            }).WithoutBurst().Run();
        }
    }
}