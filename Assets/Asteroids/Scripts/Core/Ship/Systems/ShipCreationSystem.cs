using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class ShipCreationSystem : SystemBase
    {
        private const float Cooldown = 1;

        protected override void OnCreate()
        {
            EntityManager.CreateEntity(typeof(ShipSpawnCooldown));
        }

        protected override void OnUpdate()
        {
            var shipQuery = GetEntityQuery(typeof(Ship));
            var cooldownEntity = GetEntityQuery(typeof(ShipSpawnCooldown)).GetSingletonEntity();

            if (HasComponent<IsCooldownComplete>(cooldownEntity))
            {
                CreateShip();
            }

            if (shipQuery.IsEmpty && !HasComponent<Cooldown>(cooldownEntity))
            {
                EntityManager.AddComponentData(cooldownEntity, new Cooldown() { Value = Cooldown });
            }

            var doResetQuery = GetEntityQuery(typeof(DoReset));
            if (!doResetQuery.IsEmpty)
            {
                if (HasComponent<Cooldown>(cooldownEntity))
                    EntityManager.SetComponentData(cooldownEntity, new Cooldown() { Value = Cooldown });
            }
        }

        private void CreateShip()
        {
            var shipSettings = Settings.Instance.Ship;

            var entityManager = EntityManager;

            var shipEntity = entityManager.CreateEntity(typeof(Ship),
                typeof(Velocity),
                typeof(Warpable),
                typeof(FacingDirection),
                typeof(Collidable),
                typeof(ShootAmount));
            var sizeFactor = shipSettings.SizeFactor;

            EntityManager.AddComponent<CanShoot>(shipEntity);
            EntityManager.AddComponentData(shipEntity, new ShootAmount() { Value = 1 });

            EntityCreationHelper.AddBaseComponents(shipEntity,
                entityManager,
                shipSettings.StartingPosition,
                shipSettings.ColorID,
                new float2(0, 0.5f) * sizeFactor,
                new float2(0.5f, -0.5f) * sizeFactor,
                new float2(0.3f, -0.35f) * sizeFactor,
                new float2(-0.3f, -0.35f) * sizeFactor,
                new float2(-0.5f, -0.5f) * sizeFactor
                );
        }
    }
}