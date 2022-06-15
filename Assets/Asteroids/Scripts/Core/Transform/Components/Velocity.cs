using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public struct Velocity : IComponentData
    {
        public float2 Value;
    }
}