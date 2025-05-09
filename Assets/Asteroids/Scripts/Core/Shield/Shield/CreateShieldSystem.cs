using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class CreateShieldSystem : SystemBase
    {
        private ShipSettings shipSettings;

        protected override void OnCreate()
        {
            shipSettings = Settings.Instance.Ship;
        }

        protected override void OnUpdate()
        {
            var endSimulationSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);
            var shieldQuery = GetEntityQuery(typeof(Shield));
            Entities.WithAll<Powerup, ShieldPowerup>().ForEach((Entity entity, IsColliding isColliding) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<Ship>(otherEntity))
                {
                    if (shieldQuery.IsEmpty)
                    {
                        var shieldEntity = ecb.CreateEntity();

                        var shipPosition = GetComponent<Position>(otherEntity);
                        ecb.AddComponent(shieldEntity, new Shield());
                        ecb.AddComponent(shieldEntity, new Collider() { Layer = 1 });

                        var sizeFactor = 0.7f;

                        var points = new NativeArray<float2>(8, Allocator.Temp);
                        points[0] = new float2(-0.19f, 0.5f) * sizeFactor;
                        points[1] = new float2(0.19f, 0.5f) * sizeFactor;
                        points[2] = new float2(0.5f, 0.19f) * sizeFactor;
                        points[3] = new float2(0.5f, -0.19f) * sizeFactor;
                        points[4] = new float2(0.19f, -0.5f) * sizeFactor;
                        points[5] = new float2(-0.19f, -0.5f) * sizeFactor;
                        points[6] = new float2(-0.5f, -0.19f) * sizeFactor;
                        points[7] = new float2(-0.5f, 0.19f) * sizeFactor;

                        EntityCreationHelper.AddBaseComponents(shieldEntity,
                            ecb,
                            shipPosition.Value,
                            shipSettings.ColorID,
                            points
                            );
                    }

                    ecb.DestroyEntity(entity);
                }
            }).WithoutBurst().Run();
        }
    }
}