using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class DestroyOutsideScreenSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var endSimulationSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);

            Entities.WithAll<MustDestroyOutsideScreen>().ForEach((Entity entity, in Position position) =>
            {
                if (position.Value.x > ScreenHelper.MaxBoundX
                || position.Value.x < ScreenHelper.MinBoundX
                || position.Value.y < ScreenHelper.MinBoundY
                || position.Value.y > ScreenHelper.MaxBoundY)
                {
                    ecb.DestroyEntity(entity);
                }
            }).Schedule();

            this.CompleteDependency();
        }
    }
}