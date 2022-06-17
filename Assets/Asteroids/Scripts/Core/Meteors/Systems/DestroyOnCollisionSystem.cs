using Unity.Entities;

namespace Asteroids.Core
{
    public class DestroyOnCollisionSystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            commandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();

            Entities.ForEach((Entity e, ref IsColliding isColliding) =>
            {
                commandBuffer.DestroyEntity(e);
            }).Run();
        }
    }
}