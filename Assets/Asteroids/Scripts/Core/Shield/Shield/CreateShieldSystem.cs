using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class CreateShieldSystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;
        private ShipSettings shipSettings;

        protected override void OnCreate()
        {
            shipSettings = Settings.Instance.Ship;
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();
            var shieldQuery = GetEntityQuery(typeof(Shield));
            Entities.WithAll<Powerup, ShieldPowerup>().ForEach((Entity entity, IsColliding isColliding) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<Ship>(otherEntity))
                {
                    if (shieldQuery.IsEmpty)
                    {
                        var shieldEntity = commandBuffer.CreateEntity();

                        var shipPosition = GetComponent<Position>(otherEntity);
                        commandBuffer.AddComponent(shieldEntity, new Shield());
                        commandBuffer.AddComponent(shieldEntity, new Collidable());

                        var sizeFactor = 1f;

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
                            commandBuffer,
                            shipPosition.Value,
                            shipSettings.ColorID,
                            points
                            );
                    }

                    commandBuffer.DestroyEntity(entity);
                }
            }).WithoutBurst().Run();
        }
    }
}