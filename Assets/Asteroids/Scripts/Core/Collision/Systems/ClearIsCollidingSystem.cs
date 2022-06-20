using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateBefore(typeof(CollisionSystem))]
    public class ClearIsCollidingSystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();

            Entities.ForEach((Entity e, ref IsCooldownComplete isCooldownComplete) =>
            {
                commandBuffer.RemoveComponent<IsColliding>(e);
            }).Run();
        }
    }
}