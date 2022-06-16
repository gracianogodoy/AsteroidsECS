using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class MeteorSpawnSystem : SystemBase
    {
        private MeteorSettings settings;

        protected override void OnCreate()
        {
            settings = Settings.Instance.Meteor;

            SpawnMeteors();
        }

        private void SpawnMeteors()
        {
            for (int i = 0; i < settings.SpawnAmount; i++)
            {
                var spawnPosition = new float2(UnityEngine.Random.Range(ScreenHelper.MinBoundX, ScreenHelper.MaxBoundX),
                    UnityEngine.Random.Range(ScreenHelper.MinBoundY, ScreenHelper.MaxBoundY));

                var randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

                var direction = new float2(randomDirection.x, randomDirection.y);

                var speed = UnityEngine.Random.Range(settings.MinSpeed, settings.MaxSpeed);

                SpawnSingle(spawnPosition, direction, speed, Meteor.MeteorSize.Big);
            }
        }

        private void SpawnSingle(float2 spawnPosition, float2 direction, float speed, Meteor.MeteorSize size)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var shipEntity = entityManager.CreateEntity(typeof(Meteor), typeof(Velocity));

            entityManager.SetComponentData(shipEntity, new Meteor() { Size = size });
            entityManager.SetComponentData(shipEntity, new Velocity() { Value = direction * speed });

            float sizeFactor = GetSizeFactor(size);

            float minAdjustmentMulplier = 0.6f;
            float maxAdjustmentMulplier = 1.2f;

            EntityCreationHelper.AddViewComponents(shipEntity,
                entityManager,
                spawnPosition,
                settings.ColorID,
                new float2(-0.19f, 0.5f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier),
                new float2(0.19f, 0.5f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier),
                new float2(0.5f, 0.19f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier),
                new float2(0.5f, -0.19f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier),
                new float2(0.19f, -0.5f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier),
                new float2(-0.19f, -0.5f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier),
                new float2(-0.5f, -0.19f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier),
                new float2(-0.5f, 0.19f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier)
                );
        }

        private float GetSizeFactor(Meteor.MeteorSize size)
        {
            switch (size)
            {
                case Meteor.MeteorSize.Small:
                    return settings.SmallSizeFactor;

                case Meteor.MeteorSize.Medium:
                    return settings.MediumSizeFactor;

                case Meteor.MeteorSize.Big:
                    return settings.BigSizeFactor;

                default:
                    return 0;
            }
        }

        protected override void OnUpdate()
        {
        }
    }
}