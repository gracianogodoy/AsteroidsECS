using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class PowerupSpawnSystem : SystemBase
    {
        public const float Cooldown = 1f;
        public const float Speed = 1f;

        protected override void OnCreate()
        {
            CreateCooldownEntity();
        }

        protected override void OnUpdate()
        {
            var cooldownEntity = GetEntityQuery(typeof(PowerupCooldown)).GetSingletonEntity();

            if (HasComponent<IsCooldownComplete>(cooldownEntity))
            {
                CreatePowerup();
                EntityManager.AddComponentData(cooldownEntity, new Cooldown() { Value = Cooldown });
            }

            var doResetQuery = GetEntityQuery(typeof(DoReset));
            if (!doResetQuery.IsEmpty)
            {
                if (HasComponent<Cooldown>(cooldownEntity))
                    EntityManager.SetComponentData(cooldownEntity, new Cooldown() { Value = Cooldown });
            }
        }

        private void CreateCooldownEntity()
        {
            var cooldownEntity = EntityManager.CreateEntity(typeof(PowerupCooldown), typeof(Cooldown));
            EntityManager.SetComponentData(cooldownEntity, new Cooldown() { Value = Cooldown });
        }

        private void CreatePowerup()
        {
            var archetype = EntityManager.CreateArchetype(typeof(DoCreatePowerup));
            var entity = EntityManager.CreateEntity(archetype);

            var mustSpawnLeftSide = UnityEngine.Random.value > 0.5f;

            var position = mustSpawnLeftSide ? new float2(ScreenHelper.MaxBoundX, UnityEngine.Random.Range(ScreenHelper.MinBoundY, ScreenHelper.MaxBoundY)) :
                new float2(ScreenHelper.MinBoundX, UnityEngine.Random.Range(ScreenHelper.MinBoundY, ScreenHelper.MaxBoundY));

            var id = UnityEngine.Random.Range(0, PowerupUtils.PowerupUpAmount);

            var direction = new float2();
            direction.x = mustSpawnLeftSide ? -1f : 1f;

            EntityManager.SetComponentData(entity, new DoCreatePowerup() { ID = id, Position = position, Velocity = direction * Speed });
        }
    }
}