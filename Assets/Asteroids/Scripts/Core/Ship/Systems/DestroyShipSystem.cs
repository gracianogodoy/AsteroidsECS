﻿using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class DestroyShipSystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();
            var entityManager = EntityManager;
            var settings = Settings.Instance.Meteor;

            Entities.WithAll<Ship>().ForEach((Entity e, in IsColliding isColliding) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<Meteor>(otherEntity) || HasComponent<UFOBullet>(otherEntity))
                {
                    commandBuffer.DestroyEntity(e);
                    commandBuffer.DestroyEntity(otherEntity);
                }
            }).WithoutBurst().Run();
        }
    }
}