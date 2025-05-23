﻿using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public partial class UFOShootSystem : SystemBase
    {
        private UFOSettings settings;

        protected override void OnUpdate()
        {
            var endSimulationSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);
            
            settings = Settings.Instance.UFO;
            var archetype = EntityManager.CreateArchetype(typeof(Velocity), typeof(MustDestroyOutsideScreen), typeof(UFOBullet), typeof(Collider));
            var shootCooldown = settings.ShootCooldown;
            var bulletSpeed = settings.BulletSpeed;
            var bulletColorID = settings.BulletColorID;

            var shipQuery = GetEntityQuery(typeof(Ship));

            Entities.ForEach((Entity entity, in UFO ufo, in Position position) =>
            {
                if (!HasComponent<Cooldown>(entity))
                {
                    ecb.AddComponent(entity, new Cooldown() { Value = shootCooldown });
                }

                if (HasComponent<IsCooldownComplete>(entity))
                {
                    var bulletEntity = ecb.CreateEntity(archetype);

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

                    ecb.SetComponent(bulletEntity, new Velocity() { Value = direction * bulletSpeed });
                    ecb.SetComponent(bulletEntity, new Collider() { Layer = 0 });

                    var points = new NativeArray<float2>(4, Allocator.Temp);
                    points[0] = new float2(0, 0.05f);
                    points[1] = new float2(0.05f, 0);
                    points[2] = new float2(0, -0.05f);
                    points[3] = new float2(-0.05f, 0);

                    EntityCreationHelper.AddBaseComponents(bulletEntity,
                        ecb,
                        position.Value,
                        bulletColorID,
                        points
                        );
                }
            }).WithoutBurst().Run();
        }
    }
}