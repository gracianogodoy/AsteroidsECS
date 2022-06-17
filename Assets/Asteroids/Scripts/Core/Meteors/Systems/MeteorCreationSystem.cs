using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class MeteorCreationSystem : SystemBase
    {
        private MeteorSettings settings;
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            commandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            settings = Settings.Instance.Meteor;
        }

        protected override void OnUpdate()
        {
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();
            var archetype = EntityManager.CreateArchetype(typeof(Meteor), typeof(Velocity), typeof(Warpable), typeof(RotateSpeed), typeof(Collidable));

            var minSpeed = settings.MinSpeed;
            var maxSpeed = settings.MaxSpeed;
            var minRotateSpeed = settings.MinRotateSpeed;
            var maxRotateSpeed = settings.MaxRotateSpeed;
            var smallSizeFactor = settings.SmallSizeFactor;
            var mediumSizeFactor = settings.MediumSizeFactor;
            var bigSizeFactor = settings.BigSizeFactor;
            var colorID = settings.ColorID;

            Entities.ForEach((Entity entity, in DoCreateMeteor doCreateMeteor) =>
            {
                var speed = UnityEngine.Random.Range(minSpeed, maxSpeed);

                var randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

                var direction = new float2(randomDirection.x, randomDirection.y);

                var meteorEntity = commandBuffer.CreateEntity(archetype);

                var size = doCreateMeteor.Size;

                commandBuffer.SetComponent(meteorEntity,
                    new Meteor() { Size = size });
                commandBuffer.SetComponent(meteorEntity,
                    new RotateSpeed()
                    {
                        Value = UnityEngine.Random.Range(minRotateSpeed, maxRotateSpeed)
                    });
                commandBuffer.SetComponent(meteorEntity,
                    new Velocity() { Value = direction * speed });

                float sizeFactor = bigSizeFactor;

                if (size == Meteor.MeteorSize.Medium)
                    sizeFactor = mediumSizeFactor;
                if (size == Meteor.MeteorSize.Small)
                    sizeFactor = smallSizeFactor;

                float minAdjustmentMulplier = 0.7f;
                float maxAdjustmentMulplier = 1.2f;

                var points = new NativeArray<float2>(8, Allocator.Temp);
                points[0] = new float2(-0.19f, 0.5f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier);
                points[1] = new float2(0.19f, 0.5f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier);
                points[2] = new float2(0.5f, 0.19f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier);
                points[3] = new float2(0.5f, -0.19f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier);
                points[4] = new float2(0.19f, -0.5f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier);
                points[5] = new float2(-0.19f, -0.5f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier);
                points[6] = new float2(-0.5f, -0.19f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier);
                points[7] = new float2(-0.5f, 0.19f) * sizeFactor * UnityEngine.Random.Range(minAdjustmentMulplier, maxAdjustmentMulplier);

                EntityCreationHelper.AddViewComponents(meteorEntity,
                    commandBuffer,
                    doCreateMeteor.Position,
                    colorID,
                    points
                    );

                points.Dispose();

                commandBuffer.DestroyEntity(entity);
            }).Run();
        }
    }
}