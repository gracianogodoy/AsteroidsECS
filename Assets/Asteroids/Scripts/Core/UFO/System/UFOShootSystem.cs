using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class UFOShootSystem : SystemBase
    {
        private UFOSettings settings;

        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();
            settings = Settings.Instance.UFO;
            var archetype = EntityManager.CreateArchetype(typeof(Velocity), typeof(MustDestroyOutsideScreen), typeof(UFOBullet), typeof(Collidable));
            var shootCooldown = settings.ShootCooldown;
            var bulletSpeed = settings.BulletSpeed;
            var bulletColorID = settings.BulletColorID;

            var shipQuery = GetEntityQuery(typeof(Ship));

            Entities.ForEach((Entity entity, in UFO ufo, in Position position) =>
            {
                if (!HasComponent<Cooldown>(entity))
                {
                    commandBuffer.AddComponent(entity, new Cooldown() { Value = shootCooldown });
                }

                if (HasComponent<IsCooldownComplete>(entity))
                {
                    var bulletEntity = commandBuffer.CreateEntity(archetype);

                    var direction = new float2(1, 0);

                    if (ufo.Type == UFO.UFOType.Big)
                    {
                        var randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
                        direction = new float2(randomDirection.x, randomDirection.y);
                    }
                    else
                    {
                        if (!shipQuery.IsEmpty)
                        {
                            var shipEntity = shipQuery.GetSingletonEntity();
                            var shipPosition = GetComponent<Position>(shipEntity);
                            direction = math.normalize(shipPosition.Value - position.Value);
                        }
                    }

                    commandBuffer.SetComponent(bulletEntity, new Velocity() { Value = direction * bulletSpeed });

                    var points = new NativeArray<float2>(4, Allocator.Temp);
                    points[0] = new float2(0, 0.05f);
                    points[1] = new float2(0.05f, 0);
                    points[2] = new float2(0, -0.05f);
                    points[3] = new float2(-0.05f, 0);

                    EntityCreationHelper.AddViewComponents(bulletEntity,
                        commandBuffer,
                        position.Value,
                        bulletColorID,
                        points
                        );
                }
            }).WithoutBurst().Run();
        }
    }
}