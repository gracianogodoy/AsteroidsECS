using Unity.Entities;

namespace Asteroids.Core
{
    public partial class ClearIsCooldownCompleteSystem : SystemBase
    {
       protected override void OnUpdate()
        {
            var endSimulationSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);
            
            Entities.ForEach((Entity e, ref IsCooldownComplete isCooldownComplete) =>
            {
                ecb.RemoveComponent<IsCooldownComplete>(e);
            }).Run();
        }
    }
}