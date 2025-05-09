using Unity.Entities;

namespace Asteroids.Core
{
    public partial class ResetSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var doResetQuery = GetEntityQuery(typeof(DoReset));
            if (doResetQuery.IsEmpty)
                return;

            var endSimulationSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);

            Entities.WithAll<Resetable>().ForEach((Entity e) =>
            {
                ecb.DestroyEntity(e);
            }).WithoutBurst().Run();

            EntityManager.DestroyEntity(doResetQuery.GetSingletonEntity());
        }
    }
}