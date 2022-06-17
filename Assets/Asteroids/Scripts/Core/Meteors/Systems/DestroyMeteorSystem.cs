using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class DestroyMeteorSystem : SystemBase
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

            Entities.ForEach((Entity e, in IsColliding isColliding, in Meteor meteor, in Position position) =>
            {
                var otherEntity = isColliding.OtherEntity;

                if (HasComponent<ShipBullet>(otherEntity))
                {
                    var sizeToCreate = Meteor.MeteorSize.Medium;

                    if (meteor.Size == Meteor.MeteorSize.Medium)
                        sizeToCreate = Meteor.MeteorSize.Small;

                    if (meteor.Size != Meteor.MeteorSize.Small)
                        for (int i = 0; i < 2; i++)
                        {
                            var doCreateEntity = commandBuffer.CreateEntity();
                            commandBuffer.AddComponent(doCreateEntity, new DoCreateMeteor() { Position = position.Value, Size = sizeToCreate });
                        }

                    commandBuffer.DestroyEntity(e);
                    commandBuffer.DestroyEntity(otherEntity);
                }
            }).WithoutBurst().Run();
        }
    }
}