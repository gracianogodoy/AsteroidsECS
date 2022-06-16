using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class ShipShootSystem : SystemBase
    {
        private ShipSettings settings;

        protected override void OnStartRunning()
        {
            var shipEntity = GetEntityQuery(typeof(Ship)).GetSingletonEntity();
            EntityManager.AddComponent<CanShoot>(shipEntity);
        }

        protected override void OnUpdate()
        {
            settings = Settings.Instance.Ship;

            var shipEntity = GetEntityQuery(typeof(Ship)).GetSingletonEntity();

            if (HasComponent<IsCooldownComplete>(shipEntity))
            {
                EntityManager.AddComponent<CanShoot>(shipEntity);
            }

            var shootInputQuery = GetEntityQuery(typeof(ShootInput));

            if (shootInputQuery.IsEmpty)
                return;

            var shootInputEntity = shootInputQuery.GetSingletonEntity();
            EntityManager.DestroyEntity(shootInputEntity);

            if (!HasComponent<CanShoot>(shipEntity))
                return;

            EntityManager.RemoveComponent<CanShoot>(shipEntity);
            EntityManager.AddComponentData(shipEntity, new Cooldown() { Value = settings.ShootCooldown });

            var facingDirection = GetComponent<FacingDirection>(shipEntity);

            var position = GetComponent<Position>(shipEntity);

            CreateBullet(facingDirection.Value, position.Value);
        }

        private void CreateBullet(float2 direction, float2 position)
        {
            var entityManager = EntityManager;

            var bulletEntity = entityManager.CreateEntity(typeof(Velocity), typeof(MustDestroyOutsideScreen));

            entityManager.SetComponentData(bulletEntity, new Velocity() { Value = direction * settings.BulletSpeed });

            EntityCreationHelper.AddViewComponents(bulletEntity,
                entityManager,
                position,
                settings.BulletColorID,
                new float2(0, 0.05f),
                new float2(0.05f, 0),
                new float2(0, -0.05f),
                new float2(-0.05f, 0)
                );
        }
    }
}