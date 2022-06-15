using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class ShipCreationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            CreateShip();
        }

        private void CreateShip()
        {
            var shipSettings = Settings.Instance.Ship;

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var shipEntity = entityManager.CreateEntity(typeof(Ship), typeof(Velocity));
            var size = shipSettings.Size;

            EntityCreationHelper.AddViewComponents(shipEntity,
                entityManager,
                shipSettings.StartingPosition,
                shipSettings.ColorID,
                new float2(0, 0.5f) * size,
                new float2(0.5f, -0.5f) * size,
                new float2(0.3f, -0.35f) * size,
                new float2(-0.3f, -0.35f) * size,
                new float2(-0.5f, -0.5f) * size
                );
        }

        protected override void OnUpdate()
        {
        }
    }
}