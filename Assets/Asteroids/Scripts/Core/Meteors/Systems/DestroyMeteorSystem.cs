using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class DestroyMeteorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var endSimulationSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = endSimulationSingleton.CreateCommandBuffer(World.Unmanaged);

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
                            var doCreateEntity = ecb.CreateEntity();
                            ecb.AddComponent(doCreateEntity, new DoCreateMeteor() { Position = position.Value, Size = sizeToCreate });
                        }

                    ecb.DestroyEntity(e);
                    ecb.DestroyEntity(otherEntity);
                }
            }).WithoutBurst().Run();
        }
    }
}