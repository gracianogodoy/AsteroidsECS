using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class ShipCreationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var shipQuery = GetEntityQuery(typeof(Ship));
            if (shipQuery.IsEmpty)
            {
                CreateShip();
            }
        }

        private void CreateShip()
        {
            var shipSettings = Settings.Instance.Ship;

            var entityManager = EntityManager;

            var shipEntity = entityManager.CreateEntity(typeof(Ship),
                typeof(Velocity),
                typeof(Warpable),
                typeof(FacingDirection),
                typeof(Collidable));
            var sizeFactor = shipSettings.SizeFactor;

            EntityManager.AddComponent<CanShoot>(shipEntity);

            EntityCreationHelper.AddBaseComponents(shipEntity,
                entityManager,
                shipSettings.StartingPosition,
                shipSettings.ColorID,
                new float2(0, 0.5f) * sizeFactor,
                new float2(0.5f, -0.5f) * sizeFactor,
                new float2(0.3f, -0.35f) * sizeFactor,
                new float2(-0.3f, -0.35f) * sizeFactor,
                new float2(-0.5f, -0.5f) * sizeFactor
                );
        }
    }
}