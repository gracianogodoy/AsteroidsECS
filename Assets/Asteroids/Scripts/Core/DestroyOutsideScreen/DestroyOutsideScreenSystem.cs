using Unity.Entities;

namespace Asteroids.Core
{
    public class DestroyOutsideScreenSystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();

            Entities.WithAll<MustDestroyOutsideScreen>().ForEach((Entity entity, in Position position) =>
            {
                if (position.Value.x > ScreenHelper.MaxBoundX
                || position.Value.x < ScreenHelper.MinBoundX
                || position.Value.y < ScreenHelper.MinBoundY
                || position.Value.y > ScreenHelper.MaxBoundY)
                {
                    commandBuffer.DestroyEntity(entity);
                }
            }).Schedule();

            this.CompleteDependency();
        }
    }
}