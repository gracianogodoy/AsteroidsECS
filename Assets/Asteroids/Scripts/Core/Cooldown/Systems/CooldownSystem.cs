using Unity.Entities;

namespace Asteroids.Core
{
    public class CooldownSystem : SystemBase
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

            Entities.ForEach((Entity e, ref Cooldown cooldown) =>
            {
                cooldown.Value -= deltaTime;

                if (cooldown.Value <= 0)
                {
                    cooldown.Value = 0;
                    commandBuffer.RemoveComponent<Cooldown>(e);
                    commandBuffer.AddComponent<IsCooldownComplete>(e);
                }
            }).Run();
        }
    }
}