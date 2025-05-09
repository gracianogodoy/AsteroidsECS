using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class MultiShootSystem : SystemBase
    {
        private SystemHandle commandBufferSystem;
        private MultiShootSettings settings;

        protected override void OnCreate()
        {
            settings = Settings.Instance.MultiShoot;
        }

        protected override void OnUpdate()
        {
            var endSimulationSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);
            
            Entities.WithAll<Powerup, MultiShootPowerup>().ForEach((Entity entity, IsColliding isColliding) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<Ship>(otherEntity))
                {
                    var shootAmount = GetComponent<ShootAmount>(otherEntity);

                    shootAmount.Value += 1;

                    if (shootAmount.Value > settings.MaxShootAmount)
                        shootAmount.Value = settings.MaxShootAmount;

                    ecb.SetComponent(otherEntity, new ShootAmount() { Value = shootAmount.Value });
                    ecb.DestroyEntity(entity);
                }
            }).WithoutBurst().Run();
        }
    }
}