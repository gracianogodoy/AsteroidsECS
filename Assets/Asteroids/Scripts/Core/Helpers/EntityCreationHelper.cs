using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public static class EntityCreationHelper
    {
        public static void AddViewComponents(Entity entity, EntityManager entityManager, float2 startingPosition, byte colorId, params float2[] points)
        {
            entityManager.AddComponentData(entity, new Position()
            {
                Value = startingPosition
            });

            entityManager.AddComponentData(entity, new Rotation());

            entityManager.AddComponentData(entity, new ColorID()
            {
                Value = colorId
            });

            var pointBuffer = entityManager.AddBuffer<Points>(entity);

            foreach (var point in points)
            {
                pointBuffer.Add(new Points() { Value = point });
            }
        }
    }
}