using Unity.Entities;

namespace Asteroids.Core
{
    public struct IsColliding : IComponentData
    {
        public Entity OtherEntity;
    }
}