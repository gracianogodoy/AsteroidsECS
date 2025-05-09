using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public partial class ShipShootSystem : SystemBase
    {
        private ShipSettings settings;

        protected override void OnUpdate()
        {
            settings = Settings.Instance.Ship;

            var shipQuery = GetEntityQuery(typeof(Ship));
            var shootInputQuery = GetEntityQuery(typeof(ShootInput));

            var doResetQuery = GetEntityQuery(typeof(DoReset));

            if (shipQuery.IsEmpty || !doResetQuery.IsEmpty)
            {
                if (!shootInputQuery.IsEmpty)
                    EntityManager.DestroyEntity(shootInputQuery.GetSingletonEntity());

                return;
            }

            var shipEntity = shipQuery.GetSingletonEntity();

            if (HasComponent<IsCooldownComplete>(shipEntity))
            {
                EntityManager.AddComponent<CanShoot>(shipEntity);
            }

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

            var shootAmount = GetComponent<ShootAmount>(shipEntity);

            var lastAngle = 0.0f;
            var angleBetween = math.radians(30);

            for (int i = 0; i < shootAmount.Value; i++)
            {
                var direction = facingDirection.Value;

                var angle = i % 2 == 0 ? -i * angleBetween : i * angleBetween;

                lastAngle += angle;

                direction = VectorHelper.Rotate(lastAngle, direction);

                direction = math.normalize(direction);

                CreateBullet(direction, position.Value);
            }
        }

        private void CreateBullet(float2 direction, float2 position)
        {
            var entityManager = EntityManager;

            var bulletEntity = entityManager.CreateEntity(typeof(Velocity), typeof(MustDestroyOutsideScreen), typeof(ShipBullet), typeof(Collider));

            entityManager.SetComponentData(bulletEntity, new Velocity() { Value = direction * settings.BulletSpeed });
            EntityManager.SetComponentData(bulletEntity, new Collider() { Layer = 1 });

            EntityCreationHelper.AddBaseComponents(bulletEntity,
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