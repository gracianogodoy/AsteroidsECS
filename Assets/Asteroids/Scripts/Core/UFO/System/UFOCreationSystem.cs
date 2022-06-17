using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class UFOCreationSystem : SystemBase
    {
        private UFOSettings settings;

        protected override void OnCreate()
        {
            settings = Settings.Instance.UFO;
            var cooldownEntity = EntityManager.CreateEntity(typeof(UFOSpawnCoodown), typeof(Cooldown));
            EntityManager.SetComponentData(cooldownEntity, new Cooldown() { Value = settings.SpawnCooldown });
        }

        protected override void OnUpdate()
        {
            var cooldownEntity = GetEntityQuery(typeof(UFOSpawnCoodown)).GetSingletonEntity();

            if (HasComponent<IsCooldownComplete>(cooldownEntity))
            {
                CreateUFO();
                EntityManager.AddComponentData(cooldownEntity, new Cooldown() { Value = settings.SpawnCooldown });
            }
        }

        private void CreateUFO()
        {
            var minSpeed = settings.MinSpeed;
            var maxSpeed = settings.MaxSpeed;
            var smallSizeFactor = settings.SmallSizeFactor;
            var bigSizeFactor = settings.BigSizeFactor;
            var colorID = settings.ColorID;

            var speed = UnityEngine.Random.Range(minSpeed, maxSpeed);

            var randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

            var direction = new float2(randomDirection.x, randomDirection.y);

            var archetype = EntityManager.CreateArchetype(typeof(UFO), typeof(Velocity), typeof(Warpable), typeof(Collidable));
            var ufoEntity = EntityManager.CreateEntity(archetype);

            EntityManager.SetComponentData(ufoEntity, new Velocity() { Value = direction * speed });

            var ufoType = UnityEngine.Random.value > 0.5f ? UFO.UFOType.Big : UFO.UFOType.Small;

            var mustSpawnLeftSide = UnityEngine.Random.value > 0.5f;

            var position = mustSpawnLeftSide ? new float2(ScreenHelper.MaxBoundX, UnityEngine.Random.Range(ScreenHelper.MinBoundY, ScreenHelper.MaxBoundY)) :
                new float2(ScreenHelper.MinBoundX, UnityEngine.Random.Range(ScreenHelper.MinBoundY, ScreenHelper.MaxBoundY));

            var sizeFactor = ufoType == UFO.UFOType.Big ? bigSizeFactor : smallSizeFactor;

            EntityManager.SetComponentData(ufoEntity, new UFO() { Type = ufoType });

            EntityCreationHelper.AddViewComponents(ufoEntity,
                EntityManager,
                position,
                colorID,
                new float2(0, 0.22f) * sizeFactor,
                new float2(0.2f, 0.18f) * sizeFactor,
                new float2(0.3f, 0f) * sizeFactor,
                new float2(0.4f, 0f) * sizeFactor,
                new float2(0.5f, -0.2f) * sizeFactor,
                new float2(-0.5f, -0.2f) * sizeFactor,
                new float2(-0.4f, 0f) * sizeFactor,
                new float2(-0.3f, 0f) * sizeFactor,
                new float2(-0.2f, 0.18f) * sizeFactor
                );
        }
    }
}