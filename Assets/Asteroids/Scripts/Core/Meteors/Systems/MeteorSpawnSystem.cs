using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(MeteorCreationSystem))]
    public partial class MeteorSpawnSystem : SystemBase
    {
        private MeteorSettings settings;

        protected override void OnCreate()
        {
            settings = Settings.Instance.Meteor;
            EntityManager.CreateEntity(typeof(MeteorSpawnCooldown));
        }

        protected override void OnUpdate()
        {
            var meteorQuery = GetEntityQuery(typeof(Meteor));
            var doCreateMeteorQuery = GetEntityQuery(typeof(DoCreateMeteor));
            var cooldownEntity = GetEntityQuery(typeof(MeteorSpawnCooldown)).GetSingletonEntity();

            if (HasComponent<IsCooldownComplete>(cooldownEntity))
            {
                SpawnMeteors();
            }

            if (doCreateMeteorQuery.IsEmpty && meteorQuery.IsEmpty && !HasComponent<Cooldown>(cooldownEntity))
            {
                EntityManager.AddComponentData(cooldownEntity, new Cooldown() { Value = settings.SpawnCooldown });
            }

            var doResetQuery = GetEntityQuery(typeof(DoReset));
            if (!doResetQuery.IsEmpty)
            {
                if (HasComponent<Cooldown>(cooldownEntity))
                    EntityManager.SetComponentData(cooldownEntity, new Cooldown() { Value = settings.SpawnCooldown });
            }
        }

        private void SpawnMeteors()
        {
            for (int i = 0; i < settings.SpawnAmount; i++)
            {
                var x = UnityEngine.Random.Range(ScreenHelper.MinBoundX, ScreenHelper.MaxBoundX);
                var y = UnityEngine.Random.Range(ScreenHelper.MinBoundY, ScreenHelper.MaxBoundY);

                var randomScreenSide = UnityEngine.Random.Range(0, 4);
                var spawnPosition = new float2(x, y);

                switch (randomScreenSide)
                {
                    case 0:
                        spawnPosition.y = ScreenHelper.MaxBoundY;
                        break;

                    case 1:
                        spawnPosition.x = ScreenHelper.MaxBoundX;
                        break;

                    case 2:
                        spawnPosition.y = ScreenHelper.MinBoundY;
                        break;

                    case 3:
                    default:
                        spawnPosition.x = ScreenHelper.MinBoundX;
                        break;
                }

                var doCreateEntity = EntityManager.CreateEntity();
                EntityManager.AddComponentData(doCreateEntity, new DoCreateMeteor() { Position = spawnPosition, Size = Meteor.MeteorSize.Big });
            }
        }
    }
}