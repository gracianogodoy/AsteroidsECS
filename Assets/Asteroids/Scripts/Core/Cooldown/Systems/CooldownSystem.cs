using Unity.Entities;

namespace Asteroids.Core
{
    public partial class CooldownSystem : SystemBase
    {
       protected override void OnUpdate()
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var endSimulationSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);

            Entities.ForEach((Entity e, ref Cooldown cooldown) =>
            {
                cooldown.Value -= deltaTime;

                if (cooldown.Value <= 0)
                {
                    cooldown.Value = 0;
                    ecb.RemoveComponent<Cooldown>(e);
                    ecb.AddComponent<IsCooldownComplete>(e);
                }
            }).Run();
        }
    }
}