using Unity.Entities;

namespace Asteroids.Core
{
    public class ResetSystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            commandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var doResetQuery = GetEntityQuery(typeof(DoReset));
            if (doResetQuery.IsEmpty)
                return;

            var commandBuffer = commandBufferSystem.CreateCommandBuffer();

            Entities.WithAll<Resetable>().ForEach((Entity e) =>
            {
                commandBuffer.DestroyEntity(e);
            }).WithoutBurst().Run();

            EntityManager.DestroyEntity(doResetQuery.GetSingletonEntity());
        }
    }
}