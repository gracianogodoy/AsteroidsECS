using Unity.Entities;

namespace Asteroids.Core
{
    public struct Collider : IComponentData
    {
        public byte Layer;
    }
}