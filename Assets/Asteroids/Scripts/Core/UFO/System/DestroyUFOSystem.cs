using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class DestroyUFOSystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();

            Entities.ForEach((Entity e, in IsColliding isColliding, in UFO ufo) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<ShipBullet>(otherEntity))
                {
                    commandBuffer.DestroyEntity(e);
                    commandBuffer.DestroyEntity(otherEntity);
                }
            }).WithoutBurst().Run();
        }
    }
}