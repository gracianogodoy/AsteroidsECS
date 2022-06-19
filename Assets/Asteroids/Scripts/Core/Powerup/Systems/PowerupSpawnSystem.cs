using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class PowerupSpawnSystem : SystemBase
    {
        private PowerupsSettings settings;

        protected override void OnCreate()
        {
            settings = Settings.Instance.PowerupsSettings;
            CreateCooldownEntity();
        }

        protected override void OnUpdate()
        {
            var cooldownEntity = GetEntityQuery(typeof(PowerupCooldown)).GetSingletonEntity();

            if (HasComponent<IsCooldownComplete>(cooldownEntity))
            {
                CreateRandomPowerup();
                EntityManager.AddComponentData(cooldownEntity, new Cooldown() { Value = settings.SpawnCooldown });
            }

            var doResetQuery = GetEntityQuery(typeof(DoReset));
            if (!doResetQuery.IsEmpty)
            {
                if (HasComponent<Cooldown>(cooldownEntity))
                    EntityManager.SetComponentData(cooldownEntity, new Cooldown() { Value = settings.SpawnCooldown });
            }
        }

        private void CreateCooldownEntity()
        {
            var cooldownEntity = EntityManager.CreateEntity(typeof(PowerupCooldown), typeof(Cooldown));
            EntityManager.SetComponentData(cooldownEntity, new Cooldown() { Value = settings.SpawnCooldown });
        }

        public void CreateRandomPowerup()
        {
            var mustSpawnLeftSide = UnityEngine.Random.value > 0.5f;

            var position = mustSpawnLeftSide ? new float2(ScreenHelper.MaxBoundX, UnityEngine.Random.Range(ScreenHelper.MinBoundY, ScreenHelper.MaxBoundY)) :
             new float2(ScreenHelper.MinBoundX, UnityEngine.Random.Range(ScreenHelper.MinBoundY, ScreenHelper.MaxBoundY));

            var randomIndex = UnityEngine.Random.Range(0, Powerups.Configs.Length);
            var config = Powerups.Configs[randomIndex];

            var direction = new float2();
            direction.x = mustSpawnLeftSide ? -1f : 1f;

            var entity = EntityManager.CreateEntity(typeof(Powerup),
                typeof(Velocity),
                typeof(MustDestroyOutsideScreen),
                typeof(Collidable),
                config.ComponentType);

            EntityManager.SetComponentData(entity, new Velocity() { Value = direction * settings.Speed });

            EntityCreationHelper.AddBaseComponents(entity,
               EntityManager,
               position,
               config.ColorID,
               config.Points
               );
        }
    }
}