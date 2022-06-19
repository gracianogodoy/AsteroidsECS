using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class MultiShootSystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;
        private MultiShootSettings settings;

        protected override void OnCreate()
        {
            settings = Settings.Instance.MultiShoot;
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();
            Entities.WithAll<Powerup, MultiShoot>().ForEach((Entity entity, IsColliding isColliding) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<Ship>(otherEntity))
                {
                    var shootAmount = GetComponent<ShootAmount>(otherEntity);

                    shootAmount.Value += 1;

                    if (shootAmount.Value > settings.MaxShootAmount)
                        shootAmount.Value = settings.MaxShootAmount;

                    commandBuffer.SetComponent(otherEntity, new ShootAmount() { Value = shootAmount.Value });
                    commandBuffer.DestroyEntity(entity);
                }
            }).WithoutBurst().Run();
        }
    }
}