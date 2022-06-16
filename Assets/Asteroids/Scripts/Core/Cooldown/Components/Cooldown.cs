using Unity.Entities;

namespace Asteroids.Core
{
    public struct Cooldown : IComponentData
    {
        public float Value;
    }
}