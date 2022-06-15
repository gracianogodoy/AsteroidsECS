using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public struct Position : IComponentData
    {
        public float2 Value;
    }
}