using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateBefore(typeof(CollisionSystem))]
    public partial class ClearIsCollidingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var endSimulationSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);

            Entities.ForEach((Entity e, ref IsCooldownComplete isCooldownComplete) =>
            {
                ecb.RemoveComponent<IsColliding>(e);
            }).Run();
        }
    }
}