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
                var x = UnityEngine.Random.Range(ScreenHelper.MinBoundX, ScreenHelper.MaxBoundX);
                var y = UnityEngine.Random.Range(ScreenHelper.MinBoundY, ScreenHelper.MaxBoundY);

                var minDistanceFromCenter = 2;

                if (y < minDistanceFromCenter && y > -minDistanceFromCenter)
                {
                    if (x < minDistanceFromCenter && x > 0)
                        x += minDistanceFromCenter;
                    if (x > -minDistanceFromCenter && x < 0)
                        x -= minDistanceFromCenter;
                }

                var spawnPosition = new float2(x, y);
                var doCreateEntity = EntityManager.CreateEntity();
                EntityManager.AddComponentData(doCreateEntity, new DoCreateMeteor() { Position = spawnPosition, Size = Meteor.MeteorSize.Big });
            }
        }

        protected override void OnUpdate()
        {
        }
    }
}