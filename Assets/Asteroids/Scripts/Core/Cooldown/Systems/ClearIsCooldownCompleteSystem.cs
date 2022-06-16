using Unity.Entities;

namespace Asteroids.Core
{
    public class ClearIsCooldownCompleteSystem : SystemBase
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
                commandBuffer.RemoveComponent<IsCooldownComplete>(e);
            }).Run();
        }
    }
}